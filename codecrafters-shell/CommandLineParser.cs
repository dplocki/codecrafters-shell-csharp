internal class CommandLineParser
{
    private const char UserInputEnd = '\0';

    public string? StdOut { get; private set; }
    public bool StdOutAppend { get; private set; }
    public string? StdErr { get; private set; }
    public bool StdErrAppend { get; internal set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    private Action<char> parserMode;
    private IList<char> currentToken;
    private IList<string> tokens;
    private int index;
#pragma warning restore CS8618

    public IEnumerable<string> Parse(string commandLine)
    {
        StdErr = null;
        StdOut = null;
        StdErrAppend = false;
        StdOutAppend = false;
        currentToken = [];
        tokens = [];
        parserMode = StartParsing;
        index = 0;

        while(index < commandLine.Length)
        {
            parserMode(commandLine[index]);
        }

        parserMode(UserInputEnd);

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
        else if (character == '\\')
        {
            parserMode = NonQuotedBackslash;
        }
        else if (character == '"')
        {
            parserMode = DoubleQuoteToken;
        }
        else if (character == '>')
        {
            if (!currentToken.All(char.IsDigit))
            {
                AddCurrentToken();
            }

            parserMode = StreamRedirectionSignToken;
            return;
        }
        else if (character == UserInputEnd)
        {
            AddCurrentToken();
            return;
        }
        else
        {
            currentToken.Add(character);
        }

        index++;
    }

    private void StreamRedirectionSignToken(char character)
    {
        if (char.IsWhiteSpace(character))
        {
            currentToken.Add(character);
            parserMode = StreamRedirectionBreakToken;
        }
        else if (character != '>')
        {
            parserMode = StreamRedirectionLocationToken;
        }
        else if (character == UserInputEnd)
        {
            throw new InvalidOperationException("Invalid input");
        }
        else
        {
            currentToken.Add(character);
            index++;
        }
    }

    private void StreamRedirectionBreakToken(char character)
    {
        if (!char.IsWhiteSpace(character))
        {
            parserMode = StreamRedirectionLocationToken;
        }
        else if (character == UserInputEnd)
        {
            throw new InvalidOperationException("Invalid input");
        }
        else
        {
            currentToken.Add(character);
            index++;
        }
    }

    private void StreamRedirectionLocationToken(char character)
    {
        if (char.IsWhiteSpace(character) || character == UserInputEnd)
        {
            var redirectionStreamToken = new string([..currentToken]);
            var append = redirectionStreamToken.Contains(">>");
            var streamType = redirectionStreamToken[..redirectionStreamToken.IndexOf('>')].Trim();
            var streamLocation = redirectionStreamToken[(redirectionStreamToken.LastIndexOf('>') + 1)..].Trim();

            if (streamType == "2")
            {
                StdErr = streamLocation;
                StdErrAppend = append;
            }
            else if (string.IsNullOrEmpty(streamType) || streamType == "1")
            {
                StdOut = streamLocation;
                StdOutAppend = append;
            }

            currentToken = [];
            parserMode = SimpleToken;
        }
        else
        {
            currentToken.Add(character);
            index++;
        }
    }

    private void NonQuotedBackslash(char character)
    {
        if (character == UserInputEnd)
        {
            throw new InvalidOperationException("Invalid input");
        }

        currentToken.Add(character);
        index++;
        parserMode = SimpleToken;
    }

    private void DoubleQuotedBackslash(char character)
    {
        if (character == UserInputEnd)
        {
            throw new InvalidOperationException("Invalid input");
        }

        if (character != '\\' && character != '"' && character != '$' && character != '\n')
        {
            currentToken.Add('\\');
        }

        currentToken.Add(character);
        index++;
        parserMode = DoubleQuoteToken;
    }

    private void SingleQuoteToken(char character)
    {
        if (character == '\'')
        {
            parserMode = SimpleToken;
        }
        else if (character == UserInputEnd)
        {
            throw new InvalidOperationException("Invalid input");
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
        else if (character == '\\')
        {
            parserMode = DoubleQuotedBackslash;
        }
        else if (character == UserInputEnd)
        {
            throw new InvalidOperationException("Invalid input");
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
        else if (character == UserInputEnd)
        {
            AddCurrentToken();
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
