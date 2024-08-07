﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace My.Infrastructure.Localization;

public static class LocalizationServiceCollectionExtensions
{
    public static IServiceCollection AddLocalization(this IServiceCollection services, LocalizationProviders providers)
    {
        if (providers?.SqlServer?.IsEnabled ?? false)
        {
            services.Configure<SqlServerOptions>(op =>
            {
                op.ConnectionString = providers.SqlServer.ConnectionString;
                op.SqlQuery = providers.SqlServer.SqlQuery;
                op.CacheMinutes = providers.SqlServer.CacheMinutes;
            });

            services.AddSingleton<IStringLocalizerFactory, SqlServerStringLocalizerFactory>();
            services.AddScoped<IStringLocalizer>(provider => provider.GetRequiredService<IStringLocalizerFactory>().Create(null));
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("vi-VN"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
                {
                    // My custom request culture logic
                    // return new ProviderCultureResult("vi-VN");
                    return new ProviderCultureResult("en-US");
                }));
            });
        }

        return services;
    }
}
