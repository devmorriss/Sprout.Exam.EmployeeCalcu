using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Models;

namespace Sprout.Exam.Business.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EditEmployeeDto, Employee>()
                .ForMember(d => d.EmployeeTypeId, o => o.MapFrom(s => s.TypeId));
            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(d => d.EmployeeTypeId, o => o.MapFrom(s => s.TypeId));
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.TypeId, o => o.MapFrom(s => s.EmployeeTypeId))
                .ForMember(d => d.Birthdate, o => o.MapFrom(s => s.BirthDate.ToString("yyyy-MM-dd")));
        }
    }
}
