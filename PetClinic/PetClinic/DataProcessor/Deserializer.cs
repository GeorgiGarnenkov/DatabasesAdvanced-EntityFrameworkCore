using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using PetClinic.DataProcessor.Dto.Import;
using PetClinic.Models;

namespace PetClinic.DataProcessor
{
    using System;

    using PetClinic.Data;

    public class Deserializer
    {
        private const string FailureMessage = "Error: Invalid data.";
        private const string SuccessMessage = "Record successfully imported.";
        private const string SuccessMessageImportAnimalAids = "Record {0} successfully imported.";
        private const string SuccessMessageImportAnimals = "Record {0} Passport №: {1} successfully imported.";
        private const string SuccessMessageImportVets = "Record {0} successfully imported.";



        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserializedAnimalAid = JsonConvert.DeserializeObject<AnimalAidsDto[]>(jsonString);

            List<AnimalAid> animalAids = new List<AnimalAid>();

            foreach (AnimalAidsDto animalAidsDto in deserializedAnimalAid)
            {
                var animalAidExists = animalAids.Any(x => x.Name == animalAidsDto.Name);

                if (!IsValid(animalAidsDto) || animalAidExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var animalAid = new AnimalAid
                {
                    Name = animalAidsDto.Name,
                    Price = animalAidsDto.Price
                };

                animalAids.Add(animalAid);

                sb.AppendLine(string.Format(SuccessMessageImportAnimalAids, animalAid.Name));
            }

            context.AnimalAids.AddRange(animalAids);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserializedAnimal = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString);

            List<Animal> animals = new List<Animal>();

            foreach (AnimalDto animalDto in deserializedAnimal)
            {
                var passportExists = animals.Any(x => x.Passport.SerialNumber == animalDto.Passport.SerialNumber);

                if (!IsValid(animalDto) || !IsValid(animalDto.Passport) || passportExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                
                var animal = new Animal
                {
                    Name = animalDto.Name,
                    Type = animalDto.Type,
                    Age = animalDto.Age,
                    Passport = new Passport
                    {
                        SerialNumber = animalDto.Passport.SerialNumber,
                        OwnerName = animalDto.Passport.OwnerName,
                        OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                        RegistrationDate = animalDto.Passport.RegistrationDate
                    }
                };

                animals.Add(animal);

                sb.AppendLine(string.Format(SuccessMessageImportAnimals, animal.Name, animal.Passport.SerialNumber));
            }

            context.Animals.AddRange(animals);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }



        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);
            var elements = xDoc.Root.Elements();

            var sb = new StringBuilder();
            var validEntries = new List<Vet>();

            foreach (var el in elements)
            {
                string name = el.Element("Name")?.Value;
                string profession = el.Element("Profession")?.Value;
                string ageString = el.Element("Age")?.Value;
                string phoneNumber = el.Element("PhoneNumber")?.Value;

                int age = 0;

                if (ageString != null)
                {
                    age = int.Parse(ageString);
                }

                Vet vet = new Vet()
                {
                    Name = name,
                    Profession = profession,
                    Age = age,
                    PhoneNumber = phoneNumber,
                };

                bool isValid = IsValid(vet);
                bool phoneNumberExists = validEntries.Any(v => v.PhoneNumber == vet.PhoneNumber);

                if (!isValid || phoneNumberExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                validEntries.Add(vet);

                sb.AppendLine(String.Format(SuccessMessageImportVets, vet.Name));
            }

            context.Vets.AddRange(validEntries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);
            var elements = xDoc.Root.Elements();

            var sb = new StringBuilder();
            var validEntries = new List<Procedure>();

            foreach (var el in elements)
            {
                string vetName = el.Element("Vet")?.Value;
                string passportId = el.Element("Animal")?.Value;
                string dateTimeString = el.Element("DateTime")?.Value;

                int? vetId = context.Vets.SingleOrDefault(v => v.Name == vetName)?.Id;
                bool passportExists = context.Passports.Any(p => p.SerialNumber == passportId);

                bool dateIsValid = DateTime
                    .TryParseExact(dateTimeString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);

                var animalAidElements = el.Element("AnimalAids")?.Elements();

                if (vetId == null || !passportExists || animalAidElements == null || !dateIsValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var animalAidIds = new List<int>();
                bool allAidsExist = true;

                foreach (var aid in animalAidElements)
                {
                    string aidName = aid.Element("Name")?.Value;

                    int? aidId = context.AnimalAids.SingleOrDefault(a => a.Name == aidName)?.Id;

                    if (aidId == null || animalAidIds.Any(id => id == aidId))
                    {
                        allAidsExist = false;
                        break;
                    }

                    animalAidIds.Add(aidId.Value);
                }

                if (!allAidsExist)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var procedure = new Procedure()
                {
                    VetId = vetId.Value,
                    AnimalId = context.Animals.Single(a => a.PassportSerialNumber == passportId).Id,
                    DateTime = dateTime,
                };

                foreach (var id in animalAidIds)
                {
                    var mapping = new ProcedureAnimalAid()
                    {
                        Procedure = procedure,
                        AnimalAidId = id
                    };

                    procedure.ProcedureAnimalAids.Add(mapping);
                }

                bool isValid = IsValid(procedure);

                if (!isValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                validEntries.Add(procedure);
                sb.AppendLine(SuccessMessage);
            }

            context.Procedures.AddRange(validEntries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
    }
}
