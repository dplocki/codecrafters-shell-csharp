while(true)
{
    Console.Write("$ ");

    // Wait for user input
    var userInput = Console.ReadLine();

    var parameters = userInput.Split(' ');
    var command = parameters[0];

    if (command == "echo")
    {
        Console.WriteLine(string.Join(" ", parameters.Skip(1)));
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
