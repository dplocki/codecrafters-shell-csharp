internal class PwdCommand : IBuiltinCommand
{
    public string Name => "pwd";

    public int Execute(string[] args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());

        return 0;
    }
}
