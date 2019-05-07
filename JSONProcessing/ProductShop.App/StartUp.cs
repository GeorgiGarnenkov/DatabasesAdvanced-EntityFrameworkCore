using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ProductShop.App
{
    using Data;
    using Models;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();


            //Console.WriteLine(AddUsers(context));
            //Console.WriteLine(AddProducts(context));
            //Console.WriteLine(AddCategories(context));
            //Console.WriteLine(AddCategoryProduct(context));

            //Console.WriteLine(ExportProductsInRange(context));
            //Console.WriteLine(ExportUsersSoldProducts(context));
            //Console.WriteLine(ExportCategoriesByProducts(context));
            //Console.WriteLine(ExportUsersAndProducts(context));

        }
        
        #region Import
        private static string AddUsers(ProductShopContext context)
        {
            var jsonString = File.ReadAllText("..\\..\\..\\Json\\users.json");
            var deserializedUsers = JsonConvert.DeserializeObject<User[]>(jsonString);

            List<User> users = new List<User>();

            foreach (User user in deserializedUsers)
            {
                if (IsValid(user))
                {
                    users.Add(user);
                }
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return "Users are added to database!";
        }

        private static string AddProducts(ProductShopContext context)
        {
            var jsonString = File.ReadAllText("..\\..\\..\\Json\\products.json");
            var deserializedProduct = JsonConvert.DeserializeObject<Product[]>(jsonString);

            List<Product> products = new List<Product>();

            int counter = 1;

            foreach (var product in deserializedProduct)
            {
                if (IsValid(product))
                {
                    var buyerId = new Random().Next(1, 30);
                    var sellerId = new Random().Next(31, 57);

                    product.BuyerId = buyerId;
                    product.SellerId = sellerId;

                    if (counter == 4)
                    {
                        product.BuyerId = null;
                        counter = 0;
                    }

                    products.Add(product);

                    counter++;
                }
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return "Products are added to database!";
        }

        private static string AddCategories(ProductShopContext context)
        {
            var jsonString = File.ReadAllText("..\\..\\..\\Json\\categories.json");
            var deserializedCategories = JsonConvert.DeserializeObject<Category[]>(jsonString);

            List<Category> categories = new List<Category>();

            foreach (Category category in deserializedCategories)
            {
                if (IsValid(category))
                {
                    categories.Add(category);
                }
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return "Categories are added to database!";
        }

        private static string AddCategoryProduct(ProductShopContext context)
        {
            var categoryProducts = new List<CategoryProduct>();

            for (int productId = 1; productId <= 200; productId++)
            {
                var categoryId = new Random().Next(1, 12);

                var categoryProduct = new CategoryProduct()
                {
                    CategoryId = categoryId,
                    ProductId = productId
                };

                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return "Category-Products are added!";
        }
        #endregion

        #region Export
        private static string ExportProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName ?? x.Seller.LastName
                })
                .ToArray();

            var jsonProducts = JsonConvert.SerializeObject(products, Formatting.Indented);

            File.WriteAllText("..\\..\\..\\JsonExport\\products-in-range.json", jsonProducts);

            return "Products In Range Exported!";
        }

        private static string ExportUsersSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Count >= 1 && x.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold
                                    .Where(p => p.Buyer != null)
                                    .Select(z => new
                                    {
                                        name = z.Name,
                                        price = z.Price,
                                        buyerFirstName = z.Buyer.FirstName,
                                        buyerLastName = z.Buyer.LastName
                                    })
                                    .ToArray()
                })
                .ToArray();

            var jsonUsers = JsonConvert.SerializeObject(users, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllText("..\\..\\..\\JsonExport\\users-sold-products.json", jsonUsers);

            return "Users Sold Products Exported!";
        }

        private static string ExportCategoriesByProducts(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Count,
                    averagePrice = x.CategoryProducts
                                    .Select(s => s.Product.Price)
                                    .DefaultIfEmpty(0)
                                    .Average(),
                    totalRevenue = x.CategoryProducts
                                    .Sum(s => s.Product.Price)
                })
                .ToArray();

            var jsonCategories = JsonConvert.SerializeObject(categories, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllText("..\\..\\..\\JsonExport\\categories-by-products.json", jsonCategories);

            return "Categories By Products Exported!";
        }

        private static string ExportUsersAndProducts(ProductShopContext context)
        {
            var users = new
            {
                usersCount = context.Users.Count(),
                users = context.Users
                    .OrderByDescending(x => x.ProductsSold.Count)
                    .ThenBy(x => x.LastName)
                    .Where(x => x.ProductsSold.Count >= 1 && x.ProductsSold.Any(s => s.Buyer != null))
                    .Select(x => new
                    {
                        firstName = x.FirstName,
                        lastName = x.LastName,
                        age = x.Age,
                        soldProducts = new
                        {
                            count = x.ProductsSold.Count,
                            products = x.ProductsSold.Select(z => new
                            {
                                name = z.Name,
                                price = z.Price
                            })
                        }
                    })
            };


            var jsonUsers = JsonConvert.SerializeObject(users, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllText("..\\..\\..\\JsonExport\\users-and-products.json", jsonUsers);

            return "Users And Products Exported!";
        }

        #endregion

        public static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}
