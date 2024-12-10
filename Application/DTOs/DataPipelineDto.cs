using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DataPipelineDto : IMap
    {
        public Guid Id { get; set; }
        public PipelineStatus Status { get; set; }
        public string Name { get;  set; }
        public DateTime? LastExecutedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DataPipeline, DataPipelineDto>();
        }
    }
    public class CreateDataPipelineDto
    {
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateDataPipelineDto, DataPipeline>();
        }
    }
    public class UpdateDataPipelineDto
    {
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateDataPipelineDto, DataPipeline>();
        }
    }
}
