using CommandLine;

namespace Taogar.PassStorage.Core.Commands;

/// <summary>
/// Command for delete password by name
/// </summary>
[Verb("remove", HelpText = "Remove password by name")]
public class RemoveCommand : BaseCommand
{
    /// <summary>
    /// Name of password
    /// </summary>
    [Option('n', "name", Required = true, HelpText = "The name of the password")]
    public string Name { get; set; }

    public override int Execute(PassManager PM)
    {
        try
        {
            PM.Remove(Name);
            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 1;
        }
    }
}