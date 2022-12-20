using System.Net;
using System.Reflection;

using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;

using PSSN.Api.Extensions.SerilogEnricher;
using PSSN.Api.ServiceInterfaces;
using PSSN.Api.Services;
using PSSN.Core;
using PSSN.Core.Matricies;
using PSSN.Core.Round;

using Serilog;
using Serilog.OpenTelemetry;

namespace PSSN.Api;

public static class Startup
{
    internal static WebApplicationBuilder ConfigureHost(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, lc) => lc
            .Enrich.WithCaller()
            .Enrich.WithResource(
                ("server", Environment.MachineName),
                ("app", AppDomain.CurrentDomain.FriendlyName))
            .WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration)
        );
        
        builder.WebHost.ConfigureKestrel((_, opt) =>
        {
            var host = builder.Configuration.GetValue<string>("App:Host");
            var port = builder.Configuration.GetValue<int>("App:Port");

            opt.Limits.MinRequestBodyDataRate = null;

            opt.Listen(IPAddress.Parse(host), port, listenOptions =>
            {
                Log.Information(
                    "The application rest api (http1) [{AppName}] is successfully started at [{StartTime}] (UTC)",
                    AppDomain.CurrentDomain.FriendlyName,
                    DateTime.UtcNow.ToString("F"));

                listenOptions.Protocols = HttpProtocols.Http1;
            });

            opt.AllowAlternateSchemes = true;
        });
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ClientPermissionCombined", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials();
            });

            options.AddPolicy("ClientPermissionDivided", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://0.0.0.0:3000")
                    .AllowCredentials();
            });
        });
        
        builder.Services.AddSingleton<StrategiesContainer>();
        
        builder.Services.AddScoped<IParserService, ParserService>();
        
        builder.Services.AddScoped<IGameRunner, SimpleGameRunner>();
        builder.Services.AddScoped<PopulationFrequency>();
        
        builder.Services.AddAutoMapper(typeof(Program));
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "PSSN API",
                Description = "ASP.NET Core Web API"
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        
        return builder;
    }

    internal static WebApplication ConfigApp(WebApplication app, CancellationToken token)
    {
        /* for tests */
        using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            if (serviceScope != null)
            {
                
                var service = serviceScope.ServiceProvider.GetRequiredService<IParserService>();
                service.GetVectors();
            }
        }
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            Log.ForContext("Mode", app.Environment.EnvironmentName);
            Log.Debug("App activated in [{Environment}] mode", app.Environment.EnvironmentName);
            
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        
        app.UseCors("ClientPermissionCombined");
        
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller}/{action=Index}/{id?}");
        });
        
        app.MapFallbackToFile("index.html");
        
        return app;
    }
}