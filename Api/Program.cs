namespace PSSN.Api;
using PSSN.Api.Data;
using Serilog;
using PSSN.Core;
using PSSN.Core.Matrices;
using PSSN.Core.Round;

public class Program
{
    private static void Main(string[] args)
    {
        // Add services to the container.
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped(cfg => "vectors.txt");
        builder.Services.AddScoped<ITestRepository, TestRepository>();
        builder.Services.AddScoped<IFileLineProvider, FileLineProvider>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddScoped<IServiceCollection>(_ => builder.Services);
        builder.Services.AddSingleton<StrategesContainer>();
        builder.Services.AddScoped<PopulationFrequency>();
        builder.Services.AddScoped<IGameRunner,ParallelGameRunner>();

        builder.Services.AddSwaggerGen();

        // builder.Services.AddCors(options => {
        //     options.
        // })
        var app = builder.Build();
        app.UseCors(cfg => {
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.AllowAnyOrigin();
            // cfg.AllowCredentials();
        });
        // app.Use(async (content,next) => {
        //     content.Request.Headers.Add("Content-Security-Policy", "default-src 'self';");
        //     await next();
        // });
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}