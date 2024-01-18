using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Models;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Business.Employees
{
    public class Edit
    {
        public class Command : IRequest<EmployeeDto>
        {
            public EditEmployeeDto Employee{ get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Employee).SetValidator(new EmployeeValidator());
            }
        }

        public class Handler : IRequestHandler<Command, EmployeeDto>
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

            public async Task<EmployeeDto> Handle(Command request, CancellationToken cancellationToken)
            {
                _validator.ValidateAndThrow(request);

                if (!Enum.IsDefined(typeof(Common.Enums.EmployeeType), request.Employee.TypeId)) return null;

                var employee = await _context.Employees.SingleOrDefaultAsync(x => !x.IsDeleted && x.Id == request.Employee.Id);

                if (employee == null) return null;

                _mapper.Map(request.Employee, employee);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return null;

                return _mapper.Map<EmployeeDto>(employee);
            }
        }
    }
}
