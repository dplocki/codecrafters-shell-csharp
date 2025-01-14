internal class EchoCommand : ICommand
{
    public string Name => "echo";

    public Task<int> Execute(TextWriter stdOut, string[] args)
    {
        stdOut.WriteLine(string.Join(" ", args.Skip(1)));

        return Task.FromResult(0);
    }
}
