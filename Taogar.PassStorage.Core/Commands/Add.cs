using CommandLine;

namespace Taogar.PassStorage.Core.Commands;

/// <summary>
/// Command for add password in Storage
/// </summary>
[Verb("add", HelpText = "Add password")]
public class AddCommand : BaseCommand
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
            if (PM.Keys.Contains(Name))
            {
                Console.WriteLine("Password already exists");
                return 0;
            }
            else
            {
                Console.Write($"Input password: ");
                var pass = Utils.ReadPassword();
                PM.Add(Name, pass);
                return 0;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 1;
        }
    }
}

