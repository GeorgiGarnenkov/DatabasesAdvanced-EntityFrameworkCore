using System;
using System.Globalization;
using System.IO;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace P11_FindLatest10Projects
{
    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var projects = context.Projects
                    .OrderByDescending(a => a.StartDate)
                    .Take(10)
                    .OrderBy(a => a.Name)
                    .Select(a => new
                    {
                        ProjectName = a.Name,
                        ProjectDescription = a.Description,
                        ProjectDate = a.StartDate
                    })
                    .ToArray();


                using (StreamWriter writer = new StreamWriter
                    (@"D:\SOFTUNI\RepoFolder\DatabasesAdvancedEntityFramework\IntroductionToEFCore\Writer.txt"))
                {
                    foreach (var project in projects)
                    {
                        writer.WriteLine($"{project.ProjectName}");
                        writer.WriteLine($"{project.ProjectDescription}");
                        writer.WriteLine($"{project.ProjectDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
                    }
                    
                }
            }
        }
    }
}
