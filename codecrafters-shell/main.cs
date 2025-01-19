[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("codecrafters-shell-tests")]

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

var userInput = new SimpleUserInput(builtinCommandsMap.Keys.Select((k, _) => k), executableDirectories);


while (true)
{
    var parsedUserInput = CommandLineInput.Parse(userInput.Read());

    var stdOut = parsedUserInput.StdOut == null
        ? Console.Out
        : new StreamWriter(parsedUserInput.StdOut, parsedUserInput.StdOutAppend);

    var stdErr = parsedUserInput.StdErr == null
        ? Console.Error
        : new StreamWriter(parsedUserInput.StdErr, parsedUserInput.StdErrAppend);

    if (builtinCommandsMap.TryGetValue(parsedUserInput.Command, out var builtinCommand))
    {
        await builtinCommand.Execute(stdOut, stdErr, parsedUserInput.Args);
    }
    else
    {
        var executablePath = executableDirectories.GetProgramPath(parsedUserInput.Command);
        if (executablePath != null)
        {
            await runExecutable.Execute(stdOut, stdErr, parsedUserInput.Args);
        }
        else
        {
            stdErr.WriteLine($"{parsedUserInput.Command}: command not found");
        }
    }

    stdErr.Flush();
    stdErr.Close();
    stdErr.Dispose();

    stdOut.Flush();
    stdOut.Close();
    stdOut.Dispose();
}
