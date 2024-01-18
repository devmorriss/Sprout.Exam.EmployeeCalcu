using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Business.Employees
{
    public class Calculate
    {
        public class Command : IRequest<decimal?>
        {
            public int Id { get; set; }
            public decimal Days { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Days).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(22);
            }
        }

        public class Handler : IRequestHandler<Command, decimal?>
        {
            private readonly ApplicationDbContext _context;
            private readonly EmployeeSalaryCalculatorFactory _calculatorFactory;
            private readonly IValidator<Command> _validator;

            public Handler(ApplicationDbContext context, EmployeeSalaryCalculatorFactory calculatorFactory, IValidator<Command> validator)
            {
                _context = context;
                _calculatorFactory = calculatorFactory;
                _validator = validator;
            }
            public async Task<decimal?> Handle(Command request, CancellationToken cancellationToken)
            {
                _validator.ValidateAndThrow(request);

                var employee = await _context.Employees.SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);

                if (employee == null) return null;

                try
                {
                    var calculator = _calculatorFactory.Create((EmployeeType)employee.EmployeeTypeId);
                    var salary = calculator.CalculateSalary(request.Days);

                    return salary;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
