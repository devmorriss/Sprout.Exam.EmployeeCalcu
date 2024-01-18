using AutoMapper;
using FluentValidation;
using MediatR;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Models;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Business.Employees
{
    public class Create
    {
        public class Command : IRequest<int?>
        {
            public CreateEmployeeDto Employee{ get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Employee).SetValidator(new EmployeeValidator());
            }
        }

        public class Handler : IRequestHandler<Command, int?>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<Command> _validator;

            public Handler(ApplicationDbContext context, IMapper mapper, IValidator<Command> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }
            public async Task<int?> Handle(Command request, CancellationToken cancellationToken)
            {
                _validator.ValidateAndThrow(request);

                if (!Enum.IsDefined(typeof(Common.Enums.EmployeeType), request.Employee.TypeId)) return null;

                var employee = _mapper.Map<Employee>(request.Employee);

                _context.Employees.Add(employee);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return null;

                return employee.Id;
            }
        }
    }
}
