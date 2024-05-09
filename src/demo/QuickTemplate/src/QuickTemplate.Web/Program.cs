using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QuickTemplate.Web;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .MinimumLevel.Override("OpenIddict", LogEventLevel.Information)
            .MinimumLevel.Override("QuickTemplate", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt", retainedFileCountLimit: 100, fileSizeLimitBytes: 10485760,
                encoding: Encoding.UTF8,
                rollOnFileSizeLimit: true))
#if DEBUG
            .WriteTo.Async(c => c.Console())
#endif
            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .UseContentRoot(Directory.GetCurrentDirectory())
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<QuickTemplateWebModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}