using System.Diagnostics;

internal class RunExecutableCommand : ICommand
{
    public string Name => "RunExecutable";

    async public Task<int> Execute(TextWriter stdOut, TextWriter stdErr, IEnumerable<string> args)
    {
        var command = args.ElementAt(0);
        var processInfo = new ProcessStartInfo(command, args.Skip(1))
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process();
        process.StartInfo = processInfo;

        try
        {
            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    return;
                }

                lock (stdOut)
                {
                    stdOut.WriteLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                {
                    return;
                }

                stdErr.WriteLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();
        }
        catch (Exception exception)
        {
            stdErr.WriteLine(exception.Message);
        }

        return process.ExitCode;
    }
}