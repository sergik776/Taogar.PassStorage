using System.Runtime.InteropServices;

namespace Taogar.PassStorage.Core;

/// <summary>
/// Configurations
/// </summary>
public class Config
{
    /// <summary>
    /// Path to folder with configurations of application
    /// </summary>
    public static string ConfigFolder;
    /// <summary>
    /// Path to file with configurations of application
    /// </summary>
    public static string ConfigFile;
    /// <summary>
    /// Path to public key
    /// </summary>
    public string PubKeyPath { get; set; } = String.Empty;
    /// <summary>
    /// Path to private key
    /// </summary>
    public string PrivKeyPath { get; set; } = String.Empty;
    /// <summary>
    /// Path to binary file
    /// </summary>
    public string StorageBinPath  { get; set; } = String.Empty;

    static Config()
    {
        ConfigFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/binstorage";
        ConfigFile = $"{ConfigFolder}/configuration.json";
    }
}