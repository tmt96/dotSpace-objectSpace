# dotSpace: Tuple Space on .NET

## Introduction

This framework provides support to concurrent and distributed programming with (tuple) spaces in .NET. The framework could be found on [GitHub](https://github.com/tmt96/dotSpace-objectSpace/).

The framework was originally designed and developed by [Shane D. McLean](https://github.com/sequenze) in collaboration with [Alberto Lluch Lafuente](https://github.com/albertolluch). The original framework could be found [here](https://github.com/pSpaces/dotSpace/).

This fork is a port of the application to support .NET Standard 2.0. With this port, the library could be used from all three popular .NET runtime: .NET Framework on Windows, and .NET Core and Mono on both Windows and *nix. Ultimately the goal is to have the library support an object space - an extension of the original tuple space idea.

The goal of the fork is to support [object space](docs/Proposal.md), an extension on the original idea of tuple space. This fork is maintained by Tuan Tran and Daishiro Nishida as part of their project for Williams College's Distributed System course.

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

Follow these links for more information on [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/get-started) and its implementations [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/index), [Mono](http://www.mono-project.com/docs/), or [.NET Framework](https://docs.microsoft.com/en-us/dotnet/framework/).

## Additional Documentation

Please go to dotSpace's [wiki page](https://github.com/pSpaces/dotSpace/wiki) for [documentation on the API](https://github.com/pSpaces/dotSpace/wiki/basics) and [dotSpace-examples' wiki page](https://github.com/pSpaces/dotSpace-Examples/wiki) for information about the examples.

## Main Extensions

### TreeSpace

* [TreeSpace](dotSpace/Objects/Space/TreeSpace.cs): Implementation of a tuple space optimized for lookups. Uses the TupleTree data structure.

* [TupleTree](dotSpace/Objects/Utility/TupleTree.cs): A data structure which stores tuples in a tree.

### ObjectSpace

#### Interface

* [IObjectSpaceSimple](dotSpace/Interfaces/ObjectSpace/IObjectSpaceSimple.cs) and [IObjectSpace](dotSpace/Interfaces/ObjectSpace/IObjectSpace.cs): Specify the interface for an ObjectSpace.

* [IObjectMessage](dotSpace/Interfaces/ObjectNetwork/IObjectMessage.cs): Base interface for all network message.

* [IObjectRepository](dotSpace/Interfaces/ObjectNetwork/IObjectRepository.cs) and [IObjectRepositorySimple](dotSpace/Interfaces/ObjectNetwork/IObjectRepositorySimple.cs): Specify the interface for a remote ObjectSpace Space repository.

#### Classes

The classes we added to the project can be found in these folders:

* [ObjectSpaceBase](dotSpace/BaseClasses/ObjectSpace/): An abstract class which supports the main operations for an ObjectSpace. We also provide base implementation of ObjectSpace agents for ease of use.

* [ObjectSpace Utilities](dotSpace/BaseClasses/ObjectUtility/): Includes classes that make up an entry in the base ObjectSpace

* [Base remote Object Message Classes](dotSpace/BaseClasses/ObjectNetwork): Base classes for distributed ObjectSpace, including base classes for network messages, a base Json encoder for network messages, and a base remote space repository.

* [SequentialObjectSpace](dotSpace/Objects/ObjectSpace/): A sample FIFO implementation of ObjectSpace.

* [Remote ObjectSpace & Object Repository](dotSpace/Objects/ObjectNetwork/): Allows a user to access an ObjectSpaceSimple remotely, including implementations of a remote Object Space, remote Space Repository, a custom Json writer, an OperationMap to handle different types of message, and all network messages in ObjectSpace.

Other additions include classes and interfaces that mimic existing ones but are adapted for the implementation of an ObjectSpace.

### Main Examples

* [ClickRate](examples/ClickRate/): A program which reads in impressions and clicks for advertisements, and computes the click-through rate for each ad.

* [ClickRateD](examples/distributed/ClickRateD/): A distributed version of the ClickRate program.

* [Fork](examples/ObjectSpaceExamples/Fork/): A version of Example2 (dining philosophers problem) which uses ObjectSpace.

* [ProducerConsumer](examples/ObjectSpaceExamples/ProducerConsumer/): An example of how subtyping and filtering may be used with ObjectSpace.

* [ProducerConsumerD](examples/ObjectSpaceExamples/ProducerConsumerD/): A distributed version of the ProducerConsumer program.