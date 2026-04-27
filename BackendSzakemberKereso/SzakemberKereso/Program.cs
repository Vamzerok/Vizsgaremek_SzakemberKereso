using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using SzakemberKereso.Models;
using SzakemberKereso.Utils;
using SzakemberKereso.DTOs.Expert;
using SzakemberKereso.DTOs.ExpertSpecialty;
using SzakemberKereso.DTOs.Job;
using SzakemberKereso.Services;
using SzakemberKereso.DTOs.Pricing;
using SzakemberKereso.DTOs.ResidentialAddress;
using SzakemberKereso.DTOs.Service;
using SzakemberKereso.DTOs.User;
using FluentValidation;
using SzakemberKereso.DTOs.Settlement;

namespace SzakemberKereso
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("vrdb");
            if(string.IsNullOrWhiteSpace(connectionString)) throw new ApplicationException("Connection string 'vrdb' not found.");

            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            //auth stuff
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();

            builder.Services.AddIdentity<User,Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("dev", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //mapster
            ConfigureMapster();

            //validators
            builder.Services.AddScoped<IValidator<PricingDto>, PricingDtoValidator>();
            builder.Services.AddScoped<IValidator<InputServiceDto>, InputServiceDtoValidator>();
            builder.Services.AddScoped<IValidator<InputSettlementDto>, InputSettlementDtoValidator>();

            //custom services
            builder.Services.AddScoped<AddressService>();
            builder.Services.AddScoped<TimeIntervalService>();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //should catch all unhandled exceptions
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("{\"error\":\"Szerverhiba történt\"}");
                });
            });

            app.UseCors("dev");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using(var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<Context>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<Role>>();
                    context.Database.Migrate();
                    DummyDataGenerator.GenerateDummyData(context, userManager, roleManager).Wait();
                }
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        public static void ConfigureMapster()
        {
            TypeAdapterConfig<Expert, OutputExpertDto>.NewConfig()
                .Map(dest => dest.FirstName, src => src.User.FirstName)
                .Map(dest => dest.LastName, src => src.User.LastName)
                .Map(dest => dest.Email, src => src.User.Email)
                .Map(dest => dest.PhoneNumber, src => src.User.PhoneNumber)
            ;
            TypeAdapterConfig<ExpertSpecialty, OutputExpertSpecialtyDto>.NewConfig()
                .Map(dest => dest.Name, src => src.Occupation.Name)
                .Map(dest => dest.Description, src => src.Occupation.Description)
            ;
            TypeAdapterConfig<User, OutputUserDto>.NewConfig()
                .Map(dest => dest.Roles, src => Array.Empty<string>()) //roles not loaded in job context
            ;
            TypeAdapterConfig<Job, OutputJobDto>.NewConfig()
                .Map(dest => dest.UserAvailability, src => src.TimeIntervals.Where(t => t.Type == TimeIntervalType.Available))
                .Map(dest => dest.OfferedAppointments, src => src.TimeIntervals.Where(t => t.Type == TimeIntervalType.Offered))
            ;
            TypeAdapterConfig<Service, OutputServiceDto>.NewConfig()
                .Map(dest => dest.ExpertName, src => src.ExpertSpecialty.Expert.User.LastName + " " + src.ExpertSpecialty.Expert.User.FirstName)
                .Map(dest => dest.ExpertUserId, src => src.ExpertSpecialty.Expert.UserId)
            ;
        }
    }
}
