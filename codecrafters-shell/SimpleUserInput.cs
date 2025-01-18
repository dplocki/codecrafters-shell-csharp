using System.Text;

class SimpleUserInput(IEnumerable<string> buildInCommands)
{
    private const string Prompt = "$ ";

    public IEnumerable<string> BuildInCommands { get; } = buildInCommands;

    public string Read()
    {
        var input = new StringBuilder();
        Console.Write(Prompt);

        while (true)
        {
            var keyInfo = Console.ReadKey(intercept: true);
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                var suggestion = BuildInCommands.FirstOrDefault(s => s.StartsWith(input.ToString()));
                if (suggestion != null)
                {
                    var autoCompleat = suggestion[input.Length..] + ' ';
                    input.Append(autoCompleat);
                    Console.Write(autoCompleat);
                }
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
