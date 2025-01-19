internal class ExecutableDirectories(string pathVariableContent)
{
    private readonly string[] sourcesPaths = [.. pathVariableContent
            .Split([';', ':'], StringSplitOptions.RemoveEmptyEntries)
            .Where(Directory.Exists)];

    public string? GetProgramPath(string programName)
    {
        return sourcesPaths.Select(path => Path.Combine(path, programName)).FirstOrDefault(File.Exists);
    }

    public IEnumerable<string> GetProgramsBeginWith(string startsWith)
    {
        var wildcardPattern = startsWith + "*";

        return from sourcesPath in sourcesPaths
               from name in Directory.GetFiles(sourcesPath, wildcardPattern)
               select Path.GetFileName(name);
    }
}
