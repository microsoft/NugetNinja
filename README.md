# Nuget Ninja (A Hackthon project)

![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)
![Build Status](https://github.com/microsoft/NugetNinja/actions/workflows/build.yml/badge.svg)

(Nuget Ninjia was not built or released as a production product. Instead it was our hackthon project while we prefer opensource. It was not officialy release by Microsoft as a product.)

(Non-production! This project is still working in progress...)

Nuget Ninjia is a tool for detecting dependencies of .NET projects. It analyzes the dependency structure of .NET projects in a directory and builds a directed acyclic graph. And will give some modification suggestions for Nuget packages, so that the dependencies of the project are as concise and up-to-date as possible.

The tool can also generate a list of all top level dependencies into a CSV file. This CSV includes the nuget description and avoids issues with `shproj` and `dotnet list packages`.

## Usage

After getting the binary, run it directly in the terminal.

```cmd
C:\workspace> ninja.exe

Description:
  Nuget Ninja, a tool for detecting dependencies of .NET projects.

Usage:
  Microsoft.NugetNinja [command] [options]

Options:
  -p, --path <path> (REQUIRED)   Path of the projects to be changed.
  --nuget-server <nuget-server>  If you want to use a customized nuget server instead of the official nuget.org, 
  --token <token>                The PAT token which has privilege to access the nuget server.
  -d, --dry-run                  Preview changes without actually making them
  -v, --verbose                  Show detailed log
  -?, -h, --help                 Show help and usage information

Commands:
  all, all-officials  The command to run all officially supported features.
  remove-deprecated   The command to replace all deprecated packages to new packages.
  upgrade-pkg         The command to upgrade all package references to possible latest and avoid conflicts.
  clean-pkg           The command to clean up possible useless package references.
  clean-prj           The command to clean up possible useless project references.
  list-packages       The command to generate a csv file with all direct packages and nuget.org descriptions. Dry run will only list the packages on the console.
```

## How to build and run locally

Requirements about how to develop.

* [.NET SDK 8.0](https://github.com/dotnet/core/tree/master/release-notes)

1. Execute `dotnet restore` to restore all .NET dependencies.
2. Execute the following command to build the app:
   * `dotnet publish -c Release -r win-x64   --self-contained` on Windows.
   * `dotnet publish -c Release -r linux-x64 --self-contained` on Linux.
   * `dotnet publish -c Release -r osx-x64   --self-contained` on Mac OS.
3. Execute `dotnet run` to run the app

## Run in Microsoft Visual Studio

1. Open the `.sln` file in the project path.
2. Press `F5`.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
