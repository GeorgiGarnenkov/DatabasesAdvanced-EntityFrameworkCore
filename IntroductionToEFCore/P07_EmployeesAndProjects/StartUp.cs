using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace P07_EmployeesAndProjects
{
    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => e.EmployeesProjects.Any(p =>
                        p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                    .Select(e => new
                    {
                        EmployeeName = e.FirstName + " " + e.LastName,
                        ManagerName = e.Manager.FirstName + " " + e.Manager.LastName,
                        Projects = e.EmployeesProjects.Select(p => new
                        {
                            ProjectName = p.Project.Name,
                            StartDate = p.Project.StartDate,
                            EndDate = p.Project.EndDate
                        }).ToArray()
                    })
                    .Take(30)
                    .ToArray();



                using (StreamWriter writer = new StreamWriter(@"D:\SOFTUNI\RepoFolder\DatabasesAdvancedEntityFramework\IntroductionToEFCore\Writer.txt"))
                {

                    foreach (var e in employees)
                    {
                        writer.WriteLine($"{e.EmployeeName} - Manager: {e.ManagerName}");

                        foreach (var project in e.Projects)
                        {
                            writer.WriteLine($"--{project.ProjectName} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {project.EndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) ?? "not finished"}");
                        }
                    }
                }
            }
        }
    }
}
