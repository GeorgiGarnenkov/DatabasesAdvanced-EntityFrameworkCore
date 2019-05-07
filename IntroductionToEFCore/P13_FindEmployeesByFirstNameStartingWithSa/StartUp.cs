using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P13_FindEmployeesByFirstNameStartingWithSa
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => EF.Functions.Like(e.FirstName, "Sa%"))
                    .Select(e => new Employee()
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JobTitle = e.JobTitle,
                        Salary = e.Salary
                    })
                    .OrderBy(a => a.FirstName)
                    .ThenBy(a => a.LastName)
                    .ToArray();



                using (StreamWriter writer = new StreamWriter
                    (@"D:\SOFTUNI\RepoFolder\DatabasesAdvancedEntityFramework\IntroductionToEFCore\Writer.txt"))
                {

                    foreach (var e in employees)
                    {
                        writer.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
                    }
                }
            }
        }
    }
}
