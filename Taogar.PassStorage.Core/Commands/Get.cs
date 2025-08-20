using CommandLine;

namespace Taogar.PassStorage.Core.Commands;

/// <summary>
/// Command for get password
/// </summary>
[Verb("get", HelpText = "Get password by name")]
public class GetCommand : BaseCommand
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
            Console.WriteLine(PM.TryGet(Name));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 1;
        }
        return 0;
    }
}