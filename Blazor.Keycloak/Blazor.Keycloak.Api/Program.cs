using System;
using System.Threading.Tasks;
using Blazor.Keycloak.Api;
using Blazor.Keycloak.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Blazor.Keycloak.Api
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                logger.LogDebug("Starting api host");
                // Create a new scope
                using (var scope = host.Services.CreateScope())
                {
                    // Get the DbContext instance
                    var contributionsService = scope.ServiceProvider.GetRequiredService<ContributionsService>();
                    var speakerService = scope.ServiceProvider.GetRequiredService<SpeakerService>();

                    //Do the migration asynchronously
                    await contributionsService.InitAsync();
                    await speakerService.InitAsync();
                }

                // Run the WebHost, and start accepting requests
                // There's an async overload, so we may as well use it
                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "A fatal error caused service to crash");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) => configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Console()
                )
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}