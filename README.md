# dotSpace: Tuple Space on .NET

## Introduction

This framework provides support to concurrent and distributed programming with (tuple) spaces in .NET.

The framework was originally designed and developed by [Shane D. McLean](https://github.com/sequenze) in collaboration with [Alberto Lluch Lafuente](https://github.com/albertolluch).

This fork is a port of the application to support .NET Standard 2.0. With this port, the library could be used from all three popular .NET runtime: .NET Framework on Windows, and .NET Core and Mono on both Windows and *nix. Ultimately the goal is to have the library support an object space - an extension of the original tuple space idea.

The goal of the fork is to support [object space](docs/Proposal.md), an extension on the original idea of tuple space. THis fork is maintained by Tuan Tran and Daishiro Nishida as part of their project for Williams College's Distributed System course.

## Building

The library could be built on all three popular .NET platform as long as the version used follows .NET Standard 2.0. It is advised that you have the latest version of the framework installed. For *nix, you need to download and install either .NET Core 2.0 or above, or Mono 5.2 or above. On Windows, .NET Framework is installed by default. See the instruction for building on .NET Framework and Windows below. Note that you could still use .NET Core and Mono on Windows if you want to.

(if it is beneficial to you, I'm developing the library on .NET Core 2.0 on a Mac).

### .NET Core

Make sure you have .NET Core version 2.0 or above. .NET Core could be downloaded from [Microsoft](https://www.microsoft.com/net/learn/get-started)

To build the project head to the root directory and run

```bash
dotnet restore
dotnet build
```

To run an example, do

```bash
dotnet run -p examples/{example name}/{example name}.csproj
```

For example `dotnet run -p examples/Example1/Example1.csproj`

### Mono

Make sure you have Mono version 5.2 or above. Mono could be downloaded from [the Mono project](http://www.mono-project.com/download/)

To build the project head to the root directory and run

```bash
msbuild /t:restore
msbuild
```

To run an example, do

```bash
mono examples/{example name}/bin/{Debug/Release}/netcoreapp2.0/{example name}.dll
```

For example `mono examples/Example1/bin/{Debug/Release}/netcoreapp2.0/Example1.dll`

Alternatively you could use IDEs such as MonoDevelop, JetBrains Rider or Visual Studio, which should simplify the job of building and running. Just import the project into the IDEs, build and run using the menu bar or the solution sidebar. Make sure you have the latest version of the IDE and that it supports Mono 5.2/.NET Core 2.0 and above.

### .NET Framework

Given that .NET Framework is not optimized for development from the command line , the easiest way to build and run from .NET Framework would be to use Visual Studio. Download the latest version of [Visual Studio](https://www.visualstudio.com/downloads/), which should come with the latest .NET Framework. Simply import the solution to Visual Studio, build solution and run projects from the menu bar or solution sidebar.

Please message me if you find a way to build & run the library easily on .NET Framework.

### IDEs and Other

As mentioned, Visual Studio (on Windows and Mac), [MonoDevelop](http://www.monodevelop.com/download/), and [JetBrains Rider](https://www.jetbrains.com/rider/download/) are good .NET IDEs and should simplify development. If you prefer using a text editor, [Omnisharp](http://www.omnisharp.net/#integrations) provides plugins for a wide array of popular tools.

Follow these links for more information on building and running on .NEt Core, Mono, or .Net Framework.

## Additional Documentation

Please go to dotSpace's [wiki page](https://github.com/pSpaces/dotSpace/wiki) for [documentation on the API](https://github.com/pSpaces/dotSpace/wiki/basics) and [dotSpace-examples' wiki page](https://github.com/pSpaces/dotSpace-Examples/wiki) for information about the examples. We currently have the first seven examples in the library for testing purposes.