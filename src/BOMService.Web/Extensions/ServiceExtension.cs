using AutoMapper;
using BOMService.Application;
using BOMService.Application.Behaviors;
using BOMService.Application.Common.Interfaces;
using BOMService.Domain.Entities;
using BOMService.Domain.Repositories;
using BOMService.Infrastructure;
using BOMService.Infrastructure.Persistence.EFModels;
using BOMService.Infrastructure.Repositories;
using BOMService.Infrastructure.Services;
using BOMService.Web.Middleware;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BOMService.Web.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "BOM Service API", Version = "v1" });
            });
        }

        public static void RegisterApplicationLayers(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddThirdPartyServices(typeof(ApplicationAssemblyReference).Assembly);
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBOMEngineService, BOMEngineService>();
            services.AddScoped<IBOMEngineManagerService, BOMEngineManagerService>();
            services.AddScoped<IProductService, ProductService>();
        }

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("BOMDatabase")));
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static void AddThirdPartyServices(this IServiceCollection services, Assembly assembly)
        {
            services.AddAutoMapper(assembly);
            services.AddAutoMapper(typeof(InfrastructureAssemblyReference).Assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
        }
    }
}
