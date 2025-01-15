internal class ExitCommand : ICommand
{
    public string Name => "exit";

    public Task<int> Execute(TextWriter stdOut, string[] args)
    {
        int exitCode = 0;

        if (args.Length > 1)
        {
            if (!int.TryParse(args[1], out exitCode))
            {
                exitCode = 0;
            }
        }

        Environment.Exit(exitCode);
        return Task.FromResult(0);
    }
}
