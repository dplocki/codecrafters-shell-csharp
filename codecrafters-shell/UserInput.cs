using System.Text;

class UserInput
{
    private const string Prompt = "$ ";

    public static string Read()
    {
        var input = new StringBuilder();
        int cursorPosition = 0;
        Console.Write(Prompt);

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            else if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (cursorPosition > 0)
                {
                    input.Remove(cursorPosition - 1, 1);
                    cursorPosition--;

                    RedrawInput(input.ToString(), cursorPosition);
                }
            }
            else if (keyInfo.Key == ConsoleKey.Delete)
            {
                if (cursorPosition < input.Length)
                {
                    input.Remove(cursorPosition, 1);

                    RedrawInput(input.ToString(), cursorPosition);
                }
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                if (cursorPosition > 0)
                {
                    cursorPosition--;
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                if (cursorPosition < input.Length)
                {
                    cursorPosition++;
                    Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                }
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                string suggestion = "AutoCompleteText";
                input.Append(suggestion);
                cursorPosition = input.Length;

                RedrawInput(input.ToString(), cursorPosition);
            }
            else
            {
                input.Insert(cursorPosition, keyInfo.KeyChar);
                cursorPosition++;

                RedrawInput(input.ToString(), cursorPosition);
            }
        }

        return input.ToString();
    }

    static void RedrawInput(string input, int cursorPosition)
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));

        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(Prompt);
        Console.Write(input);
        Console.SetCursorPosition(cursorPosition + Prompt.Length, Console.CursorTop);
    }
}
