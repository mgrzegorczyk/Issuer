using Issuer.BusinessLogic;
using Issuer.BusinessLogic.Github;
using Issuer.BusinessLogic.GitLab;
using Issuer.BusinessLogic.Interfaces;

namespace Issuer.ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("*** Issuer V1.0 by Marcin Grzegorczyk Software ***");
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: <github|gitlab> <arguments>");
            return;
        }

        string platform = args[0].ToLower();
        IIssuesHostingService? service = null;

        switch (platform)
        {
            case "github":
                if (args.Length < 4)
                {
                    Console.WriteLine("Usage: github <repositoryOwner> <repositoryName> <authToken>");
                    return;
                }
                service = new GitHubService(args[1]!, args[2]!, args[3]!);
                break;

            case "gitlab":
                if (args.Length < 3)
                {
                    Console.WriteLine("Usage: gitlab <repositoryId> <authToken>");
                    return;
                }
                service = new GitLabService(args[1]!, args[2]!);
                break;

            default:
                Console.WriteLine("Unknown platform specified. Use 'github' or 'gitlab'.");
                return;
        }

        var manager = new IssuesManager(service);

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Enter command (export, import, create, close, setname, setdesc, exit):");
            var command = Console.ReadLine()?.ToLower();

            if (string.IsNullOrEmpty(command))
            {
                Console.WriteLine("Unknown command.");
                continue;
            }

            switch (command)
            {
                case "export":
                    Console.WriteLine("Enter file path:");
                    var exportPath = Console.ReadLine();
                    if (!string.IsNullOrEmpty(exportPath))
                    {
                        await manager.ExportIssuesAsync(exportPath);
                        Console.WriteLine("Issues exported successfully.");
                    }
                    break;

                case "import":
                    Console.WriteLine($"[!info] issuesToImport.txt contains default data");
                    Console.WriteLine("Enter file path:");
                    var importPath = Console.ReadLine();
                    if (!string.IsNullOrEmpty(importPath))
                    {
                        await manager.ImportIssuesAsync(importPath);
                        Console.WriteLine("Issues imported successfully.");
                    }
                    break;

                case "create":
                    Console.WriteLine("Enter issue title:");
                    var title = Console.ReadLine();
                    Console.WriteLine("Enter issue description:");
                    var description = Console.ReadLine();
                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(description))
                    {
                        await manager.CreateIssueAsync(title, description);
                        Console.WriteLine("Issue created successfully.");
                    }
                    break;

                case "close":
                    Console.WriteLine("Enter issue number:");
                    var closeNumberInput = Console.ReadLine();
                    if (long.TryParse(closeNumberInput, out long closeNumber))
                    {
                        await manager.CloseIssueAsync(closeNumber);
                        Console.WriteLine("Issue closed successfully.");
                    }
                    break;

                case "setname":
                    Console.WriteLine("Enter issue number:");
                    var nameNumberInput = Console.ReadLine();
                    Console.WriteLine("Enter new issue title:");
                    var newName = Console.ReadLine();
                    if (long.TryParse(nameNumberInput, out long nameNumber) && !string.IsNullOrEmpty(newName))
                    {
                        await manager.SetIssueNameAsync(nameNumber, newName);
                        Console.WriteLine("Issue title updated successfully.");
                    }
                    break;

                case "setdesc":
                    Console.WriteLine("Enter issue number:");
                    var descNumberInput = Console.ReadLine();
                    Console.WriteLine("Enter new issue description:");
                    var newDescription = Console.ReadLine();
                    if (long.TryParse(descNumberInput, out long descNumber) && !string.IsNullOrEmpty(newDescription))
                    {
                        await manager.SetIssueDescriptionAsync(descNumber, newDescription);
                        Console.WriteLine("Issue description updated successfully.");
                    }
                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}