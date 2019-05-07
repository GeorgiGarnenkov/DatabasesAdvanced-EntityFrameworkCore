using System;
using System.Globalization;
using System.IO;
using System.Linq;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P12_IncreaseSalaries
{
    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                string[] departments = new[] 
                    {"Engineering", "Tool Design", "Marketing", "Information Services"};

                var employees = context.Employees
                    .Where(d => departments.Any(s => s == d.Department.Name))
                    .OrderBy(a => a.FirstName)
                    .ThenBy(a => a.LastName)
                    .ToArray();

               

                using (StreamWriter writer = new StreamWriter
                    (@"D:\SOFTUNI\RepoFolder\DatabasesAdvancedEntityFramework\IntroductionToEFCore\Writer.txt"))
                {

                    foreach (var e in employees)
                    {
                        e.Salary *= 1.12m;

                        writer.WriteLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
