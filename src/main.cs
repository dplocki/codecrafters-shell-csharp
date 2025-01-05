while(true)
{
    Console.Write("$ ");

    // Wait for user input
    var command = Console.ReadLine();

    if (command == "exit 0")
    {
        return 0;
    }

    Console.WriteLine($"{command}: command not found");
}
