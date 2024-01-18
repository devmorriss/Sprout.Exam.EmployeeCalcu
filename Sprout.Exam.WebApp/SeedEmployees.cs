using Sprout.Exam.DataAccess.Models;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp
{
    public class SeedEmployees
    {
        public static async Task SeedData(ApplicationDbContext context)
        {
            if(!context.EmployeeTypes.Any())
            {
                var typesToAdd = new List<EmployeeType>
                {
                    new EmployeeType
                    {
                        TypeName = "Regular"
                    },
                     new EmployeeType
                    {
                        TypeName = "Contractual"
                    },
                };

                await context.EmployeeTypes.AddRangeAsync(typesToAdd);
                await context.SaveChangesAsync();
            }
            
            if (!context.Employees.Any())
            {
                var employeesToAdd = new List<Employee>
                {
                    new Employee
                    {
                        BirthDate = DateTime.Parse("1993-03-25", null, System.Globalization.DateTimeStyles.RoundtripKind),
                        FullName = "Jane Doe",
                        Tin = "123215413",
                        EmployeeTypeId = (int)Common.Enums.EmployeeType.Regular,
                        IsDeleted = false
                    },
                    new Employee
                    {

                        BirthDate = DateTime.Parse("1993-05-28", null, System.Globalization.DateTimeStyles.RoundtripKind),
                        FullName = "John Doe",
                        Tin = "957125412",
                        EmployeeTypeId = (int)Common.Enums.EmployeeType.Contractual,
                        IsDeleted = false
                    }
                };

                await context.Employees.AddRangeAsync(employeesToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}
