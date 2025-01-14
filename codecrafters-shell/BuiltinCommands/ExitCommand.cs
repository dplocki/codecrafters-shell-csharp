internal class ExitCommand : ICommand
{
    public string Name => "exit";

    public Task<int> Execute(TextWriter stdOut, string[] args)
    {
        if (args.Length > 1 && int.TryParse(args[1], out var exitCode))
        {
            Environment.Exit(exitCode);
            return Task.FromResult(exitCode);
        }

        return Task.FromResult(0);
    }
}
