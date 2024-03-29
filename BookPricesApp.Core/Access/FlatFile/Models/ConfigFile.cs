using BookPricesApp.Core.Domain.Config;
using BookPricesApp.Core.Utils;
using Newtonsoft.Json;

namespace BookPricesApp.Core.Access.FlatFile.Models;
internal class ConfigFile
{
    private string _path => $"{Directory.GetCurrentDirectory()}\\Domain\\Files\\Config.json";
    private readonly object _configFileLock = new object();

    private AppConfig _config;

    public ConfigFile()
    {
        _config = new AppConfig();
    }

    public Option SaveAppConfig()
    {
        try
        {
            lock (_configFileLock)
            {
                if (!File.Exists(_path))
                {
                    var ex = new Exception($"{_path} path not found");
                    return new Option(ex);
                }
                var text = JsonConvert.SerializeObject(_config, Formatting.Indented);
                File.WriteAllText(_path, text);
            }
        }
        catch (Exception ex)
        {
            return new Option(ex);
        }
        
        return new();
    }

    public Option<AppConfig> GetAppConfig(string? path = null)
    {
        try
        {
            lock (_configFileLock)
            {
                var file = path != null ? File.ReadAllText(path) : File.ReadAllText(_path);
                _config = JsonConvert.DeserializeObject<AppConfig>(file)!;
                return new Option<AppConfig> { Data = _config };
            }
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
}
