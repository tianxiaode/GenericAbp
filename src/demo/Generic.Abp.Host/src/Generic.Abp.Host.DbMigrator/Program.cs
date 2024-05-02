using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Host.DbMigrator;

class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
            .MinimumLevel.Override("Generic.Abp.Host", LogEventLevel.Debug)
#else
                .MinimumLevel.Override("Generic.Abp.Host", LogEventLevel.Information)
#endif
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt", retainedFileCountLimit: 100, fileSizeLimitBytes: 10485760,
                encoding: Encoding.UTF8,
                rollOnFileSizeLimit: true))
#if DEBUG
            .WriteTo.Async(c => c.Console())
#endif
            .CreateLogger();

        await CreateHostBuilder(args).RunConsoleAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .AddAppSettingsSecretsJson()
            .ConfigureLogging((context, logging) => logging.ClearProviders())
            .ConfigureServices((hostContext, services) => { services.AddHostedService<DbMigratorHostedService>(); });
}