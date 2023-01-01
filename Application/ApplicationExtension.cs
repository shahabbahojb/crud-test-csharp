
using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            

            services.AddMvc()
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);
            
            return services;
        }
        
    }
}