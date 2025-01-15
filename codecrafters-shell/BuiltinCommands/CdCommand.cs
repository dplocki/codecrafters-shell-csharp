internal class CdCommand : ICommand
{
    public string Name => "cd";

    public Task<int> Execute(TextWriter stdOut, TextWriter stdErr, string[] args)
    {
        var path = args[1];

        if (path == "~")
        {
            Directory.SetCurrentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            return Task.FromResult(0);
        }

        if (Directory.Exists(path))
        {
            Directory.SetCurrentDirectory(path);
            return Task.FromResult(0);
        }

        stdErr.WriteLine($"{Name}: {path}: No such file or directory");
        return Task.FromResult(1);
    }
}
