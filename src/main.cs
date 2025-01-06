string[] pathToSources = (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
    .Split([';', ':'], StringSplitOptions.RemoveEmptyEntries)
    .Where(Directory.Exists)
    .ToArray();

while(true)
{
    Console.Write("$ ");

    var userInput = Console.ReadLine();
    var parameters = userInput?.Trim().Split(' ') ?? [];
    var command = parameters.FirstOrDefault("");

    if (command == "echo")
    {
        Console.WriteLine(string.Join(" ", parameters.Skip(1)));
        continue;
    }

    if (command == "type")
    {
        foreach(var programName in parameters.Skip(1))
        {
            if (programName == "type" || programName == "echo" || programName == "exit")
            {
                Console.WriteLine($"{programName} is a shell builtin");
            }
            else
            {
                var path = pathToSources.FirstOrDefault(path => File.Exists(Path.Combine(path, programName)));
                if (path != null)
                {
                    var programPath = Path.Combine(path, programName);

                    Console.WriteLine($"{programName} is {programPath}");
                }
                else
                {
                    Console.WriteLine($"{programName}: not found");
                }
            }
        }

        continue;
    }

    if (command == "exit")
    {
        if (parameters.Length > 1 && int.TryParse(parameters[1], out var exitCode))
        {
            return exitCode;
        }
        else
        {
            return 0;
        }
    }

    Console.WriteLine($"{command}: command not found");
}
