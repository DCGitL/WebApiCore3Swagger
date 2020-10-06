using Adventure.Works._2012.dbContext.Models;
using Adventure.Works._2012.dbContext.ResponseModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure.Works._2012.dbContext.AutoMapper
{
    public class AutoMapperDataProfile : Profile
    {
        public AutoMapperDataProfile()
        {

            //source, destination
            CreateMap<Orders, ResponseOrder>();
            CreateMap<Employees, ResponseEmployee>();
        }
    }
}
