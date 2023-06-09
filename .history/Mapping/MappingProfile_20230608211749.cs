using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using angular_vega.Controllers.Resources;
using angular_vega.Models;
using AutoMapper;

namespace angular_vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Make,MakeResource>();
        }
    }
}