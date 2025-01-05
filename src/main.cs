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
    else if (command == "exit 0")
    {
        return 0;
    }
    else
    {
        Console.WriteLine($"{command}: command not found");
    }
}
