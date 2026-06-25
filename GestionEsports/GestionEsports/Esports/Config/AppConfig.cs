namespace GestionEsports.Esports.Config;
using Microsoft.Extensions.Configuration;

public class AppConfig {
    static AppConfig() {
        Config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }
    public static IConfiguration Config { get; }
    public static string DataFolder => Path.Combine(
        Environment.CurrentDirectory, 
        Config.GetValue<string>("Repository:Directory") ?? "data");
    
    

}
