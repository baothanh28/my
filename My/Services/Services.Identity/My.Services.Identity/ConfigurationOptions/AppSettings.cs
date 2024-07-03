using My.Infrastructure.Caching;
using My.Infrastructure.Interceptors;
using My.Infrastructure.Logging;
using My.Infrastructure.Monitoring;
using My.Infrastructure.Notification;
using My.Services.Identity.IdentityProviders.Auth0;
using My.Services.Identity.IdentityProviders.Azure;
using CryptographyHelper.Certificates;

namespace My.Services.Identity.ConfigurationOptions;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }

    public LoggingOptions Logging { get; set; }

    public CachingOptions Caching { get; set; }

    public MonitoringOptions Monitoring { get; set; }

    public IdentityServerAuthentication IdentityServerAuthentication { get; set; }

    public NotificationOptions Notification { get; set; }

    public InterceptorsOptions Interceptors { get; set; }

    public IdentityProvidersOptions Providers { get; set; }
}

public class ConnectionStrings
{
    public string My { get; set; }

    public string MigrationsAssembly { get; set; }
}

public class IdentityServerAuthentication
{
    public string Provider { get; set; }

    public string Authority { get; set; }

    public string ApiName { get; set; }

    public bool RequireHttpsMetadata { get; set; }

    public OpenIddictOptions OpenIddict { get; set; }
}

public class OpenIddictOptions
{
    public string IssuerUri { get; set; }

    public CertificateOption TokenDecryptionCertificate { get; set; }

    public CertificateOption IssuerSigningCertificate { get; set; }
}

public class IdentityProvidersOptions
{
    public Auth0Options Auth0 { get; set; }

    public AzureAdB2COptions AzureActiveDirectoryB2C { get; set; }
}
