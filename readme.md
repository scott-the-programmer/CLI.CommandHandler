# CLI.CommandHandler

<a href="https://codeclimate.com/github/scott-the-programmer/CLI.CommandHandler/maintainability"><img src="https://api.codeclimate.com/v1/badges/96f8f4305388f7b0fc4a/maintainability" /></a>
<a href="https://codeclimate.com/github/scott-the-programmer/CLI.CommandHandler/test_coverage"><img src="https://api.codeclimate.com/v1/badges/96f8f4305388f7b0fc4a/test_coverage" /></a>

This is currently a WIP

## Intent

This project aims to automatically map cli command models to corresponding command handlers. The overall goal is to eliminate manual command line arg parsing, and allow developers to focus on the logic.

At its current state, this project is highly coupled to https://github.com/commandlineparser/commandline

## Example

### Command Model

```csharp
    [Verb("make-noise")]
    public class MakeNoise : ICommand
    {
        [Option('n', "noise", Required = true, HelpText = "Noise to emit")]
        public string Noise { get; set; }
    }
```

### Command Handler

```csharp
    public class MakeNoiseCommand : ICommandHandler<MakeNoise>
    {
        public Task HandleAsync(MakeNoise command)
        {
            Console.WriteLine(command.Noise)
            return Task.CompletedTask;
        }
    }
```

### Program.cs

```csharp
    public static void Main(string[] args)
    {
        var types = LoadRunCommands(); //TODO: Define this a bit better

        Parser.Default.ParseArguments(args, types)
            .WithParsed(Run);

        Console.ReadLine();
    }

    public static void Run(object command)
    {
        var commandDispatcher = new CommandDispatcher(Assembly.GetExecutingAssembly().GetName());
        commandDispatcher.DispatchAsync(command) //Run the MakeNoise command
    }
```
