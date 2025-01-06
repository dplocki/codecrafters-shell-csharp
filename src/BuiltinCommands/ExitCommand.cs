internal class ExitCommand : IBuiltinCommand
{
    public string Name => "exit";

    public int Execute(string[] args)
    {
        if (args.Length > 1 && int.TryParse(args[1], out var exitCode))
        {
            return exitCode;
        }
        else
        {
            return 0;
        }
    }
}
