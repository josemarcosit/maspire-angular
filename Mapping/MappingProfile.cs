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
            //Domain to API Resource
            CreateMap<Make,MakeResource>();
            CreateMap<Make,KeyValuePairResource>();
            CreateMap<Model,KeyValuePairResource>();
            CreateMap<Vehicle,SaveVehicleResource>()               
            .ForMember(v=> v.Contact, opt => opt.MapFrom(v=> new ContactResource{Name=v.ContactName,Phone = v.ContactPhone,Email=v.ContactEmail}))
            .ForMember(v=> v.Features, opt => opt.MapFrom(v=> v.Features.Select(vf => vf.FeatureId)));
            
            CreateMap<Vehicle,VehicleResource>()
                .ForMember(v=> v.Make, opt => opt.MapFrom(v=> v.Model.Make))
                .ForMember(v=> v.Contact, opt => opt.MapFrom(v=> new ContactResource{Name=v.ContactName,Phone = v.ContactPhone,Email=v.ContactEmail}))
                 .ForMember(v=> v.Features, opt => opt.MapFrom(v=> v.Features.Select(vf => new KeyValuePairResource{Id = vf.Feature.Id, Name = vf.Feature.Name})));

            //API Resource to Domain
             CreateMap<SaveVehicleResource,Vehicle>()
             .ForMember(v => v.Id, opt=> opt.Ignore())
             .ForMember(v=> v.ContactName, opt => opt.MapFrom(vr=> vr.Contact.Name))
              .ForMember(v=> v.ContactPhone, opt => opt.MapFrom(vr=> vr.Contact.Phone))
               .ForMember(v=> v.ContactEmail, opt => opt.MapFrom(vr=> vr.Contact.Email))
               .ForMember(v=> v.Features, opt => opt.Ignore())
               .AfterMap((vr,v)=>{               

                var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId));
                foreach(var f in removedFeatures)
                    v.Features.Remove(f);     
                                    
                var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id =>new VehicleFeature { FeatureId = id});
                foreach(var f in addedFeatures)
                    v.Features.Add(f);
               });
        }
    }
}