namespace Taogar.PassStorage.Core;

/// <summary>
/// Key Analyzer
/// </summary>
public static class PemKeyAnalyzer
{
    /// <summary>
    /// Check is the key encrypted
    /// </summary>
    /// <param name="pemContent">Key on the string format</param>
    /// <returns>Result</returns>
    public static bool IsKeyEncrypted(string pemContent)
    {
        // PKCS#8
        if (pemContent.Contains("-----BEGIN ENCRYPTED PRIVATE KEY-----"))
            return true;
        // PKCS#1
        if (pemContent.Contains("-----BEGIN RSA PRIVATE KEY-----") && 
            pemContent.Contains("Proc-Type: 4,ENCRYPTED"))
            return true;
        // EC
        if (pemContent.Contains("-----BEGIN EC PRIVATE KEY-----") && 
            pemContent.Contains("Proc-Type: 4,ENCRYPTED"))
            return true;
        return false;
    }
    
    /// <summary>
    /// Get type of the key
    /// </summary>
    /// <param name="pemContent">Key on the string format</param>
    /// <returns>Type of the key</returns>
    public static KeyTypes GetKeyType(string pemContent)
    {
        if (pemContent.Contains("-----BEGIN RSA PRIVATE KEY-----"))
            return KeyTypes.RSA_PKCS1;
        if (pemContent.Contains("-----BEGIN PRIVATE KEY-----"))
            return KeyTypes.PKCS8;
        if (pemContent.Contains("-----BEGIN ENCRYPTED PRIVATE KEY-----"))
            return KeyTypes.ENCRYPTED_PKCS8;
        if (pemContent.Contains("-----BEGIN EC PRIVATE KEY-----"))
            return KeyTypes.EC;
        return KeyTypes.UNKNOWN;
    }

    /// <summary>
    /// Key types
    /// </summary>
    public enum KeyTypes
    {
        UNKNOWN,
        RSA_PKCS1,
        PKCS8,
        ENCRYPTED_PKCS8,
        EC
    }
}
