[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("codecrafters-shell-tests")]

var parser = new CommandLineParser();
var executableDirectories = new ExecutableDirectories(Environment.GetEnvironmentVariable("PATH") ?? "");
var builtinCommandsMap = new Dictionary<string, ICommand>();
var runExecutable = new RunExecutableCommand(executableDirectories);
var builtinCommands = new List<ICommand>()
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

    var userInput = Console.ReadLine() ?? string.Empty;
    var parameters = parser.Parse(userInput).ToArray();
    var fileWriter = Console.Out;

    var command = parameters.FirstOrDefault("");
    if (builtinCommandsMap.TryGetValue(command, out var builtinCommand))
    {
        await builtinCommand.Execute(fileWriter, parameters);
        continue;
    }

    var executablePath = executableDirectories.GetProgramPath(command);
    if (executablePath != null)
    {
        await runExecutable.Execute(fileWriter, parameters);
        continue;
    }

    Console.WriteLine($"{command}: command not found");
}
