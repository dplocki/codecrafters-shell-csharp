internal class TypeCommand(IDictionary<string, ICommand> builtinCommandMap, ExecutableDirectories executableDirectories) : ICommand
{
    private ExecutableDirectories executableDirectories = executableDirectories;

    public string Name => "type";

    public Task<int> Execute(TextWriter stdOut, string[] args)
    {
        foreach (var programName in args.Skip(1))
        {
            if (builtinCommandMap.ContainsKey(programName))
            {
                stdOut.WriteLine($"{programName} is a shell builtin");
                continue;
            }

            var programPath = executableDirectories.GetProgramPath(programName);
            if (programPath != null)
            {
                stdOut.WriteLine($"{programName} is {programPath}");
            }
            else
            {
                stdOut.WriteLine($"{programName}: not found");
            }
        }

        return Task.FromResult(0);
    }
}
