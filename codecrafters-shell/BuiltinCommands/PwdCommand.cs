internal class PwdCommand : ICommand
{
    public string Name => "pwd";

    public Task<int> Execute(TextWriter stdOut, string[] args)
    {
        stdOut.WriteLine(Directory.GetCurrentDirectory());

        return Task.FromResult(0);
    }
}
