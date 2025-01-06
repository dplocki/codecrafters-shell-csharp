internal class CdCommand : IBuiltinCommand
{
    public string Name => "cd";

    public int Execute(string[] args)
    {
        var path = args[1];

        if (Directory.Exists(path))
        {
            Directory.SetCurrentDirectory(path);
            return 0;
        }

        Console.WriteLine($"{Name}: {path}: No such file or directory");
        return 1;
    }
}
