using AutoMapper;
using MediatR;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Business.Employees
{
    public class Delete
    {
        public class Command : IRequest<int?>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, int?>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<int?> Handle(Command request, CancellationToken cancellationToken)
            {
                var employee = await _context.Employees.FindAsync(request.Id);

                if (employee.IsDeleted || employee == null) return null;

                employee.IsDeleted = true;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return null;

                return request.Id;
            }
        }
    }
}
