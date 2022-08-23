namespace MigratorLogParser
{
    using CommandLine;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using MigratorLogParser.Commands.ParseDataMigrationTool;
    using MigratorLogParser.Exporters;
    using MigratorLogParser.Parsers.DataMigrationTool;
    using System.IO.Abstractions;
    using System.Reflection;

    public static class Program
    {
        private static IConfigurationRoot Configuration { get; set; }
        private static IServiceProvider ServiceProvider { get; set; }
        private static IMediator Mediator { get; set; }

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();

            ServiceProvider = ConfigureServices();

            Mediator = ServiceProvider.GetRequiredService<IMediator>();

            Parser.Default.ParseArguments<ParseDataMigrationLogCommand>(args).WithParsedAsync(async c => await Mediator.Send(c));
        }

        static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(options => options.AddConsole());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IFileExporter, CsvExporter>();
            services.AddSingleton<DataMigrationToolLogParser>();

            return services.BuildServiceProvider();
        }
    }
}