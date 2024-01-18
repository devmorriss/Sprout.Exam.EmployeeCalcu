using Sprout.Exam.WebApp.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Factories
{
    public class ContractualEmployeeSalaryCalculator : ISalaryCalculator
    {
        private const decimal ratePerDay = 500.00M;
        public decimal CalculateSalary(decimal days)
        {
            decimal finalPay = ratePerDay * days;
            return Math.Round(finalPay, 2);
        }
    }
}
