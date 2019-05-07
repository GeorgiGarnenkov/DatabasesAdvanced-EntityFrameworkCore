using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            StringBuilder sb = new StringBuilder();

		    var deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            List<Employee> employees = new List<Employee>();

		    foreach (EmployeeDto employeeDto in deserializedEmployees)
		    {
		        if (!IsValid(employeeDto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

		        Position position = GetPosition(context, employeeDto.Position);

		        var employee = new Employee
		        {
                    Name= employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = position
		        };

                employees.Add(employee);

		        sb.AppendLine(string.Format(SuccessMessage, employee.Name));
		    }

            context.Employees.AddRange(employees);
		    context.SaveChanges();


		    return sb.ToString().TrimEnd();
		}

	    

	    public static string ImportItems(FastFoodDbContext context, string jsonString)
		{

		    StringBuilder sb = new StringBuilder();

		    var deserializedItem = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

		    List<Item> items = new List<Item>();

		    foreach (ItemDto itemDto in deserializedItem)
		    {
		        if (!IsValid(itemDto))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        var itemExists = items.Any(x => x.Name == itemDto.Name);
		        if (itemExists)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        Category category = GetCategory(context, itemDto.Category);

		        var item = new Item
		        {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Category = category
		        };

		        items.Add(item);
                

                sb.AppendLine(string.Format(SuccessMessage, item.Name));
		    }

		    context.Items.AddRange(items);
		    context.SaveChanges();

		    return sb.ToString().TrimEnd();
        }

	    

	    public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var deserializedOrders = (OrderDto[])serializer.Deserialize(new StringReader(xmlString));

		    List<OrderItem> orderItems = new List<OrderItem>();
            List<Order> orders = new List<Order>();

		    foreach (var orderDto in deserializedOrders)
		    {
		        bool isValidItem = true;

		        if (!IsValid(orderDto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

		        foreach (var orderItem in orderDto.OrderItems)
		        {
		            if (!IsValid(orderItem))
		            {
		                sb.AppendLine(FailureMessage);
		                isValidItem = false;
		                break;
		            }
		        }

		        if (!isValidItem)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var employee = context.Employees.FirstOrDefault(x => x.Name == orderDto.Employee);
		        if (employee == null)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var areValidItems = AreValidItems(context, orderDto.OrderItems);

		        if (!areValidItems)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        var date = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
		        var type = Enum.Parse<OrderType>(orderDto.Type);
                var order = new Order
                {
                    Customer = orderDto.Customer,
                    Employee = employee,
                    DateTime = date,
                    Type = type,

                };
                orders.Add(order);

		        foreach (var itemDto in orderDto.OrderItems)
		        {
		            var item = context.Items.FirstOrDefault(x => x.Name == itemDto.Name);
		            var orderItem = new OrderItem
		            {
		                Order = order,
		                Item = item,
		                Quantity = itemDto.Quantity
		            }; 

                    orderItems.Add(orderItem);
		        }

		        sb.AppendLine($"Order for {orderDto.Customer} on {date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
		    }

            context.Orders.AddRange(orders);
		    context.SaveChanges();

            context.OrderItems.AddRange(orderItems);
		    context.SaveChanges();

		    return sb.ToString().TrimEnd();
		}

	    private static bool AreValidItems(FastFoodDbContext context, OrderItemDto[] OrderItems)
	    {
	        foreach (var item in OrderItems)
	        {
	            var itemExists = context.Items.Any(x => x.Name == item.Name);

	            if (!itemExists)
	            {
	                return false;
	            }
	        }

	        return true;
	    }

	    private static Position GetPosition(FastFoodDbContext context, string positionName)
	    {
	        var position = context.Positions.FirstOrDefault(x => x.Name == positionName);

	        if (position == null)
	        {
	            position = new Position
	            {
                    Name = positionName
	            };

	            context.Positions.Add(position);
	            context.SaveChanges();
	        }

	        return position;
	    }

	    private static Category GetCategory(FastFoodDbContext context, string categoryName)
	    {
	        var category = context.Categories.FirstOrDefault(x => x.Name == categoryName);

	        if (category == null)
	        {
	            category = new Category
	            {
	                Name = categoryName
	            };

	            context.Categories.Add(category);
	            context.SaveChanges();
	        }

	        return category;
        }

        public static bool IsValid(object obj)
	    {
	        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
	        var validationResult = new List<ValidationResult>();
	        return Validator.TryValidateObject(obj, validationContext, validationResult, true);
	    }
    }
}