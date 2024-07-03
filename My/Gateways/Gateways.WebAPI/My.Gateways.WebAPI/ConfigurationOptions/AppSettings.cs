namespace My.Gateways.WebAPI.ConfigurationOptions;

public class AppSettings
{
    public string ProxyProvider { get; set; }

    public OcelotOptions Ocelot { get; set; }
}
