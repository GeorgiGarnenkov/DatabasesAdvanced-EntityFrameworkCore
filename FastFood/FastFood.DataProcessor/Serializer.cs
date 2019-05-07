using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models.Enums;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace FastFood.DataProcessor
{
	public class Serializer
	{
		public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
		{
		    var orderTypeAsEnum = Enum.Parse<OrderType>(orderType);

		    var employee = context.Employees
		        .ToArray()
		        .Where(x => x.Name == employeeName)
		        .Select(x => new
		        {
		            Name = x.Name,
		            Orders = x.Orders.Where(s => s.Type == orderTypeAsEnum)
		                .Select(z => new
		                             {
                                         Customer = z.Customer,
                                         Items = z.OrderItems
                                             .Select(oi => new
                                                           {
                                                               Name = oi.Item.Name,
                                                               Price = oi.Item.Price,
                                                               Quantity = oi.Quantity
                                                           }).ToArray(),
                                         TotalPrice = z.TotalPrice
		                             }).OrderByDescending(obd => obd.TotalPrice)
		                               .ThenByDescending(tbd => tbd.Items.Length)
                                       .ToArray(),
                    TotalMade = x.Orders.Where(s => s.Type == orderTypeAsEnum)
                                        .Sum(tm => tm.TotalPrice)
		        }).FirstOrDefault();

		    var jsonJsonAsString = JsonConvert.SerializeObject(employee, Formatting.Indented);

		    return jsonJsonAsString;
        }

		public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
		{
		    var catergoriesArray = categoriesString.Split(',');

		    var categories = context.Categories
		        .Where(x => catergoriesArray.Any(s => s == x.Name))
		        .Select(s => new CategoryDto
		        {
		            Name = s.Name,
		            MostPopularItem = s.Items.Select(z => new MostPopularItemDto
		                {
		                    Name = z.Name,
		                    TimesSold = z.OrderItems.Sum(x => x.Quantity),
		                    TotalMade = z.OrderItems.Sum(x => x.Item.Price * x.Quantity)
		                })
		                .OrderByDescending(x => x.TotalMade)
		                .ThenByDescending(x => x.TimesSold)
		                .FirstOrDefault()
		        })
		        .OrderByDescending(x => x.MostPopularItem.TotalMade)
		        .ThenByDescending(x => x.MostPopularItem.TimesSold)
		        .ToArray();

		    StringBuilder sb = new StringBuilder();

		    var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
		    var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));
		    serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

		    return sb.ToString();
        }
	}
}