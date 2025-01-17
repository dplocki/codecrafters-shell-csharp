internal interface ICommand
{
    string Name { get; }

    Task<int> Execute(TextWriter stdOut, TextWriter stdErr, IEnumerable<string> args);
}
