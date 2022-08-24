namespace MigratorLogParser
{
    using CommandLine;
    using CommandLine.Text;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
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

            var parser = new Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<ParseDataMigrationLogCommand,object>(args);

            parserResult
                .WithNotParsed(errs => DisplayHelp(parserResult))
                .WithParsedAsync<ParseDataMigrationLogCommand>(async c => await Mediator.Send(c));
        }

        static void DisplayHelp<T>(ParserResult<T> result)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "Azure DevOps Services Migrator Log Parser 1.0.0";
                h.Copyright = "Copyright (c) 2022 Vinny Martins";
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }

        static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddLogging(options => options.AddSimpleConsole(f => f.IncludeScopes = true));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IFileExporter, CsvExporter>();
            services.AddSingleton<DataMigrationToolLogParser>();

            return services.BuildServiceProvider();
        }
    }
}