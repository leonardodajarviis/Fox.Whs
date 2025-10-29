using Fox.Whs.Options;
using Fox.Whs.Services;

namespace Fox.Whs.Extensions;

public static class SapServiceLayerExtensions
{
    /// <summary>
    /// Đăng ký SAP Service Layer Authentication Service
    /// </summary>
    public static IServiceCollection AddSapServiceLayer(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Đăng ký Options
        services.Configure<SapServiceLayerOptions>(
            configuration.GetSection(SapServiceLayerOptions.SectionName));

        // Đăng ký HttpClient với timeout
        services.AddHttpClient<SapServiceLayerAuthService>((serviceProvider, client) =>
        {
            var options = configuration
                .GetSection(SapServiceLayerOptions.SectionName)
                .Get<SapServiceLayerOptions>();

            if (options != null)
            {
                client.Timeout = TimeSpan.FromSeconds(options.HttpTimeoutSeconds);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
        });

        return services;
    }
}
