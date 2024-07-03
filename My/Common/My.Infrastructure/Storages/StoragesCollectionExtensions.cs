using My.Infrastructure.Storages;
using My.Infrastructure.Storages.Amazon;
using My.Infrastructure.Storages.Azure;
using My.Infrastructure.Storages.Fake;
using My.Infrastructure.Storages.Local;

namespace Microsoft.Extensions.DependencyInjection;

public static class StoragesCollectionExtensions
{
    public static IServiceCollection AddLocalStorageManager(this IServiceCollection services, LocalOptions options)
    {
        services.AddSingleton<IFileStorageManager>(new LocalFileStorageManager(options));

        return services;
    }

    public static IServiceCollection AddAzureBlobStorageManager(this IServiceCollection services, AzureBlobOption options)
    {
        services.AddSingleton<IFileStorageManager>(new AzureBlobStorageManager(options));

        return services;
    }

    public static IServiceCollection AddAmazonS3StorageManager(this IServiceCollection services, AmazonOptions options)
    {
        services.AddSingleton<IFileStorageManager>(new AmazonS3StorageManager(options));

        return services;
    }

    public static IServiceCollection AddFakeStorageManager(this IServiceCollection services)
    {
        services.AddSingleton<IFileStorageManager>(new FakeStorageManager());

        return services;
    }

    public static IServiceCollection AddStorageManager(this IServiceCollection services, StorageOptions options)
    {
        if (options.UsedAzure())
        {
            services.AddAzureBlobStorageManager(options.Azure);
        }
        else if (options.UsedAmazon())
        {
            services.AddAmazonS3StorageManager(options.Amazon);
        }
        else if (options.UsedLocal())
        {
            services.AddLocalStorageManager(options.Local);
        }
        else
        {
            services.AddFakeStorageManager();
        }

        return services;
    }
}
