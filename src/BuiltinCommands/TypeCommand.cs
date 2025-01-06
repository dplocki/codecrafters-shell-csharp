internal class TypeCommand(IDictionary<string, IBuiltinCommand> buildinCommandMap, ExecutableDirectories executableDirectories) : IBuiltinCommand
{
    private ExecutableDirectories executableDirectories = executableDirectories;

    public string Name => "type";

    public int Execute(string[] args)
    {
        foreach (var programName in args.Skip(1))
        {
            if (programName == "type" || programName == "echo" || programName == "exit")
            {
                Console.WriteLine($"{programName} is a shell builtin");
            }
            else
            {
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
        }

        return 0;
    }
}
