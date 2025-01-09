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

        EndParsing();
        return tokens;
    }

    private void StartParsing(char character)
    {
        if (char.IsWhiteSpace(character))
        {
            parserMode = WhiteCharacterParameter;
            return;
        }

        parserMode = NonWhiteCharacterParameter;
        return;
    }

    private void NonWhiteCharacterParameter(char character)
    {
        if (char.IsWhiteSpace(character))
        {
            parserMode = WhiteCharacterParameter;
            return;
        }

        if (character == '\'')
        {
            parserMode = SingleQuoteParameter;
            index++;
            return;
        }


        index++;
        currentToken.Add(character);
        return;
    }

    private void SingleQuoteParameter(char character)
    {
        if (character == '\'')
        {
            index++;
            parserMode = NonWhiteCharacterParameter;
            return;
        }

        currentToken.Add(character);
        index++;
    }

    private void WhiteCharacterParameter(char character)
    {
        if (currentToken.Count > 0)
        {
            tokens.Add(new string([.. currentToken]));
            currentToken = [];
        }

        if (!char.IsWhiteSpace(character))
        {
            parserMode = NonWhiteCharacterParameter;
            return;
        }

        index++;
        return;
    }

    private void EndParsing()
    {
        if (currentToken.Count > 0)
        {
            tokens.Add(new string([.. currentToken]));
            currentToken = [];
        }
    }
}
