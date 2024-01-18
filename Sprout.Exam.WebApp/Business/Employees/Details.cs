using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Business.Employees
{
    public class Details
    {
        public class Query : IRequest<EmployeeDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, EmployeeDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<EmployeeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var employee = await _context.Employees.SingleOrDefaultAsync(x => !x.IsDeleted && x.Id == request.Id);

                if (employee == null) return null;

                return _mapper.Map<EmployeeDto>(employee);
            }
        }
    }
}
