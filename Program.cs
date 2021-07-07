using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PlaylisterCli
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            host.Services.GetRequiredService<AppHost>().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddFile(context.Configuration.GetSection("Logging"));
                })
                .ConfigureServices((context, services) =>
                {
                    if (context.HostingEnvironment.IsDevelopment()) ShowConfig(context.Configuration);

                    services
                        .AddSingleton<AppHost>()
                        .AddTransient<IDbService, DbService>();
                });

        /// <summary>
        ///     Write App Configuration to Console
        /// </summary>
        /// <param name="configuration"></param>
        private static void ShowConfig(IConfiguration configuration)
        {
            foreach (IConfigurationSection pair in configuration.GetChildren())
            {
                Console.WriteLine($"{pair.Path} - {pair.Value}");
                ShowConfig(pair);
            }
        }
    }
}
