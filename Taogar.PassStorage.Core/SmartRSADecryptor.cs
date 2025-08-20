using System.Security.Cryptography;
using System.Text;

namespace Taogar.PassStorage.Core;

/// <summary>
/// Encrypt manager
/// </summary>
public class SmartRSADecryptor
{
    /// <summary>
    /// Password request function
    /// </summary>
    private readonly Func<string> _passwordCallback;
    /// <summary>
    /// Configuration
    /// </summary>
    private Config _conf;
    /// <summary>
    /// RSA instance
    /// </summary>
    private RSA _rsa;
    
    /// <summary>
    /// New instance
    /// </summary>
    /// <param name="conf">Instance of configuration</param>
    /// <param name="passwordCallback">Function for request password from user</param>
    public SmartRSADecryptor(Config conf, Func<string> passwordCallback = null)
    {
        _rsa = RSA.Create();
        _conf =  conf;
        _passwordCallback = passwordCallback ?? (() => 
        {
            Console.Write($"Введите пароль для приватного ключа [{Path.GetFileName(_conf.PrivKeyPath)}]: ");
            return Utils.ReadPassword();
        });
    }

    /// <summary>
    /// Method for encrypt data
    /// </summary>
    /// <param name="encrypt">Source data</param>
    /// <returns>Encrypted data</returns>
    public byte[] Encrypt(byte[] encrypt)
    {
        var pubKeyPem = File.ReadAllText(_conf.PubKeyPath);
        _rsa.ImportFromPem(pubKeyPem);
        return _rsa.Encrypt(encrypt, RSAEncryptionPadding.OaepSHA1);
    }
    
    /// <summary>
    /// Method for import key in instance
    /// </summary>
    /// <returns>Result of try import</returns>
    /// <exception cref="Exception">Password was null/empty/wrong</exception>
    public bool ImportKey()
    {
        var privateKeyPem = File.ReadAllText(_conf.PrivKeyPath);
        try
        {
            if (PemKeyAnalyzer.IsKeyEncrypted(privateKeyPem))
            {
                string password = _passwordCallback();
                if (string.IsNullOrEmpty(password))
                    throw new Exception("Password is required for encrypted key");
                _rsa.ImportFromEncryptedPem(privateKeyPem, password);
                return true;
            }
            else
            {
                _rsa.ImportFromPem(privateKeyPem);
                return true;
            }
        }
        catch (CryptographicException ex) when (ex.Message.Contains("The length of the data to decrypt is not valid for the size of this key"))
        {
            throw new Exception("Incorrect password");
        }
        catch (ArgumentException ex) when (ex.Message.Contains("encrypted PEM-encoded key"))
        {
            string password = _passwordCallback();
            _rsa.ImportFromEncryptedPem(privateKeyPem, password);
            return true;
        }
        catch (CryptographicException ex) when (
            ex.Message.Contains("password may be incorrect") ||
            ex.Message.Contains("The EncryptedPrivateKeyInfo structure was decoded") ||
            ex.Message.Contains("Padding is invalid and cannot be removed"))
        {
            throw new Exception("Incorrect password");
        }
        catch (CryptographicException ex) when (ex.Message.Contains("password is incorrect"))
        {
            throw new Exception("Incorrect password");
        }
    }
    
    /// <summary>
    /// Method for decrypt data
    /// </summary>
    /// <param name="encrypted">Encrypted data</param>
    /// <returns>Decrypted data</returns>
    /// <exception cref="Exception">Password was null/empty/wrong</exception>
    public string DecryptWithPemKey(byte[] encrypted)
    {
        var privateKeyPem = File.ReadAllText(_conf.PrivKeyPath);
        if (ImportKey())
        {
            var decryptedBytes = _rsa.Decrypt(encrypted, RSAEncryptionPadding.OaepSHA1);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        throw new Exception("Incorrect password");
    }
}
