﻿public class CommandLineParserTests
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
    public void ShouldSplitInputToTokens(string rawInput, string[] expectedTokens)
    {
        // Act & Arrange
        var result = CommandLineInput.Parse(rawInput);

        // Assert
        Assert.Null(result.StdOut);
        Assert.False(result.StdOutAppend);
        Assert.Equal(expectedTokens, result.Args);
    }

    [Theory]
    [InlineData("1>abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData(">abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("> abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("1> abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("cde >abc >bcd", new string[] { "cde" }, "bcd")]
    [InlineData("abc1>bcd cde", new string[] { "abc1", "cde" }, "bcd")]
    public void ShouldRecognizeStdOutStreamRedirect(string rawInput, string[] expectedTokens, string expectedStdStream)
    {
        // Act & Arrange
        var result = CommandLineInput.Parse(rawInput);

        // Assert
        Assert.False(result.StdOutAppend);
        Assert.Equal(expectedTokens, result.Args);
        Assert.Equal(expectedStdStream, result.StdOut);
    }

    [Theory]
    [InlineData("2>abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("2> abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("cde 2>abc >bcd", new string[] { "cde" }, "abc")]
    [InlineData("cde >abc 2>bcd", new string[] { "cde" }, "bcd")]
    [InlineData("abc 2>bcd", new string[] { "abc" }, "bcd")]

    public void ShouldRecognizeStdErrStreamRedirect(string rawInput, string[] expectedTokens, string expectedStdStream)
    {
        // Act & Arrange
        var result = CommandLineInput.Parse(rawInput);

        // Assert
        Assert.False(result.StdOutAppend);
        Assert.Equal(expectedTokens, result.Args);
        Assert.Equal(expectedStdStream, result.StdErr);
    }

    [Theory]
    [InlineData("1>>abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData(">>abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData(">> abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("1>> abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("cde   >>abc    >>bcd", new string[] { "cde" }, "bcd")]
    [InlineData("abc1>>bcd cde", new string[] { "abc1", "cde" }, "bcd")]
    public void ShouldRecognizeStdOutStreamAppendRedirect(string rawInput, string[] expectedTokens, string expectedStdStream)
    {
        // Act & Arrange
        var result = CommandLineInput.Parse(rawInput);

        // Assert
        Assert.True(result.StdOutAppend);
        Assert.Equal(expectedTokens, result.Args);
        Assert.Equal(expectedStdStream, result.StdOut);
    }

    [Theory]
    [InlineData("2>>abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("2>> abc bcd", new string[] { "bcd" }, "abc")]
    [InlineData("cde 2>>abc >>bcd", new string[] { "cde" }, "abc")]
    [InlineData("cde >abc 2>>bcd", new string[] { "cde" }, "bcd")]
    [InlineData("abc 2>>bcd", new string[] { "abc" }, "bcd")]
    public void ShouldRecognizeStdErrStreamAppendRedirect(string rawInput, string[] expectedTokens, string expectedStdStream)
    {
        // Act & Arrange
        var result = CommandLineInput.Parse(rawInput);

        // Assert
        Assert.True(result.StdErrAppend);
        Assert.Equal(expectedTokens, result.Args);
        Assert.Equal(expectedStdStream, result.StdErr);
    }
}
