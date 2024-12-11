using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            services.AddScoped<IDataPipelineRepository, DataPipelineRepository>();
            services.AddScoped<IDataFlowRepository, DataFlowRepository>();
            return services;
        }
    }
}
