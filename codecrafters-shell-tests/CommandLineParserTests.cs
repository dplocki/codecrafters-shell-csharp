public class CommandLineParserTests
{
    [Theory]
    [InlineData("abc bcd", new string[] { "abc", "bcd" })]
    [InlineData("1234 13243", new string[] { "1234", "13243" })]
    [InlineData("1234          132    43", new string[] { "1234", "132", "43" })]
    [InlineData("     1234          132    43", new string[] { "1234", "132", "43" })]
    [InlineData("1234 abcd 13243", new string[] { "1234", "abcd", "13243" })]
    [InlineData("1234 'sdd ssd' 13243", new string[] { "1234", "sdd ssd", "13243" })]
    [InlineData("1234 'abc''def' 13243", new string[] { "1234", "abcdef", "13243" })]
    [InlineData("1234 \"sdd ssd\" 13243", new string[] { "1234", "sdd ssd", "13243" })]
    [InlineData("1234 'sdd '\"ssd\" 13243", new string[] { "1234", "sdd ssd", "13243" })]
    [InlineData("1234 'sdd '\"ssd\" \"    \" 13243", new string[] { "1234", "sdd ssd", "    ", "13243" })]
    [InlineData("world\\ \\ \\ \\ \\ \\ script", new string[] { "world      script" })]
    [InlineData("1234 sdd\\ ssd 13243", new string[] { "1234", "sdd ssd", "13243" })]
    [InlineData("\"123456789\"", new string[] { "123456789" })]
    [InlineData("\"123456789", new string[] { "123456789" })]
    public void ShouldSplitInputToTokens(string rawInput, string[] expectedTokens)
    {
        // Arrange
        var parser = new CommandLineParser();

        // Act
        var result = parser.Parse(rawInput);

        // Assert
        Assert.Equal(expectedTokens, result);
    }

    [Theory]
    [InlineData("1>abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData(">abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("> abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("1> abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("cde >abc >bcd", new string[] { "cde" }, "bcd")]
    [InlineData("abc1>bcd cde", new string[] { "abc1", "cde" }, "bcd")]
    public void ShouldRecognizeStdStreamRedirect(string rawInput, string[] expectedTokens, string expectedStdStream)
    {
        // Arrange
        var parser = new CommandLineParser();

        // Act
        var result = parser.Parse(rawInput);

        // Assert
        Assert.Equal(expectedTokens, result);
        Assert.Equal(expectedStdStream, parser.StdOut);
    }
}
