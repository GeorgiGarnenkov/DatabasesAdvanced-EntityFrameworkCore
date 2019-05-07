using System.IO;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace P09_Employee147
{
    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employee = context.Employees
                    .Where(e => e.EmployeeId == 147)
                    .Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JobTitle = e.JobTitle,
                        Projects = e.EmployeesProjects
                        .Select(p => new
                            {
                                ProjectName = p.Project.Name
                            }).ToArray()
                    })
                    .ToArray();


                using (StreamWriter writer = new StreamWriter
                    (@"D:\SOFTUNI\RepoFolder\DatabasesAdvancedEntityFramework\IntroductionToEFCore\Writer.txt"))
                {

                    foreach (var e in employee)
                    {
                        writer.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");

                        foreach (var project in e.Projects.OrderBy(a => a.ProjectName))
                        {
                            writer.WriteLine($"{project.ProjectName}");
                        }
                    }
                }
            }
        }
    }
}
