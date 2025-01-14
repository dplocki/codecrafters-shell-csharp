internal interface ICommand
{
    string Name { get; }

    int Execute(TextWriter stdOut, string[] args);
}
