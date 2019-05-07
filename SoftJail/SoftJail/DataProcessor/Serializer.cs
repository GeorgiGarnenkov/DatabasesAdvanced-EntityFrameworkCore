using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SoftJail.DataProcessor.ExportDto;
using Formatting = Newtonsoft.Json.Formatting;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(x => ids.Any(p => p == x.Id))
                .Select(x => new
                                {
                                    Id = x.Id,
                                    Name = x.FullName,
                                    CellNumber = x.Cell.CellNumber,
                                    Officers = x.PrisonerOfficers.Select(o => new
                                                                                 {
                                                                                     OfficerName = o.Officer.FullName,
                                                                                     Department = o.Officer.Department.Name
                                                                                 }).OrderBy(o => o.OfficerName)
                                                                                   .ToArray(),
                                    TotalOfficerSalary = Math.Round(x.PrisonerOfficers.Sum(o => o.Officer.Salary), 2)
                                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            return JsonConvert.SerializeObject(prisoners, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var names = prisonersNames.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var prisoners = context.Prisoners
                .Where(x => names.Any(n => n == x.FullName))
                .Select(x => new PrisonerExportDto()
                {
                    Name = x.FullName,
                    Id = x.Id,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Messages = x.Mails.Select(m => new MessageExportDto()
                    {
                        Description = m.Description
                    }).ToArray()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            foreach (var p in prisoners)
            {
                foreach (var m in p.Messages)
                {
                    var charArray = m.Description.ToCharArray();
                    Array.Reverse(charArray);
                    m.Description = new string(charArray);
                }
            }

            var serializer = new XmlSerializer(typeof(PrisonerExportDto[]), new XmlRootAttribute("Prisoners"));
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var sb = new StringBuilder();
            serializer.Serialize(new StringWriter(sb), prisoners, xmlNamespaces);

            return sb.ToString();
        }
    }
}