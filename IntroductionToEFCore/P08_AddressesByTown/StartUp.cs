using System.Globalization;
using System.IO;
using System.Linq;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P08_AddressesByTown
{
    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var addresses = context.Addresses
                    .OrderByDescending(e => e.Employees.Count)
                    .ThenBy(t => t.Town.Name)
                    .ThenBy(a => a.AddressText)
                    .Take(10)
                    .Select(a => new
                    {
                        AddressText = a.AddressText,
                        TownName = a.Town.Name,
                        EmployeeCount = a.Employees.Count
                    })
                    .ToArray();



                using (StreamWriter writer = new StreamWriter(@"D:\SOFTUNI\RepoFolder\DatabasesAdvancedEntityFramework\IntroductionToEFCore\Writer.txt"))
                {
                    foreach (var a in addresses)
                    {
                        writer.WriteLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
                    }
                    
                }
            }
        }
    }
}
