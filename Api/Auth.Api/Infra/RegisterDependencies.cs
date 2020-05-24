using System.Net.Http;
using System.Text.Json;
using Auth.Domain.Common.Mapper;
using Auth.Domain.Services.User;
using Auth.Infra;
using Auth.Infra.Repositories;
using Auth.SharedKernel.Domain.Notification;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Infra
{
    public static class RegisterDependencies
    {
        public static IServiceCollection Register(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc => 
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddScoped<INotification, Notification>();

            services.AddSingleton<DbContext>();

            services.RegisterServices();
            services.RegisterRepositories();

            return services;
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            return services;
        }

        //Repositories
        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }
    }
}