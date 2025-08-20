using System.Diagnostics;

namespace Taogar.PassStorage.Core;

/// <summary>
/// Utilities
/// </summary>
public static class Utils
{
    /// <summary>
    /// Copy to clipboard
    /// </summary>
    /// <param name="text">Text for copy</param>
    public static void CopyToClipboard(string text)
    {
        var psi = new ProcessStartInfo()
        {
            FileName = "wl-copy",
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        if (process != null)
        {
            using var writer = process.StandardInput;
            writer.Write(text);
        }
        process.WaitForExit();
    }
    
    /// <summary>
    /// Check is command available
    /// </summary>
    /// <param name="commandName">terminal command</param>
    /// <returns>Is command available</returns>
    public static bool IsCommandAvailable(string commandName)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "which",
                Arguments = commandName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return !string.IsNullOrWhiteSpace(output);
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Base password request method
    /// </summary>
    /// <returns>Password</returns>
    public static string ReadPassword(string notice = null)
    {
        if (notice != null)
        {
            Console.WriteLine(notice);
        }
        string password = "";
        ConsoleKeyInfo keyInfo;
        do
        {
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
            {
                password += keyInfo.KeyChar;
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        }
        while (keyInfo.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return password;
    }
}