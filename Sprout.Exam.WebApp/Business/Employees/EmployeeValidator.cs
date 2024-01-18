using FluentValidation;
using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Business.Employees
{
    public class EmployeeValidator : AbstractValidator<BaseSaveEmployeeDto>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.Birthdate).NotEmpty().WithMessage("Birthdate is required")
                .Must(birthdate => CalculateAge(birthdate) >= 18).WithMessage("You must be at least 18 years old."); ;
            RuleFor(x => x.FullName).NotEmpty()
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Full Name must not contain numbers."); ;
            RuleFor(x => x.Tin).NotEmpty();
            RuleFor(x => x.TypeId).NotEmpty();
        }

        private int CalculateAge(DateTime birthdate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthdate.Year;

            if (birthdate > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
