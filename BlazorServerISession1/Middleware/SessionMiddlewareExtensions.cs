using Blazored.SessionStorage.Serialization;
using BlazorServerISession1.Serialization;
using BlazorServerISession1.State;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerISession1.Middleware
{
    public static class SessionMiddlewareExtensions
    {
        /// <summary>
        /// Middleware 方法，提供 Session Storage 的存取
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services)
            => AddBlazoredSessionStorage(services, null);

        /// <summary>
        /// Middleware 方法，提供 Session Storage 的存取
        /// </summary>
        /// <param name="services">.NET Core 的 DI 容器服務</param>
        /// <param name="configure">JSON 序列化設定物件 (System.Text.Json.JsonSerializer)</param>
        /// <returns></returns>
        public static IServiceCollection AddBlazoredSessionStorage(this IServiceCollection services, Action<JsonSessionStorageOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IBrowserStorageProvider, BrowserStorageProvider>()
                .AddScoped<ISessionStorageService, SessionStorageService>()
                .AddScoped<ISyncSessionStorageService, SessionStorageService>()
                .Configure<JsonSessionStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}
