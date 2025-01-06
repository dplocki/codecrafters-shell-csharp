internal class ExitCommand : IBuiltinCommand
{
    public const string CommandName = "exit";

    public string Name => CommandName;

    public int Execute(string[] args)
    {
        if (args.Length > 1 && int.TryParse(args[1], out var exitCode))
        {
            return exitCode;
        }

        return 0;
    }
}
