internal class TypeCommand(IDictionary<string, IBuiltinCommand> builtinCommandMap, ExecutableDirectories executableDirectories) : IBuiltinCommand
{
    private ExecutableDirectories executableDirectories = executableDirectories;

    public string Name => "type";

    public int Execute(string[] args)
    {
        foreach (var programName in args.Skip(1))
        {
            if (builtinCommandMap.ContainsKey(programName))
            {
                Console.WriteLine($"{programName} is a shell builtin");
                continue;
            }

            var programPath = executableDirectories.GetProgramPath(programName);
            if (programPath != null)
            {
                Console.WriteLine($"{programName} is {programPath}");
            }
            else
            {
                Console.WriteLine($"{programName}: not found");
            }
        }

        return 0;
    }
}
