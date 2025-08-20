using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Class for dictionary conversion
/// </summary>
public static class DictionarySerializer
{
    /// <summary>
    /// Serialize Dictionary<string, byte[]> in to byte array
    /// Format: [key_lenght][key][value_length][value]
    /// </summary>
    /// <param name="dictionary">Dictionary fore serialize</param>
    /// <returns>Byte array</returns>
    public static byte[] Serialize(Dictionary<string, byte[]> dictionary)
    {
        if (dictionary == null)
            throw new ArgumentNullException(nameof(dictionary));

        var result = new List<byte>();

        foreach (var kvp in dictionary)
        {
            string key = kvp.Key;
            byte[] value = kvp.Value;
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyLengthBytes = BitConverter.GetBytes(keyBytes.Length);
            result.AddRange(keyLengthBytes);
            result.AddRange(keyBytes);
            byte[] valueLengthBytes = BitConverter.GetBytes(value?.Length ?? 0);
            result.AddRange(valueLengthBytes);
            if (value != null)
            {
                result.AddRange(value);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Deserialize byte array in to Dictionary<string, byte[]>
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <returns>Dictionary<string, byte[]></returns>
    public static Dictionary<string, byte[]> Deserialize(byte[] data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        var dictionary = new Dictionary<string, byte[]>();
        int offset = 0;
        while (offset < data.Length)
        {
            if (offset + 4 > data.Length)
                throw new InvalidOperationException("Unexpected end of data when try read length of key");
            int keyLength = BitConverter.ToInt32(data, offset);
            offset += 4;
            if (offset + keyLength > data.Length)
                throw new InvalidOperationException("Unexpected end of data when try read length of key");
            string key = Encoding.UTF8.GetString(data, offset, keyLength);
            offset += keyLength;
            if (offset + 4 > data.Length)
                throw new InvalidOperationException("Unexpected end of data when try read length of value");
            int valueLength = BitConverter.ToInt32(data, offset);
            offset += 4;
            byte[] value = null;
            if (valueLength > 0)
            {
                if (offset + valueLength > data.Length)
                    throw new InvalidOperationException("Unexpected end of data when try read length of value");
                value = new byte[valueLength];
                Array.Copy(data, offset, value, 0, valueLength);
                offset += valueLength;
            }
            dictionary[key] = value;
        }
        return dictionary;
    }
}
