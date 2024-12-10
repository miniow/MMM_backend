using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ApplicationDbConnection"),
                    sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "app")));

            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("UserDbConnection"),
                    sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "identity")));
        }
    }
}
