namespace Taogar.PassStorage.Core.Commands;

/// <summary>
/// Base command class
/// </summary>
public abstract class BaseCommand
{
    /// <summary>
    /// Calling method of command
    /// </summary>
    /// <param name="PM">Password Manager</param>
    /// <returns>Operation result code (0 is OK)</returns>
    public abstract int Execute(PassManager PM);
}