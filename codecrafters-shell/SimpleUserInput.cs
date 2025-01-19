using System.Text;

class SimpleUserInput(IEnumerable<string> buildInCommands, ExecutableDirectories executableDirectories)
{
    private const string Prompt = "$ ";

    public IEnumerable<string> BuildInCommands { get; } = buildInCommands;

    public string Read()
    {
        var input = new StringBuilder();
        var keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.None, false, false, false);

        Console.Write(Prompt);

        while (true)
        {
            keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                var currentInput = input.ToString();
                var suggestion = BuildInCommands.FirstOrDefault(s => s != null && s.StartsWith(currentInput));
                if (suggestion == null)
                {
                    suggestion = executableDirectories.GetProgramsBeginWith(currentInput).FirstOrDefault();
                }
                if (suggestion == null)
                {
                    Console.Write('\a');
                    continue;
                }

                var autoCompleat = suggestion[input.Length..] + ' ';
                input.Append(autoCompleat);
                Console.Write(autoCompleat);
                continue;
            }
            else
            {
                input.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }
        }

        return input.ToString();
    }
}
