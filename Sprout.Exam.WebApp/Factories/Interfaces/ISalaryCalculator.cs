using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Factories.Interfaces
{
    public interface ISalaryCalculator
    {
        decimal CalculateSalary(decimal days);
    }
}
