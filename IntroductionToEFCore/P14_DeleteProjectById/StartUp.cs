using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P14_DeleteProjectById
{
    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var projects = context.EmployeesProjects.Where(a => a.ProjectId == 2);

                context.EmployeesProjects.RemoveRange(projects);

                var project = context.Projects.Find(2);

                context.Projects.Remove(project);

                context.SaveChanges();

                var projectsTake = context.Projects.Take(10).ToArray();
                
                using (StreamWriter writer = new StreamWriter
                    (@"D:\SOFTUNI\RepoFolder\DatabasesAdvancedEntityFramework\IntroductionToEFCore\Writer.txt"))
                {
                    foreach (var p in projectsTake)
                    {
                        writer.WriteLine(p.Name);
                    }
                }
            }
        }
    }
}
