using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile() {
            CreateMap<DataPipeline, DataPipelineDto>();
            CreateMap<UpdateDataPipelineDto, DataPipeline>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly ass)
        {
            var types = ass.GetExportedTypes().Where(x=> typeof(IMap).IsAssignableFrom(x) && !x.IsInterface).ToList();
            foreach(var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] {this});
            }
        }
    }
}
