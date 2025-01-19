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
                var currentInput = input.ToString();
                if (suggestions != null)
                {
                    Console.WriteLine();
                    Console.WriteLine(string.Join("  ", suggestions));
                    Console.Write(Prompt);
                    Console.Write(currentInput);
                    suggestions = null;
                    continue;
                }

                var suggestion = buildInCommands.FirstOrDefault(s => s != null && s.StartsWith(currentInput));
                if (suggestion == null)
                {
                    suggestions = [.. executableDirectories.GetProgramsBeginWith(currentInput)];
                    if (suggestions.Count() == 1)
                    {
                        suggestion = suggestions.First();
                    }
                    else
                    {
                        suggestion = GetCommonPartInSuggestions(currentInput, suggestions);
                        input.Append(suggestion);
                        Console.Write(suggestion);
                        continue;
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

    private string? GetCommonPartInSuggestions(string currentInput, IEnumerable<string> suggestions)
    {
        var result = new StringBuilder();
        var index = currentInput.Length;

        while (true)
        {
            var letters = suggestions
                .Select(suggestion => {
                    if (index >= suggestion.Length)
                    {
                        return (char?)null;
                    }

                    return suggestion[index];
                })
                .Distinct()
                .ToArray();

            if (letters.Length == 1 && letters.First() != null)
            {
                result.Append(letters.First());
            }
            else
            {
                return result.ToString();
            }

            index++;
        }
    }
}
