using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;
using ProtectorTokenAuth.Services;
using System.Linq;
using ProtectorTokenAuth.Models;

namespace ProtectorTokenAuth.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        /// <summary>
        /// Injects the DataProtection and Auth services with given options and returns the IDataProtectionBuilder to operate if needed.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="protectorName">The unique name for the data protector initialization. Different names create different tokens.</param>
        /// <param name="tokenExpirationDays">The time in days for each an access token remains valid.</param>
        public static IDataProtectionBuilder AddProtectorAuthWithOptions(this IServiceCollection services, 
            string protectorName = Constants.DEFAULT_DATA_PROTECTION_PROVIDER_NAME, 
            int tokenExpirationDays = Constants.DEFAULT_TOKEN_EXPIRATION_DAYS)
        {
            IDataProtectionBuilder dpBuilder = null;

            if (!services.Any(x => x.ServiceType == typeof(IDataProtectionBuilder)))
                dpBuilder = services.AddDataProtection();

            services.AddScoped<IAuthService>(x => new AuthService(x.GetService<IDataProtectionProvider>(), 
                protectorName, tokenExpirationDays));
            
            return dpBuilder;
        }

        /// <summary>
        /// Injects the DataProtection and Auth services and returns the IDataProtectionBuilder to operate if needed.
        /// </summary>
        /// <param name="services"></param>
        public static IDataProtectionBuilder AddProtectorAuth(this IServiceCollection services)
        {
            IDataProtectionBuilder dpBuilder = null;

            if (!services.Any(x => x.ServiceType == typeof(IDataProtectionBuilder)))
                dpBuilder = services.AddDataProtection();

            services.AddScoped<IAuthService, AuthService>();
            
            return dpBuilder;
        }
    }
}
