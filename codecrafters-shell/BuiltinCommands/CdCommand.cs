internal class CdCommand : ICommand
{
    public string Name => "cd";

    public int Execute(TextWriter stdOut, string[] args)
    {
        var path = args[1];

        if (path == "~")
        {
            Directory.SetCurrentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            return 0;
        }

        if (Directory.Exists(path))
        {
            Directory.SetCurrentDirectory(path);
            return 0;
        }

        stdOut.WriteLine($"{Name}: {path}: No such file or directory");
        return 1;
    }
}
