internal class EchoCommand : ICommand
{
    public string Name => "echo";

    public int Execute(TextWriter stdOut, string[] args)
    {
        stdOut.WriteLine(string.Join(" ", args.Skip(1)));

        return 0;
    }
}
