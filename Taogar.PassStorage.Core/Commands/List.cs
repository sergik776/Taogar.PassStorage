using CommandLine;

namespace Taogar.PassStorage.Core.Commands;

/// <summary>
/// Command for get list of passwords
/// </summary>
[Verb( "list", HelpText = "Show all passwords")]
public class ListCommand : BaseCommand
{
    public override int Execute(PassManager PM)
    {
        try
        {
            Console.WriteLine("List of passwords:");
            foreach (var pmKey in PM.Keys)
            {
                Console.WriteLine(pmKey);
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 1;
        }
    }
}