using System.Diagnostics;

var executableDirectories = new ExecutableDirectories(Environment.GetEnvironmentVariable("PATH") ?? "");
var builtinCommandsMap = new Dictionary<string, IBuiltinCommand>();
var builtinCommands = new List<IBuiltinCommand>()
{
    new EchoCommand(),
    new ExitCommand(),
    new TypeCommand(builtinCommandsMap, executableDirectories),
    new PwdCommand(),
    new CdCommand(),
};

foreach (var item in builtinCommands)
{
    builtinCommandsMap[item.Name] = item;
}

while (true)
{
    Console.Write("$ ");

    var userInput = Console.ReadLine();
    var parameters = userInput?.Trim().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) ?? [];
    var command = parameters.FirstOrDefault("");

    if (command == ExitCommand.CommandName)
    {
        return builtinCommandsMap[command].Execute(parameters);
    }

    if (builtinCommandsMap.TryGetValue(command, out var builtinCommand))
    {
        builtinCommand.Execute(parameters);
        continue;
    }

    var executablePath = executableDirectories.GetProgramPath(command);
    if (executablePath != null)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = executablePath,
            Arguments = string.Join(" ", parameters.Skip(1)),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processInfo;
        process.OutputDataReceived += (sender, args) =>
        {
            if (args.Data != null)
            {
                Console.WriteLine(args.Data);
            }
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (args.Data != null)
            {
                Console.Error.WriteLine(args.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();
        continue;
    }

    Console.WriteLine($"{command}: command not found");
}
