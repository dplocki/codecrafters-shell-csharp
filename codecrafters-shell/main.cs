[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("codecrafters-shell-tests")]

var parser = new CommandLineParser();
var executableDirectories = new ExecutableDirectories(Environment.GetEnvironmentVariable("PATH") ?? "");
var builtinCommandsMap = new Dictionary<string, ICommand>();
var runExecutable = new RunExecutableCommand();
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

    var stdOut = parser.StdOut == null
        ? Console.Out
        : new StreamWriter(parser.StdOut, parser.StdOutAppend);

    var stdErr = parser.StdErr == null
        ? Console.Error
        : new StreamWriter(parser.StdErr, parser.StdErrAppend);

    var command = parameters.FirstOrDefault("");
    if (builtinCommandsMap.TryGetValue(command, out var builtinCommand))
    {
        await builtinCommand.Execute(stdOut, stdErr, parameters);
    }
    else
    {
        var executablePath = executableDirectories.GetProgramPath(command);
        if (executablePath != null)
        {
            await runExecutable.Execute(stdOut, stdErr, parameters);
        }
        else
        {
            stdErr.WriteLine($"{command}: command not found");
        }
    }

    stdErr.Flush();
    stdErr.Close();
    stdErr.Dispose();

    stdOut.Flush();
    stdOut.Close();
    stdOut.Dispose();
}
