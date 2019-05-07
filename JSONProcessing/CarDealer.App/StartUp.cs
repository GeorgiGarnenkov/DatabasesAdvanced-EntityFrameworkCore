namespace CarDealer.App
{
    using Microsoft.EntityFrameworkCore;

    using Models;
    using Data;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(ResetDB());
            Console.WriteLine(Import());

            Console.WriteLine(OrderedCustomers());
            Console.WriteLine(ToyotaCars());
            Console.WriteLine(LocalSuppliers());
            Console.WriteLine(CarsAndParts());
            Console.WriteLine(TotalSalesByCustomer());
            Console.WriteLine(SalesWithAppliedDiscount());
        }

        #region Json Export

        private static string OrderedCustomers()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var customers = context.Customers
                    .OrderBy(x => x.BirthDate)
                    .ThenByDescending(x => x.IsYoungDriver)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Name,
                        BirthDate = x.BirthDate,
                        IsYoungDriver = x.IsYoungDriver,
                        Sales = x.Sales
                    })
                    .ToArray();

                var jsonCustomers = JsonConvert.SerializeObject(customers, Formatting.Indented);

                File.WriteAllText("..\\..\\..\\Export\\ordered-customers.json", jsonCustomers);

                return $"{customers.Length} customers exported!";
            }
        }

        private static string ToyotaCars()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var cars = context.Cars
                    .Where(x => x.Make == "Toyota")
                    .Select(x => new
                    {
                        Id = x.Id,
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance
                    })
                    .OrderBy(x => x.Model)
                    .ThenByDescending(x => x.TravelledDistance)
                    .ToArray();

                var jsonCars = JsonConvert.SerializeObject(cars, Formatting.Indented);

                File.WriteAllText("..\\..\\..\\Export\\toyota-cars.json", jsonCars);

                return $"Toyota cars exported!";
            }
        }

        private static string LocalSuppliers()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var suppliers = context.Suppliers
                    .Where(x => !x.IsImporter)
                    .Select(s => new
                    {
                        Id = s.Id,
                        Name = s.Name,
                        PartsCount = s.Parts.Count()
                    })
                    .ToArray();

                var jsonSuppliers = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

                File.WriteAllText("..\\..\\..\\Export\\local-suppliers.json", jsonSuppliers);

                return $"Local Suppliers exported!";
            }
        }

        private static string CarsAndParts()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var carsParts = context.Cars
                    .Select(x => new
                    {
                        car = new
                        {
                            Make = x.Make,
                            Model = x.Model,
                            TravelledDistance = x.TravelledDistance
                        },

                        parts = x.PartsCars
                                            .Select(z => new
                                            {
                                                Name = z.Part.Name,
                                                Price = z.Part.Price
                                            }).ToArray()
                    }).ToArray();

                var jsonCarsParts = JsonConvert.SerializeObject(carsParts, Formatting.Indented);

                File.WriteAllText("..\\..\\..\\Export\\cars-and-parts.json", jsonCarsParts);

                return $"Cars along with their list of Parts - exported!";
            }
        }

        private static string TotalSalesByCustomer()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var customers = context.Customers
                    .Where(x => x.Sales.Count > 0)
                    .Select(x => new
                    {
                        fullName = x.Name,
                        boughtCars = x.Sales.Count,
                        spentMoney = x.Sales.Sum(z => z.Car.PartsCars.Sum(a => a.Part.Price))
                    })
                    .OrderByDescending(x => x.spentMoney)
                    .ThenByDescending(x => x.boughtCars)
                    .ToArray();


                var jsonCustomers = JsonConvert.SerializeObject(customers, Formatting.Indented);

                File.WriteAllText("..\\..\\..\\Export\\customers-total-sales.json", jsonCustomers);

                return $"Total Sales by Customer - exported!";
            }
        }

        private static string SalesWithAppliedDiscount()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var sales = context.Sales
                    .Select(s => new
                    {
                        car = new
                        {
                            Make = s.Car.Make,
                            Model = s.Car.Model,
                            TravelledDistance = s.Car.TravelledDistance
                        },
                        customerName = s.Customer.Name,
                        Discount = s.Discount,
                        price = s.Car.PartsCars.Sum(cp => cp.Part.Price),
                        priceWithDiscount = s.Car.PartsCars.Sum(cp => cp.Part.Price) * ((100 - s.Discount) / 100)
                    })
                    .ToArray();

                var jsonSales = JsonConvert.SerializeObject(sales, Formatting.Indented);

                File.WriteAllText("..\\..\\..\\Export\\sales-discounts.json", jsonSales);

                return $"Sales with applied Discount - exported!";
            }
        }


        #endregion

        #region Json Import

        private static string Import()
        {
            var jsonSuppliers = File.ReadAllText(@"..\..\..\Import\suppliers.json");

            var deserializedSuppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsonSuppliers);

            var jsonParts = File.ReadAllText(@"..\..\..\Import\parts.json");

            var deserializedParts = JsonConvert.DeserializeObject<List<Part>>(jsonParts);

            var jsonCars = File.ReadAllText(@"..\..\..\Import\cars.json");

            var deserializedCars = JsonConvert.DeserializeObject<List<Car>>(jsonCars);

            var jsonCustomers = File.ReadAllText(@"..\..\..\Import\customers.json");

            var deserializedCustomers = JsonConvert.DeserializeObject<List<Customer>>(jsonCustomers);

            using (var context = new CarDealerContext())
            {

                // ------------ Suppliers
                var suppliers = new List<Supplier>();

                foreach (var supplier in deserializedSuppliers)
                {
                    if (IsValid(supplier))
                    {
                        suppliers.Add(supplier);
                    }
                }

                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();

                // ------------ Parts
                var parts = new List<Part>();

                foreach (var part in deserializedParts)
                {
                    if (IsValid(part))
                    {
                        var supplierId = new Random().Next(1, 32);
                        part.SupplierId = supplierId;
                        parts.Add(part);
                    }
                }

                context.Parts.AddRange(parts);
                context.SaveChanges();

                // ------------ Cars
                var cars = new List<Car>();

                foreach (var car in deserializedCars)
                {
                    if (IsValid(car))
                    {
                        cars.Add(car);
                    }
                }

                context.Cars.AddRange(cars);
                context.SaveChanges();


                // ------------ Customer
                var customers = new List<Customer>();

                foreach (var customer in deserializedCustomers)
                {
                    if (IsValid(customer))
                    {
                        customers.Add(customer);
                    }
                }

                context.Customers.AddRange(customers);
                context.SaveChanges();


                // ------------ Generate Parts
                var partCars = new List<PartCar>();

                foreach (var car in context.Cars)
                {
                    var partCar = new PartCar()
                    {
                        CarId = car.Id,
                        PartId = new Random().Next(1, 132)
                    };

                    partCars.Add(partCar);
                }

                context.PartsCars.AddRange(partCars);
                context.SaveChanges();


                // ------------ Generate Sales
                var discounts = new decimal[] { 0, 5, 10, 15, 20, 30, 40, 50 };
                var sales = new List<Sale>();

                for (int i = 0; i < 150; i++)
                {
                    var sale = new Sale()
                    {
                        CarId = new Random().Next(1, 359),
                        CustomerId = new Random().Next(1, 31),
                        Discount = discounts[new Random().Next(0, discounts.Length)]
                    };
                    sales.Add(sale);
                }

                context.Sales.AddRange(sales);
                context.SaveChanges();
            }

            return "Import successful!";
        }

        #endregion

        #region Private Helpers

        private static string ResetDB()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return "Database reset successful!";
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            return System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }

        #endregion
    }
}