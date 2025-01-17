internal class PwdCommand : ICommand
{
    public string Name => "pwd";

    public Task<int> Execute(TextWriter stdOut, TextWriter stdErr, IEnumerable<string> args)
    {
        stdOut.WriteLine(Directory.GetCurrentDirectory());

        return Task.FromResult(0);
    }
}
