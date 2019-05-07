using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using SoftJail.DataProcessor.ImportDto;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Deserializer
    {
        private const string FailureMessage = "Invalid Data";
        private const string SuccessMessageImportDepCells = "Imported {0} with {1} cells";
        private const string SuccessMessageImportPrisMails = "Imported {0} {1} years old";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserializedDep = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);

            List<Department> departments = new List<Department>();
            
            foreach (DepartmentDto departmentDto in deserializedDep)
            {
                if (!IsValid(departmentDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool breaking = false;
                List<Cell> cells = new List<Cell>();
                foreach (CellDto departmentDtoCell in departmentDto.Cells)
                {

                    if (!IsValid(departmentDtoCell))
                    {
                        breaking = true;
                        break;
                    }

                    var cell = new Cell
                    {
                        CellNumber = departmentDtoCell.CellNumber,
                        HasWindow = departmentDtoCell.HasWindow
                    };

                    cells.Add(cell);
                }
                if (breaking)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var dep = new Department
                {
                    Name = departmentDto.Name,
                    Cells = cells
                };

                departments.Add(dep);

                sb.AppendLine(string.Format(SuccessMessageImportDepCells, dep.Name, dep.Cells.Count));
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var deserializedPrisoners = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);

            List<Prisoner> prisoners = new List<Prisoner>();
            
            foreach (PrisonerDto prisonerDto in deserializedPrisoners)
            {
               

                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var breaking = false;
                List<Mail> mails = new List<Mail>();
                foreach (MailDto mailDto in prisonerDto.Mails)
                {
                    if (!IsValid(mailDto))
                    {
                        breaking = true;
                        break;
                    }

                    var mail = new Mail
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    };

                    mails.Add(mail);
                }
                if (breaking)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                
                var prisoner = new Prisoner();

                prisoner.FullName = prisonerDto.FullName;
                prisoner.Nickname = prisonerDto.Nickname;
                prisoner.Age = prisonerDto.Age;
                prisoner.IncarcerationDate = DateTime
                    .ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (prisonerDto.ReleaseDate != null)
                {
                    prisoner.ReleaseDate = DateTime
                        .ParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                prisoner.CellId = prisonerDto.CellId;
                prisoner.Mails = mails;


                prisoners.Add(prisoner);

                sb.AppendLine(string.Format(SuccessMessageImportPrisMails, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OfficerDto[]), new XmlRootAttribute("Officers"));
            OfficerDto[] deserializedData = (OfficerDto[]) serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();

            List<Officer> officers = new List<Officer>();

            foreach (OfficerDto dto in deserializedData)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Officer officer = new Officer();
                officer.FullName = dto.Name;
                officer.Salary = dto.Money;
                officer.DepartmentId = dto.DepartmentId;
                bool validEnums = Enum.TryParse<Weapon>(dto.Weapon, out Weapon weapon) &&
                                  Enum.TryParse<Position>(dto.Position, out Position position);

                if (!validEnums)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                officer.Position = Enum.Parse<Position>(dto.Position);
                officer.Weapon = Enum.Parse<Weapon>(dto.Weapon);
                ;

                List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();
                foreach (var prisonerDto in dto.Prisoners)
                {
                    var prisonerId = int.Parse(prisonerDto.Id);

                    var officerPrisoner = new OfficerPrisoner()
                    {
                        PrisonerId = prisonerId,
                        Officer = officer
                    };

                    officerPrisoners.Add(officerPrisoner);
                }

                officer.OfficerPrisoners = officerPrisoners;

                officers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
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