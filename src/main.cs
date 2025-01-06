using System.Diagnostics;

var executableDirectories = new ExecutableDirectories(Environment.GetEnvironmentVariable("PATH") ?? "");

while (true)
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
        foreach (var programName in parameters.Skip(1))
        {
            if (programName == "type" || programName == "echo" || programName == "exit")
            {
                Console.WriteLine($"{programName} is a shell builtin");
            }
            else
            {
                var programPath = executableDirectories.GetProgramPath(programName);
                if (programPath != null)
                {
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

    var executablePath = executableDirectories.GetProgramPath(command);
    if (executablePath != null)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = executablePath,
            Arguments = string.Join(" ", parameters.Skip(1)),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processInfo;
        process.OutputDataReceived += (sender, args) =>
        {
            if (args.Data != null)
            {
                Console.WriteLine(args.Data);
            }
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (args.Data != null)
            {
                Console.Error.WriteLine(args.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();
        continue;
    }

    Console.WriteLine($"{command}: command not found");
}
