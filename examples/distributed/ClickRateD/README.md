To run this project, first `cd` to the project directory.

To start the tuplespace:

```
dotnet run calc <ip address> <impression log path> <click log path> <output path>
```

To start a click entry parser:

```
dotnet run click <ip address>
```

To start an impression entry parser:

```
dotnet run imp1 <ip address>
```


(suppose you are running .NET Core 2.0 and above. Running using .NET Framework or Mono might differs)