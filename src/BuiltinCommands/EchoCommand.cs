internal class EchoCommand : IBuiltinCommand
{
    public string Name => "echo";

    public int Execute(string[] args)
    {
        Console.WriteLine(string.Join(" ", args.Skip(1)));

        return 0;
    }
}
