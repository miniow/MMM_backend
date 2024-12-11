using Application.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOs
{
    public class WorkspaceDto : IMap
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Workspace, WorkspaceDto>();
        }
    }

    public class CreateWorkspaceDto : IMap
    { 
        public string Name { get; set; } = string.Empty;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateWorkspaceDto, Workspace>();
        }
    }
    public class UpdateWorkspaceDto : IMap
    {
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateWorkspaceDto, Workspace>();
        }
    }

}
