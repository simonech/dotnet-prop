using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    const string VERSION = "VersionPrefix";
    const string FRAMEWORK = "TargetFramework";
    const string RUNTIME_IDENTIFIERS = "RuntimeIdentifiers";

    static void Main(string[] args)
    {
        //Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", @"C:\Program Files (x86)\dotnet\sdk\1.0.0-preview3-004056\MSBuild.dll");
        new Program().Run(args);
    }

    public void Run(string[] args)
    {

        //RemoveProperty("Version");
        //return;

        var app = new CommandLineApplication()
        {
            Name = "dotnet prop",
            FullName = ".NET project parameter utility",
            Description = "Sets and unsets parameters in the Csproj file"
        };
        app.HelpOption("-?|-h|--help");

        app.Command("add", command => {
            command.Description = "Adds a parameter to the project file";

            var versionOption = command.Option("-v|--version <VERSION>", "Set version number", CommandOptionType.SingleValue);
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
                    AddOrUpdateProperty(VERSION, versionOption.Value());
                    Console.WriteLine($"Setting {VERSION} to {versionOption.Value()}");
                }

                if (runtimesOption.HasValue())
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var option in runtimesOption.Values)
                    {
                        builder.Append($"{option};");
                    }
                    var runtimeString = builder.ToString().TrimEnd(';');
                    AddOrUpdateProperty(RUNTIME_IDENTIFIERS, runtimeString);
                    Console.WriteLine($"Setting {RUNTIME_IDENTIFIERS} to {runtimeString}");
                }
                if (frameworkOption.HasValue())
                {
                    AddOrUpdateProperty(FRAMEWORK, frameworkOption.Value());
                    Console.WriteLine($"Setting {FRAMEWORK} to {frameworkOption.Value()}");
                }

                return 0;
            });

        });

        app.Command("del", command => {
            command.Description = "Removes a parameter from the project file";
            var versionOption = command.Option("-v|--version", "Removes version number", CommandOptionType.NoValue);
            var runtimesOption = command.Option("-r|--runtime", "Removes supported runtimes", CommandOptionType.NoValue);
            //var frameworkOption = command.Option("-f|--framework", "Removes supported framework", CommandOptionType.NoValue);
            command.HelpOption("-?|-h|--help");


            command.OnExecute(() => {
                if (!versionOption.HasValue() && !runtimesOption.HasValue() && !frameworkOption.HasValue())
                {
                    command.ShowHelp();
                    return 2;
                }
                if (versionOption.HasValue())
                {
                    RemoveProperty(VERSION);
                }

                if (runtimesOption.HasValue())
                {
                    RemoveProperty(RUNTIME_IDENTIFIERS);
                }
                if (frameworkOption.HasValue())
                {
                    RemoveProperty(FRAMEWORK);
                }

                return 0;
            });

        });

        app.Command("list", command => {
            command.Description = "Lists all parameters from the project file";

            command.OnExecute(()=> {
                var msProj = GetMSBuildProject();
                foreach (var item in msProj.Properties)
                {
                    Console.WriteLine($" - {item.Name} = {item.Value}");
                }
                return 0;
            });

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

    private void RemoveProperty(string propertyName)
    {
        var msProj = GetMSBuildProject();

        var property = msProj.Properties.SingleOrDefault(p => p.Name == propertyName);

        if (property == null)
        {
            Console.WriteLine($"No property {propertyName} in the file");
            return;
        }

        Console.WriteLine($"Removing {propertyName}");
        property.Parent.RemoveChild(property);

        msProj.Save();
    }

    private ProjectRootElement GetMSBuildProject()
    {
        DirectoryInfo currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        FileInfo projectFile = currDir.GetFiles("*.csproj").FirstOrDefault();

        if (!projectFile.Exists)
        {
            throw new Exception("Unable to find any .csproj file in the current folder");
        }

        Console.WriteLine($"Updating file {projectFile.FullName}");

        return ProjectRootElement.Open(
            //projectFile.FullName,
            @"C:\Users\user\Documents\GitHub\dotnet-prop\ConsumerApp\ConsumerApp.csproj",
            ProjectCollection.GlobalProjectCollection
            );
    }
}