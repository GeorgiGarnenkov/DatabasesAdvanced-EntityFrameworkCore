using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using ProductShop.App.Dto;
using ProductShop.App.Dto.Export;
using ProductShop.Data;
using ProductShop.Models;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace ProductShop.App
{
    public class StartUp
    {
        public static void Main()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            var mapper = config.CreateMapper();

            #region Import
            // --------- ADD USERS ---------
            //var xmlString = File.ReadAllText(path: "..\\..\\..\\Xml\\users.xml");
            //
            //var serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("users"));
            //var deserializedUser = (UserDto[])serializer.Deserialize(new StringReader(xmlString));
            //
            //List<User> users = new List<User>();
            //foreach (var userDto in deserializedUser)
            //{
            //    if (!IsValid(userDto))
            //    {
            //        continue;
            //    }
            //
            //    var user = mapper.Map<User>(userDto);
            //
            //    users.Add(user);
            //}
            //
            //var context = new ProductShopDbContext();
            //
            //context.Users.AddRange(users);
            //
            //context.SaveChanges();


            // --------- ADD PRODUCTS ---------
            //var xmlString = File.ReadAllText(path: "..\\..\\..\\Xml\\products.xml");
            //
            //var serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("products"));
            //var deserializedProduct = (ProductDto[])serializer.Deserialize(new StringReader(xmlString));
            //
            //List<Product> products = new List<Product>();
            //
            //int counter = 1;
            //
            //foreach (var productDto in deserializedProduct)
            //{
            //    if (!IsValid(productDto))
            //    {
            //        continue;
            //    }
            //
            //    var product = mapper.Map<Product>(productDto);
            //
            //    var buyerId = new Random().Next(1, 30);
            //    var sellerId = new Random().Next(31, 56);
            //
            //    product.BuyerId = buyerId;
            //    product.SellerId = sellerId;
            //
            //    if (counter == 4)
            //    {
            //        product.BuyerId = null;
            //        counter = 0;
            //    }
            //
            //    products.Add(product);
            //
            //    counter++;
            //}
            //
            //var context = new ProductShopDbContext();
            //
            //context.Products.AddRange(products);
            //
            //context.SaveChanges();


            // --------- ADD CATEGORIES ---------
            //var xmlString = File.ReadAllText(path: "..\\..\\..\\Xml\\categories.xml");
            //
            //var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("categories"));
            //var deserializedCategories = (CategoryDto[])serializer.Deserialize(new StringReader(xmlString));
            //
            //List<Category> categories = new List<Category>();
            //
            //foreach (var categoryDto in deserializedCategories)
            //{
            //    if (!IsValid(categoryDto))
            //    {
            //        continue;
            //    }
            //
            //    var category = mapper.Map<Category>(categoryDto);
            //
            //    categories.Add(category);
            //}
            //
            //var context = new ProductShopDbContext();
            //
            //context.Categories.AddRange(categories);
            //
            //context.SaveChanges();



            // --------- ADD CATEGORY-PRODUCTS ---------
            //var categoryProducts = new List<CategoryProduct>();
            //
            //for (int productId = 1; productId <= 200; productId++)
            //{
            //    var categoryId = new Random().Next(1, 12);
            //
            //    var categoryProduct = new CategoryProduct()
            //    {
            //        CategoryId = categoryId,
            //        ProductId = productId
            //    };
            //
            //    categoryProducts.Add(categoryProduct);
            //}
            //
            //var context = new ProductShopDbContext();
            //
            //context.CategoryProducts.AddRange(categoryProducts);
            //
            //context.SaveChanges();

            #endregion


            #region Export
            //var context = new ProductShopDbContext();
            //
            //var products = context.Products
            //    .Where(x => x.Price >= 100 && x.Price <= 2000 && x.Buyer != null)
            //    .OrderBy(x => x.Price)
            //    .Select(x => new ProductDto
            //    {
            //        Name = x.Name,
            //        Price = x.Price,
            //        Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName ?? x.Buyer.LastName
            //    })
            //    .ToArray();
            //
            //var sb = new StringBuilder();
            //var xmlNamespace =new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty});
            //
            //var serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("products"));
            //serializer.Serialize(new StringWriter(sb), products, xmlNamespace);
            //
            //File.WriteAllText("..\\..\\..\\Xml\\products-in-range.xml", sb.ToString());



            //var context = new ProductShopDbContext();
            //
            //var users = context.Users
            //    .Where(x => x.ProductsSold.Count >= 1)
            //    .Select(x => new UserExportDto
            //    {
            //        FirstName = x.FirstName,
            //        LastName = x.LastName,
            //        SoldProducts = x.ProductsSold.Select(s => new SoldProductDto
            //        {
            //            Name = s.Name,
            //            Price = s.Price
            //        }).ToArray()
            //    })
            //    .OrderBy(x => x.LastName)
            //    .ThenBy(x => x.FirstName)
            //    .ToArray();
            //
            //var sb = new StringBuilder();
            //var xmlNamespace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            //
            //var serializer = new XmlSerializer(typeof(UserExportDto[]), new XmlRootAttribute("users"));
            //serializer.Serialize(new StringWriter(sb), users, xmlNamespace);
            //
            //File.WriteAllText("..\\..\\..\\Xml\\users-sold-products.xml", sb.ToString());



            //var context = new ProductShopDbContext();
            //
            //var categories = context.Categories
            //    .OrderByDescending(x => x.CategoryProducts.Count)
            //    .Select(x => new CategoryExportDto
            //    {
            //        Name = x.Name,
            //        ProductCount = x.CategoryProducts.Count,
            //
            //        TotalRevenue = x.CategoryProducts
            //                        .Sum(s => s.Product.Price),
            //
            //        AveragePrice = x.CategoryProducts
            //                        .Select(s => s.Product.Price)
            //                        .DefaultIfEmpty(0)
            //                        .Average()
            //
            //    })
            //    .ToArray();
            //
            //var sb = new StringBuilder();
            //var xmlNamespace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            //
            //var serializer = new XmlSerializer(typeof(CategoryExportDto[]), new XmlRootAttribute("categories"));
            //serializer.Serialize(new StringWriter(sb), categories, xmlNamespace);
            //
            //File.WriteAllText("..\\..\\..\\Xml\\categories-by-products.xml", sb.ToString());


            
           //var context = new ProductShopDbContext();
           //var users = new UsersExportDto
           //{
           //    Count = context.Users.Count(),
           //    Users = context.Users
           //        .Where(x => x.ProductsSold.Count >= 1)
           //        .Select(x => new UserDto
           //        {
           //            FirstName = x.FirstName,
           //            LastName = x.LastName,
           //            Age = x.Age.ToString(),
           //            SoldProducts = new SoldProductsExportDto
           //            {
           //                Count = x.ProductsSold.Count(),
           //                ProductDtos = x.ProductsSold.Select(k => new ProductDto
           //                {
           //                    Name = k.Name,
           //                    Price = k.Price
           //                }).ToArray()
           //            }
           //        }).ToArray()
           //};
           //
           //var sb = new StringBuilder();
           //var xmlNamespace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
           //
           //var serializer = new XmlSerializer(typeof(UsersExportDto), new XmlRootAttribute("users"));
           //serializer.Serialize(new StringWriter(sb), users, xmlNamespace);
           //
           //File.WriteAllText("..\\..\\..\\Xml\\users-and-products.xml", sb.ToString());

            #endregion

        }

        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}
