internal class PwdCommand : ICommand
{
    public string Name => "pwd";

    public int Execute(TextWriter stdOut, string[] args)
    {
        stdOut.WriteLine(Directory.GetCurrentDirectory());

        return 0;
    }
}
