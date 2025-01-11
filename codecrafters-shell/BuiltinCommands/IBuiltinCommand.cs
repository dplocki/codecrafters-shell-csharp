internal interface IBuiltinCommand
{
    string Name { get; }

    int Execute(string[] args);
}
