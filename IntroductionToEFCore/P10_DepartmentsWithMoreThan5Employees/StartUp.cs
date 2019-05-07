using System.IO;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace P10_DepartmentsWithMoreThan5Employees
{
    public class StartUp
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var departments = context.Departments
                    .Where(d => d.Employees.Count > 5)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d => d.Name)
                    .Select(d => new
                    {
                        DepartmentName = d.Name,
                        ManagerName = d.Manager.FirstName + " " + d.Manager.LastName,
                        Employees = d.Employees.Select(e => new
                          {
                              EmployeeFirstName = e.FirstName,
                              EmployeeLastName = e.LastName,
                              EmployeeJobTitle = e.JobTitle
                          }).OrderBy(e => e.EmployeeFirstName)
                            .ThenBy(e => e.EmployeeLastName)
                            .ToArray()
                    })
                    .ToArray();


                using (StreamWriter writer = new StreamWriter
                    (@"D:\SOFTUNI\RepoFolder\DatabasesAdvancedEntityFramework\IntroductionToEFCore\Writer.txt"))
                {

                    foreach (var d in departments)
                    {
                        writer.WriteLine($"{d.DepartmentName} - {d.ManagerName}");

                        foreach (var e in d.Employees)
                        {
                            writer.WriteLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - {e.EmployeeJobTitle}");
                        }

                        writer.WriteLine("----------");
                    }
                }
            }
        }
    }
}
