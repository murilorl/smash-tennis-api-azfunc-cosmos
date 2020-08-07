using App.Data;
using App.Services;
using App.Settings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

[assembly: FunctionsStartup(typeof(App.Core.Startup))]
namespace App.Core
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<CosmosDbSettings>(config.GetSection("CosmosDb"));

            builder.Services.AddScoped<ICosmosService, CosmosService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRankingsService, RankingsService>();
        }
    }
}
