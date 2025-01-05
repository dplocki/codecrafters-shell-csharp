while(true)
{
    Console.Write("$ ");

    // Wait for user input
    var userInput = Console.ReadLine();

    var parameters = userInput?.Split(' ') ?? Array.Empty<string>();
    var command = parameters.FirstOrDefault("");

    if (command == "echo")
    {
        Console.WriteLine(string.Join(" ", parameters.Skip(1)));
    }
    else if (command == "type")
    {
        foreach(var programName in parameters.Skip(1))
        {
            if (programName == "type" || programName == "echo")
            {
                Console.WriteLine($"{programName} is a shell builtin");
            }
            else
            {
                Console.WriteLine($"{command}: command not found");
            }
        }
    }
    else if (command == "exit")
    {
        if (int.TryParse(parameters[1], out var exitCode))
        {
            return exitCode;
        }
        else
        {
            return 1;
        }
    }
    else
    {
        Console.WriteLine($"{command}: command not found");
    }
}
