using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManager.RegisterSerive
{
    public static class MessageRegisterServicesExtension
    {

        public static void AddMessageServices( this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISendEmail, SendGridEmail>();
        }
    }
}
