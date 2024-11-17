
using Application;

using Microsoft.AspNetCore.Identity;

namespace WebApi.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddControllers();
        }
    }
}
