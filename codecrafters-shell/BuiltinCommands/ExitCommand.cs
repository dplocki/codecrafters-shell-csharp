internal class ExitCommand : ICommand
{
    public const string CommandName = "exit";

    public string Name => CommandName;

    public Task<int> Execute(TextWriter stdOut, string[] args)
    {
        if (args.Length > 1 && int.TryParse(args[1], out var exitCode))
        {
            return Task.FromResult(exitCode);
        }

        return Task.FromResult(0);
    }
}
