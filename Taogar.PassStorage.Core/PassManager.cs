using System.Runtime.InteropServices;
using System.Text;
using TextCopy;

namespace Taogar.PassStorage.Core;

/// <summary>
/// Password Manager
/// </summary>
public class PassManager
{
    /// <summary>
    /// Configuration
    /// </summary>
    Config _config;
    private Dictionary<string, byte[]> storage;
    /// <summary>
    /// List of passwords
    /// </summary>
    public IEnumerable<string> Keys => storage.Keys;
    /// <summary>
    /// Encrypt manager instance
    /// </summary>
    SmartRSADecryptor _rsaDecryptor;

    /// <summary>
    /// Base constructor
    /// </summary>
    /// <param name="cfg">configuration</param>
    public PassManager(Config cfg)
    {
        _config = cfg;
        _rsaDecryptor = new SmartRSADecryptor(_config);

        if (!File.Exists(_config.StorageBinPath))
        {
            storage = new Dictionary<string, byte[]>();
            var data1 = DictionarySerializer.Serialize(storage);
            File.WriteAllBytes(_config.StorageBinPath, data1);
        }
        var data = File.ReadAllBytes(_config.StorageBinPath);
        storage = DictionarySerializer.Deserialize(data);
    }

    /// <summary>
    /// Add new password
    /// </summary>
    /// <param name="key">Name of password</param>
    /// <param name="value">Password</param>
    public void Add(string key, string value)
    {
        try
        {
            var data = Encoding.UTF8.GetBytes(value);
            var encrypted = _rsaDecryptor.Encrypt(data);
            storage.Add(key, encrypted);
            Save();
        }
        catch (ArgumentException e) when (e.Message.Contains("An item with the same key has already been added"))
        {
            Console.WriteLine($"Password '{key}' already exist");
        }
    }

    /// <summary>
    /// Delete exist password
    /// </summary>
    /// <param name="key">Name of password</param>
    public void Remove(string key)
    {
        if (key == null)
        {
            throw new Exception("Password name can not be null");
            return;
        }
        if (_rsaDecryptor.ImportKey())
        {
            if (storage.Remove(key))
            {
                Console.WriteLine("Password removed successfully");
                Save();
                return;
            }
            throw new Exception("Password not found");
        }
    }

    /// <summary>
    /// Get password by name
    /// </summary>
    /// <param name="key">Name of password</param>
    /// <returns>Password</returns>
    public string TryGet(string key)
    {
        if(key == null) Console.WriteLine("Password name can not be null");
        if (storage.TryGetValue(key, out var encryptedValue))
        {
            var pass = _rsaDecryptor.DecryptWithPemKey(encryptedValue);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                new Clipboard().SetText(pass);
                return $"{key} - has been copied to clipboard";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Utils.CopyToClipboardOnLinux(pass);
                return $"{key} - has been copied to clipboard";
            }
        }
        Console.WriteLine("Password not found");
        return null;
    }

    /// <summary>
    /// Case changes
    /// </summary>
    private void Save()
    {
        var data = DictionarySerializer.Serialize(storage);
        File.WriteAllBytes(_config.StorageBinPath, data);
    }
}