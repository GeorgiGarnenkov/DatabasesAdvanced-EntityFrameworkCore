namespace CarDealer.App
{
    using Microsoft.EntityFrameworkCore;

    using Models;
    using Data;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(ResetDB());
            Console.WriteLine(ImportSuppliers());
            Console.WriteLine(ImportParts());
            Console.WriteLine(ImportCars());
            Console.WriteLine(ImportCustomers());
            Console.WriteLine(ImportSales());
            Console.WriteLine(ExportCarsWithDistance());
            Console.WriteLine(ExportFerrariCars());
            Console.WriteLine(ExportLocalSuppliers());
            Console.WriteLine(ExportCarsAndTheirParts());
            Console.WriteLine(ExportCustomersSales());
            Console.WriteLine(ExportSalesWithDiscount());
        }

        #region XML Export

        private static string ExportSalesWithDiscount()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var sales = context.Sales
                                   .Select(s => new
                                   {
                                       s.Car,
                                       CustomerName = s.Customer.Name,
                                       s.Discount,
                                       Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                                       PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) -
                                                            s.Car.PartCars.Sum(pc => pc.Part.Price) * decimal.Parse(s.Discount.ToString())
                                   })
                                   .ToArray();

                XDocument xDocument = new XDocument(new XElement("sales"));

                foreach (var s in sales)
                {
                    var saleInfo = new XElement("sale",
                                    new XElement("car",
                                        new XAttribute("make", s.Car.Make),
                                        new XAttribute("model", s.Car.Model),
                                        new XAttribute("travelled-distance", s.Car.TravelledDistance)),
                                    new XElement("customer-name", s.CustomerName),
                                    new XElement("discount", s.Discount),
                                    new XElement("price", s.Price),
                                    new XElement("price-with-discount", s.PriceWithDiscount));

                    xDocument.Root.Add(saleInfo);
                }

                string directory = "..\\..\\..\\Export\\sales-discount.xml";

                xDocument.Save(directory);

                return $"{sales.Length} sales exported to => {directory}";
            }
        }

        private static string ExportCustomersSales()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var customers = context.Customers
                                       .Include(c => c.Buys)
                                       .Where(c => c.Buys.Count > 0)
                                       .Select(c => new
                                       {
                                           c.Name,
                                           BoughtCars = c.Buys.Count,
                                           SpentMoney = c.Buys.Sum(b => b.Car.PartCars.Sum(pc => pc.Part.Price))
                                       })
                                       .OrderByDescending(c => c.SpentMoney)
                                       .ThenBy(c => c.BoughtCars)
                                       .ToArray();

                XDocument xDocument = new XDocument(new XElement("customers"));

                foreach (var c in customers)
                {
                    var customerInfo = new XElement("customer",
                                          new XAttribute("full-name", c.Name),
                                          new XAttribute("bought-cars", c.BoughtCars),
                                          new XAttribute("spent-money", c.SpentMoney));

                    xDocument.Root.Add(customerInfo);
                }

                string directory = "..\\..\\..\\Export\\customers-total-sales.xml";

                xDocument.Save(directory);

                return $"{customers.Length} customers exported to => {directory}";
            }
        }

        private static string ExportCarsAndTheirParts()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var cars = context.Cars
                                  .Include(c => c.PartCars)
                                  .ThenInclude(pc => pc.Part)
                                  .Select(c => new
                                  {
                                      c.Make,
                                      c.Model,
                                      c.TravelledDistance,
                                      Parts = c.PartCars.Select(pc => new
                                      {
                                          pc.Part.Name,
                                          pc.Part.Price,
                                      })
                                  })
                                  .ToArray();

                XDocument xDocument = new XDocument(new XElement("cars"));

                foreach (var c in cars)
                {
                    var carInfo = new XElement("car",
                                    new XAttribute("make", c.Make),
                                    new XAttribute("model", c.Model),
                                    new XAttribute("travelled-distance", c.TravelledDistance),
                                    new XElement("parts"));

                    foreach (var p in c.Parts)
                    {
                        carInfo.Element("parts").Add(new XElement("part",
                                                        new XAttribute("name", p.Name),
                                                        new XAttribute("price", p.Price)));
                    }

                    xDocument.Root.Add(carInfo);
                }

                string directory = "..\\..\\..\\Export\\cars-and-parts.xml";

                xDocument.Save(directory);

                return $"{cars.Length} cars exported to => {directory}";
            }
        }

        private static string ExportLocalSuppliers()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var suppliers = context.Suppliers
                                              .Include(s => s.Parts)
                                              .Where(s => s.IsImporter == true)
                                              .Select(s => new
                                              {
                                                  s.Id,
                                                  s.Name,
                                                  PartsCount = s.Parts.Count,
                                              })
                                              .ToArray();

                XDocument xDocument = new XDocument(new XElement("suppliers"));

                foreach (var s in suppliers)
                {
                    var supplierInfo = new XElement("supplier",
                                        new XAttribute("id", s.Id),
                                        new XAttribute("name", s.Name),
                                        new XAttribute("parts-count", s.PartsCount));

                    xDocument.Root.Add(supplierInfo);
                }

                string directory = "..\\..\\..\\Export\\local-suppliers.xml";

                xDocument.Save(directory);

                return $"{suppliers.Length} suppliers exported to => {directory}";
            }
        }

        private static string ExportFerrariCars()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                string make = "Ferrari";

                Car[] cars = context.Cars
                                    .Where(c => c.Make == make)
                                    .OrderBy(c => c.Model)
                                    .ThenByDescending(c => c.TravelledDistance)
                                    .ToArray();

                XDocument xDocument = new XDocument(new XElement("cars"));

                foreach (var car in cars)
                {
                    var carInfo = new XElement("car",
                                        new XAttribute("id", car.Id),
                                        new XAttribute("model", car.Model),
                                        new XAttribute("travelled-distance", car.TravelledDistance));

                    xDocument.Root.Add(carInfo);
                }

                string directory = "..\\..\\..\\Export\\ferrari-cars.xml";

                xDocument.Save(directory);

                return $"{cars.Length} Ferrari cars exported to => {directory}";
            }
        }

        private static string ExportCarsWithDistance(int distance = 2_000_000)
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                Car[] cars = context.Cars
                                    .Where(c => c.TravelledDistance > distance)
                                    .OrderBy(c => c.Model)
                                    .ToArray();

                XDocument xDocument = new XDocument(new XElement("cars"));

                foreach (var car in cars)
                {
                    var carInfo = new XElement("car",
                                    new XElement("make", car.Make),
                                    new XElement("model", car.Model),
                                    new XElement("travelled-distance", car.TravelledDistance));

                    xDocument.Root.Add(carInfo);
                }

                string directory = "..\\..\\..\\Export\\cars-with-distance.xml";

                xDocument.Save(directory);

                return $"{cars.Length} cars exported to => {directory}";
            }
        }

        #endregion

        #region XML Import

        private static string ImportCustomers()
        {
            XDocument xDocument = XDocument.Load("..\\..\\..\\Import\\customers.xml");

            var customersXml = xDocument.Root.Elements();

            List<Customer> customers = new List<Customer>();

            foreach (var c in customersXml)
            {
                string name = c.Attribute("name").Value;

                string xmlBirthdate = c.Element("birth-date").Value;
                string[] birthdateArgs = xmlBirthdate.Split('T');
                string birthdateString = birthdateArgs[0] + " " + birthdateArgs[1];

                bool isYoungDriver = bool.Parse(c.Element("is-young-driver").Value);

                DateTime birthdate = DateTime.ParseExact(birthdateString, "yyyy-MM-dd hh:mm:ss", null);

                Customer customer = new Customer
                {
                    Name = name,
                    IsYoungDriver = isYoungDriver,
                    BirthDate = birthdate,
                };

                customers.Add(customer);

                Console.WriteLine(customer.Name + " added!");
            }

            using (CarDealerContext context = new CarDealerContext())
            {
                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            return $"{customers.Count} added!";
        }

        private static string ImportCars()
        {
            XDocument xDocument = XDocument.Load("..\\..\\..\\Import\\cars.xml");

            var carsXml = xDocument.Root.Elements();

            List<Car> cars = new List<Car>();

            using (CarDealerContext context = new CarDealerContext())
            {
                foreach (var c in carsXml)
                {
                    string make = c.Element("make").Value;
                    string model = c.Element("model").Value;
                    double travelledDistance = double.Parse(c.Element("travelled-distance").Value);

                    Car car = new Car
                    {
                        Make = make,
                        Model = model,
                        TravelledDistance = travelledDistance,
                    };

                    cars.Add(car);
                }

                List<Part> allParts = context.Parts.ToList();

                foreach (var car in cars)
                {
                    CreatePartCars(car, allParts, context);
                    Console.WriteLine("CarId: " + car.Id + " Model: " + car.Model + $" added! Parts added: {car.PartCars.Count}");
                }
            }

            return $"\n{cars.Count} cars added!\n";
        }

        private static string ImportParts()
        {
            XDocument xDocument = XDocument.Load("..\\..\\..\\Import\\parts.xml");

            var partsXml = xDocument.Root.Elements();

            List<Part> parts = new List<Part>();

            using (CarDealerContext context = new CarDealerContext())
            {
                var allSuppliers = context.Suppliers.ToList();

                Random random = new Random();

                foreach (var p in partsXml)
                {
                    string name = p.Attribute("name").Value;
                    decimal price = decimal.Parse(p.Attribute("price").Value);
                    int quantity = int.Parse(p.Attribute("quantity").Value);

                    int supplierIndex = random.Next(0, allSuppliers.Count - 1);
                    Supplier supplier = allSuppliers[supplierIndex];

                    Part part = new Part
                    {
                        Name = name,
                        Price = price,
                        Quantity = quantity,
                        Supplier = supplier,
                    };

                    supplier.Parts.Add(part);

                    parts.Add(part);

                    Console.WriteLine($"Part {part.Name} added!");
                }

                context.Parts.AddRange(parts);
                context.SaveChanges();
            }

            return $"\n{parts.Count} parts added!\n";
        }

        private static string ImportSuppliers()
        {
            XDocument xDocument = XDocument.Load("..\\..\\..\\Import\\suppliers.xml");

            var suppliersXml = xDocument.Root.Elements();

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var s in suppliersXml)
            {
                string name = s.Attribute("name").Value;
                bool isImporter = bool.Parse(s.Attribute("is-importer").Value);

                Supplier supplier = new Supplier
                {
                    Name = name,
                    IsImporter = isImporter,
                };

                suppliers.Add(supplier);

                Console.WriteLine($"Supplier: {supplier.Name} added!");
            }

            using (CarDealerContext context = new CarDealerContext())
            {
                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

            return $"\n{suppliers.Count} suppliers added!\n";
        }

        #endregion

        #region Manual Import

        private static string ImportSales()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                double[] discountRange = new double[] { 0.00, 0.05, 0.10, 0.15, 0.20, 0.30, 0.40, 0.50 };

                Customer[] customers = context.Customers.ToArray();
                Car[] cars = context.Cars.ToArray();

                Random random = new Random();

                List<Sale> sales = new List<Sale>();

                foreach (var car in cars)
                {
                    int customerIndex = random.Next(0, customers.Length - 1);
                    int discountIndex = random.Next(0, discountRange.Length - 1);

                    Customer customer = customers[customerIndex];
                    double discount = discountRange[discountIndex];

                    Sale sale = new Sale
                    {
                        Customer = customer,
                        Car = car,
                        Discount = discount,
                    };

                    sales.Add(sale);

                    Console.WriteLine($"{customer.Name} successfully bought {car.Make} {car.Model}! Discount: {discount}%!");
                }

                context.Sales.AddRange(sales);
                context.SaveChanges();

                return $"{sales.Count} sales added!";
            }
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

        private static void CreatePartCars(Car car, List<Part> allParts, CarDealerContext context)
        {
            Random random = new Random();

            int numberOfPartsToAdd = random.Next(10, 20);

            for (int i = 0; i < numberOfPartsToAdd; i++)
            {
                int partIndex = random.Next(0, allParts.Count - 1);

                Part part = allParts[partIndex];

                bool partExists = car.PartCars.Any(pc => pc.Part == part);
                while (partExists)
                {
                    partIndex = random.Next(0, allParts.Count - 1);
                    part = allParts[partIndex];

                    partExists = car.PartCars.Any(pc => pc.Part == part);
                }

                PartCar partCar = new PartCar
                {
                    Car = car,
                    Part = part,
                };

                context.PartsCars.Add(partCar);
            }
            context.SaveChanges();
        }

        #endregion
    }
}