using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlaylisterCli.Enums;
using PlaylisterCli.Repositories;
using PlaylisterCli.Services;
using Spectre.Console;

namespace PlaylisterCli
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            await host.Services.GetRequiredService<AppHost>().Run();
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
                        .Configure<DatabaseOptions>(context.Configuration.GetSection(DatabaseOptions.Database))
                        .AddSingleton<AppHost>()
                        .AddTransient<ISearchService, SearchService>()
                        .AddTransient<IDbRepository, DbRepository>();

                    // set Dapper to be compatible with snake_case table names
                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
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

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            AnsiConsole.MarkupLine(
                "[bold red]**********\nException was thrown and crashed the program:\n**********[/]");
            AnsiConsole.WriteException((e.ExceptionObject as Exception)!, ExceptionFormats.ShowLinks);
            Environment.Exit(100);
        }
    }
}
