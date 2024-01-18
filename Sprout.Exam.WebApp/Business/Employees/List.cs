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
    public class List
    {
        public class Query : IRequest<List<EmployeeDto>> {}
        public class Handler : IRequestHandler<Query, List<EmployeeDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<EmployeeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var employees = await _context.Employees.Where(x => !x.IsDeleted).ToListAsync();

                return _mapper.Map<List<EmployeeDto>>(employees);
            }
        }
    }
}
