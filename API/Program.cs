
using API.Infrastructure;
using Application.Services;
using DataAccess;
using DataAccess.Repositories;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("AppContext")));

            builder.Services.AddScoped<ITimeIntervalsRepository, TimeIntervalRepository>();
            builder.Services.AddScoped<ISingleProductsRepository, SingleProductsRepository>();
            builder.Services.AddScoped<IComboProductsRepository, ComboProductsRepository>();
            builder.Services.AddScoped<IWindowsRepository, WindowsRepository>();
            builder.Services.AddScoped<IPricesRepository, PricesRepository>();
            builder.Services.AddScoped<IPricesService, PricesService>();
            builder.Services.AddScoped<ITimeIntervalsService, TimeIntervalsService>();
            builder.Services.AddScoped<IWindowsService, WindowsService>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
