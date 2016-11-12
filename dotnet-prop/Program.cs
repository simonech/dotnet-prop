using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Extensions.CommandLineUtils;
using System;

class Program
{
    static void Main(string[] args)
    {
        new Program().Run(args);
    }

    public void Run(string[] args)
    {
        var app = new CommandLineApplication();
        app.Name = "dotnet prop";
        app.FullName = ".NET propject parameter utility";
        app.Description = "Sets and unsets parameters in the Csproj file";

        app.HelpOption("-?|-h|--help");

        app.Command("add", command => {
            command.Description = "Adds a parameter to the project file";

            var versionOption = command.Option("-v|--version <version>", "Set version number", CommandOptionType.SingleValue);
            var runtimesOption = command.Option("-r|--runtime <RUNTIME_IDENTIFIERS>", "Set supported runtimes", CommandOptionType.MultipleValue);
            var frameworkOption = command.Option("-f|--framework <FRAMEWORK>", "Set supported framework", CommandOptionType.SingleValue);
            command.HelpOption("-?|-h|--help");

            command.OnExecute(() => {
                if (!versionOption.HasValue() && !runtimesOption.HasValue() && !frameworkOption.HasValue())
                {
                    command.ShowHelp();
                    return 2;
                }
                if (versionOption.HasValue())
                {
                    AddOrUpdateProperty("VersionPrefix", versionOption.Value());
                    Console.WriteLine($"Setting VersionPrefix to {versionOption.Value()}");
                }
                if (runtimesOption.HasValue())
                {
                    foreach ()
                    {

                    }
                    AddOrUpdateProperty("RuntimeIdentifiers", versionOption.Value());
                    Console.WriteLine($"Setting VersionPrefix to {versionOption.Value()}");
                }
                if (frameworkOption.HasValue())
                {
                    AddOrUpdateProperty("TargetFramework", versionOption.Value());
                    Console.WriteLine($"Setting TargetFramework to {versionOption.Value()}");
                }

                return 0;
            });

        });

        app.Command("del", command => {
            command.Description = "Removes a parameter from the project file";
            command.HelpOption("-?|-h|--help");
        });

        app.Command("list", command => {
            command.Description = "Lists all parameters from the  project file";
            command.HelpOption("-?|-h|--help");
        });

        app.OnExecute(() =>
        {
            app.ShowHelp();
            return 2;
        });

        try
        {
            app.Execute(args);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void AddOrUpdateProperty(string property, string value)
    {
        var msProj = GetMSBuildProject();
        msProj.AddProperty(property, value);
        msProj.Save();
    }

    private ProjectRootElement GetMSBuildProject()
    {
        //TODO: fix absolule url :)
        return ProjectRootElement.Open(
            @"C:\Users\user\Documents\GitHub\dotnet-prop\ConsumerApp\ConsumerApp.csproj",
            ProjectCollection.GlobalProjectCollection
            );
    }
}