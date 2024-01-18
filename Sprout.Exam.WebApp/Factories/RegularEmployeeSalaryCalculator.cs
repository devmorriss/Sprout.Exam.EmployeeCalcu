using Sprout.Exam.WebApp.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Factories
{
    public class RegularEmployeeSalaryCalculator : ISalaryCalculator
    {
        private const decimal regularSalary = 20000.00M;
        private const int days = 22;
        private const decimal tax = 0.12M;
        public decimal CalculateSalary(decimal daysAbsent)
        {
            decimal salaryPerday = regularSalary / 22;

            decimal finalPay = regularSalary - (salaryPerday * daysAbsent);

            decimal taxAmount = finalPay * tax;
            decimal netPay = Math.Round(finalPay - taxAmount, 2);

            return netPay;
        }
    }
}
