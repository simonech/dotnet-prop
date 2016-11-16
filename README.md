# dotnet-prop

## Why such a tool

With the move from the easily editable json based project file to the old and more difficult csproj MSBuild based file, I felt there was a need of an easier way to manage the properties of a project without going and messing with the XML based file.

During the Hackathon and 2016 MVP Summit I quicly wrote this simple dotnet-cli tool that allows to manage properties, without touching the XML file and without the need to remember the exact name of the XML tags.

## Installation

To use the tool in a dotnet core project (the new kind of project, based on Csproj file, not project.json) just add the a `DotNetCliToolReference` reference to the nuget package `dotnet-prop`.

```
<ItemGroup>
  ...
  <DotNetCliToolReference Include="dotnet-prop">
    <Version>0.1.0-*</Version>
  </DotNetCliToolReference>
  ...
</ItemGroup>
```

## Usage

### Commands available
```
>dotnet prop
.NET project parameter utility

Usage: dotnet prop [options] [command]

Options:
  -?|-h|--help  Show help information

Commands:
  add   Adds a parameter to the project file
  del   Removes a parameter from the project file
  list  Lists all parameters from the project file

Use "dotnet prop [command] --help" for more information about a command.
```

### Add command
Adds a property to the csproj project file

```
>dotnet prop add

Usage: dotnet prop add [options]

Options:
  -v|--version <VERSION>              Set version number
  -r|--runtime <RUNTIME_IDENTIFIERS>  Set supported runtimes
  -f|--framework <FRAMEWORK>          Set supported framework
  -?|-h|--help                        Show help information
```

### Del command
Removes a property from the csproj project file

```
>dotnet prop del


Usage: dotnet prop del [options]

Options:
  -v|--version  Removes version number
  -r|--runtime  Removes supported runtimes
  -?|-h|--help  Show help information
```

### List command
Lists the properties in the csproj project file

```
dotnet prop list
```
