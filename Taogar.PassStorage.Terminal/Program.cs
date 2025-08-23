using System.Runtime.InteropServices;
using CommandLine;
using Taogar.PassStorage.Core;
using Taogar.PassStorage.Core.Commands;

public static class Program
{
    private static PassManager PM;
    public static int Main(string[] args)
    {
        if (Init(out Config config))
        {
             PM = new PassManager(config);
             return Parser.Default.ParseArguments<ListCommand, AddCommand, GetCommand, RemoveCommand>(args)
             .MapResult<ListCommand, AddCommand, GetCommand, RemoveCommand, int>(
                 (ListCommand lc)=> lc.Execute(PM),
                 (AddCommand add)=> add.Execute(PM),
                 (GetCommand get)=> get.Execute(PM),
                 (RemoveCommand remove)=> remove.Execute(PM),
                 errs => 1);
        }
        return 0;
    }

    /// <summary>
    /// Initialization
    /// </summary>
    /// <param name="_config">Configuration</param>
    /// <returns>is initialization was success</returns>
    private static bool Init(out Config _config)
    {
        var result = true;
        if (!Utils.IsCommandAvailable("wl-copy"))
        {
            Console.WriteLine("wl-copy package not installed, can't start");
            _config = null;
            return false;
        }
        if (!Directory.Exists(Config.ConfigFolder))
        {
            Directory.CreateDirectory(Config.ConfigFolder);
            Console.WriteLine($"{Config.ConfigFolder}\nConfiguration folder created");
            result = false;
        }
        if (!File.Exists(Config.ConfigFile))
        {
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(new Config());
            File.WriteAllText(Config.ConfigFile, data);
            Console.WriteLine($"{Config.ConfigFile}\nConfiguration file created");
            result = false;
        }
        _config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config.ConfigFile));
        if (string.IsNullOrEmpty(_config.PrivKeyPath) | string.IsNullOrEmpty(_config.PubKeyPath) | string.IsNullOrEmpty(_config.StorageBinPath))
        { 
            Console.WriteLine($"Configuration file parameters contain empty lines, fill in the configuration file");
            result = false;
        }
        else
        {
            if (!File.Exists(_config.PrivKeyPath) | !File.Exists(_config.PubKeyPath))
            {
                Console.WriteLine($"Key files were not found at the address specified in the configuration.");
                result = false;
            }
        }
        return result;
    }
}