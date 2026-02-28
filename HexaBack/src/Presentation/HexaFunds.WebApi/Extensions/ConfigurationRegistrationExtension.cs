using Serilog;

namespace HexaFunds.WebApi.Extensions;

public static class ConfigurationRegistrationExtension
{
    public static WebApplicationBuilder AddCustomJsonConfigurations(this WebApplicationBuilder builder)
    {
        var configFolder = "Configuration";

        if (!Directory.Exists(configFolder))
        {
            Log.Logger.Error("WatchTower startup failed: Configuration directory '{ConfigFolder}' does not exist (check 'Config' in appsettings.json).", configFolder);
            Log.CloseAndFlush();
            throw new InvalidOperationException(
                $"WatchTower startup failed: Configuration directory '{configFolder}' does not exist (check 'Config' in appsettings.json).");
        }

        string[] configFiles =
        [
            "kestrel.json",
            "cors.json",
            "database.json",
            "serilog.json",
        ];

        foreach (string file in configFiles)
        {
            builder.Configuration.AddJsonFile(Path.Combine(configFolder, file), optional: false, reloadOnChange: true);
        }

        return builder;
    }
}
