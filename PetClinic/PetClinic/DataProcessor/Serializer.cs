using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace PetClinic.DataProcessor
{
    using Data;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animals = context.Animals
                .Where(x => x.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(x => new
                {
                    OwnerName = x.Passport.OwnerName,
                    AnimalName = x.Name,
                    Age = x.Age,
                    SerialNumber = x.PassportSerialNumber,
                    RegisteredOn = x.Passport.RegistrationDate
                })
                .OrderBy(x => x.Age)
                .ThenBy(x => x.SerialNumber)
                .ToArray();

            string result = JsonConvert.SerializeObject(animals, Formatting.Indented, 
                new JsonSerializerSettings()
                {
                    DateFormatString = "dd-MM-yyyy"
                });
            return result;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var procedures = context.Procedures
                .Select(p => new
                {
                    Passport = p.Animal.Passport.SerialNumber,
                    OwnerNumber = p.Animal.Passport.OwnerPhoneNumber,
                    DateTime = p.DateTime,
                    AnimalAids = p.ProcedureAnimalAids.Select(map => new
                    {
                        Name = map.AnimalAid.Name,
                        Price = map.AnimalAid.Price
                    }),
                    TotalPrice = p.ProcedureAnimalAids.Select(paa => paa.AnimalAid.Price).Sum(),
                })
                .OrderBy(p => p.DateTime)
                .ThenBy(p => p.Passport)
                .ToArray();

            var xDoc = new XDocument(new XElement("Procedures"));

            foreach (var p in procedures)
            {
                xDoc.Root.Add
                (
                    new XElement("Procedure",
                        new XElement("Passport", p.Passport),
                        new XElement("OwnerNumber", p.OwnerNumber),
                        new XElement("DateTime", p.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                        new XElement("AnimalAids", p.AnimalAids.Select(aa =>
                                                                        new XElement("AnimalAid",
                                                                            new XElement("Name", aa.Name),
                                                                            new XElement("Price", aa.Price)))),
                        new XElement("TotalPrice", p.TotalPrice))
                );
            }

            string result = xDoc.ToString();
            return result;
        }
    }
}
