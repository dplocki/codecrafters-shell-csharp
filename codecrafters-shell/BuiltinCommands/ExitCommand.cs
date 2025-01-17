internal class ExitCommand : ICommand
{
    public string Name => "exit";

    public Task<int> Execute(TextWriter stdOut, TextWriter stdErr, IEnumerable<string> args)
    {
        int exitCode = 0;

        if (args.Count() > 1)
        {
            if (!int.TryParse(args.ElementAt(1), out exitCode))
            {
                exitCode = 0;
            }
        }

        Environment.Exit(exitCode);
        return Task.FromResult(0);
    }
}
