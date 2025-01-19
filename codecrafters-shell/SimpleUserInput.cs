using System.Text;

class SimpleUserInput(IEnumerable<string> buildInCommands, ExecutableDirectories executableDirectories)
{
    private const string Prompt = "$ ";

    public string Read()
    {
        var input = new StringBuilder();
        var keyInfo = new ConsoleKeyInfo(' ', ConsoleKey.None, false, false, false);
        IEnumerable<string>? suggestions = null;

        Console.Write(Prompt);

        while (true)
        {
            keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                suggestions = null;
                Console.WriteLine();
                break;
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                if (suggestions != null)
                {
                    Console.WriteLine();
                    Console.WriteLine(string.Join("  ", suggestions));
                    Console.Write(Prompt);
                    Console.Write(input.ToString());
                    suggestions = null;
                    continue;
                }

                var currentInput = input.ToString();
                var suggestion = buildInCommands.FirstOrDefault(s => s != null && s.StartsWith(currentInput));
                if (suggestion == null)
                {
                    suggestions = [.. executableDirectories.GetProgramsBeginWith(currentInput)];
                    if (suggestions.Count() == 1)
                    {
                        suggestion = suggestions.First();
                    }
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
                suggestions = null;
                input.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }
        }

        return input.ToString();
    }
}
