internal class CommandLineParser
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    private Action<char> parserMode;
    private IList<char> currentToken;
    private IList<string> tokens;
    private int index;

#pragma warning restore CS8618

    public IEnumerable<string> Parse(string commandLine)
    {
        currentToken = [];
        tokens = [];
        parserMode = StartParsing;
        index = 0;

        while(index < commandLine.Length)
        {
            parserMode(commandLine[index]);
        }

        AddCurrentToken();
        return tokens;
    }

    private void StartParsing(char character)
    {
        parserMode = char.IsWhiteSpace(character)
            ? BreakBetweenTokens
            : SimpleToken;
    }

    private void SimpleToken(char character)
    {
        if (char.IsWhiteSpace(character))
        {
            parserMode = BreakBetweenTokens;
            return;
        }

        if (character == '\'')
        {
            parserMode = SingleQuoteToken;
        }
        else if (character == '"')
        {
            parserMode = DoubleQuoteToken;
        }
        else
        {
            currentToken.Add(character);
        }

        index++;
    }

    private void SingleQuoteToken(char character)
    {
        if (character == '\'')
        {
            parserMode = SimpleToken;
        }
        else
        {
            currentToken.Add(character);
        }

        index++;
    }

    private void DoubleQuoteToken(char character)
    {
        if (character == '"')
        {
            parserMode = SimpleToken;
        }
        else
        {
            currentToken.Add(character);
        }

        index++;
    }

    private void BreakBetweenTokens(char character)
    {
        AddCurrentToken();

        if (char.IsWhiteSpace(character))
        {
            index++;
            return;
        }

        parserMode = SimpleToken;
    }

    private void AddCurrentToken()
    {
        if (currentToken.Count == 0)
        {
            return;
        }

        tokens.Add(new string([.. currentToken]));
        currentToken = [];
    }
}
