using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Thesaurus.Engine.DAL.DataContext;
using Thesaurus.Engine.BAL;
using Thesaurus.Engine.BAL.Repositories.Interfaces;
using System.Configuration;
using Thesaurus.Engine.BAL.Repositories.Services;

namespace Thesaurus.Engine.Console
{
    class Program
    {
        /// <summary>
        /// This will start the engine.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var path = Directory.GetCurrentDirectory();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
               // .WriteTo.Console()
                .WriteTo.File($"{path}\\Logs\\{DateTime.Now.Date.ToString("yyyyMMdd")}.log")
                .CreateLogger();
            //Dependecny Injection for classes, DBContext and Logger 
            //Inject SQL connection to DB context
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IThesaurusEngine, ThesaurusEngine>();
                    services.AddScoped<IThesaurus, ThesaurusService>();

                    services.AddDbContext<ThesaurusDbContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("connectionstring")));
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<ThesaurusEngine>(host.Services);
            await svc.Start();
        }

        /// <summary>
        /// Build configuration
        /// </summary>
        /// <param name="builder"></param>
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    }
}
