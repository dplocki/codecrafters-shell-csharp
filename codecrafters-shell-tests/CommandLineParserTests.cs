public class CommandLineParserTests
{
    [Theory]
    [InlineData("abc bcd", new string[] { "abc", "bcd" })]
    [InlineData("1234 13243", new string[] { "1234", "13243" })]
    [InlineData("1234          132    43", new string[] { "1234", "132", "43" })]
    [InlineData("     1234          132    43", new string[] { "1234", "132", "43" })]
    [InlineData("1234 sddssd 13243", new string[] { "1234", "sddssd", "13243" })]
    [InlineData("1234 'sdd ssd' 13243", new string[] { "1234", "sdd ssd", "13243" })]
    [InlineData("1234 'abc''def' 13243", new string[] { "1234", "abcdef", "13243" })]
    [InlineData("1234 \"sdd ssd\" 13243", new string[] { "1234", "sdd ssd", "13243" })]
    [InlineData("1234 'sdd '\"ssd\" 13243", new string[] { "1234", "sdd ssd", "13243" })]
    [InlineData("1234 'sdd '\"ssd\" \"    \" 13243", new string[] { "1234", "sdd ssd", "    ", "13243" })]
    [InlineData("world\\ \\ \\ \\ \\ \\ script", new string[] { "world      script" })]
    [InlineData("1234 sdd\\ ssd 13243", new string[] { "1234", "sdd ssd", "13243" })]
    [InlineData("\"123456789\"", new string[] { "123456789" })]
    [InlineData("\"123456789", new string[] { "123456789" })]
    public void Should_Split_Input_To_Tokens(string rawInput, string[] expectedTokens)
    {
        // Arrange
        var parser = new CommandLineParser();

        // Act
        var result = parser.Parse(rawInput);

        // Assert
        Assert.Equal(expectedTokens, result);
    }
}
