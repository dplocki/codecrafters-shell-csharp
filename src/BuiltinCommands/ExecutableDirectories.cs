internal class ExecutableDirectories(string pathVariableContent)
{
    private readonly string[] sourcesPaths = pathVariableContent
            .Split([';', ':'], StringSplitOptions.RemoveEmptyEntries)
            .Where(Directory.Exists)
            .ToArray();

    public string? GetProgramPath(string programName)
    {
        return sourcesPaths.Select(path => Path.Combine(path, programName)).FirstOrDefault(File.Exists);
    }
}
