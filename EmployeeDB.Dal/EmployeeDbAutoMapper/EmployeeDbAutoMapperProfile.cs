using AutoMapper;
using EmployeeDB.Dal.EmployeeDbResponseModels;
using EmployeeDB.Dal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDB.Dal.EmployeeDbAutoMapper
{
    public class EmployeeDbAutoMapperProfile : Profile
    {
        public EmployeeDbAutoMapperProfile()
        {
            CreateMap<Employees, EmployeeDbResponse>()
                .ReverseMap();
        }
    }
}
