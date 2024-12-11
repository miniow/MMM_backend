using Infrastructure.Data;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
           
            services.Configure<MongoSettings>(
                configuration.GetSection("MongoSettings"));
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });
            services.AddScoped<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return sp.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName);
            });
        }
    }
}
