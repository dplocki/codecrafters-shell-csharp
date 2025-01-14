internal interface ICommand
{
    string Name { get; }

    Task<int> Execute(TextWriter stdOut, string[] args);
}
