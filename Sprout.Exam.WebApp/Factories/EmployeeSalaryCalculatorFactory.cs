using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Factories
{
    public class EmployeeSalaryCalculatorFactory
    {
        public ISalaryCalculator Create(EmployeeType employeeType) => employeeType switch
        {
            EmployeeType.Regular => new RegularEmployeeSalaryCalculator(),
            EmployeeType.Contractual => new ContractualEmployeeSalaryCalculator(),
            _ => throw new ArgumentException("Employee Type Not Found")
        };
    }
}
