namespace PracticalFuncyness.ConsoleClient.Common.Models;

internal abstract class FailureBase
{
    // Add what ever properties you need to represent the failure. The one I use in production >> https://github.com/code-dispenser/Flow/blob/main/Source/Flow.Core/Areas/Returns/Failures.cs
    public string Message { get; }
    protected FailureBase(string message) => Message = message;

    public static NoFailure CreateNoFailure() => new();

    public sealed class NoFailure()                           : FailureBase("No Failure") { }
    public sealed class ApplicationFailure(string message)    : FailureBase(message) { }
    public sealed class FileProcessingFailure(string message) : FailureBase(message) { }

    public sealed class ValidationFailure(IEnumerable<ValidationEntry> failures, string summary) : FailureBase(summary)
    {
        public IEnumerable<ValidationEntry> Failures => failures;
    }
}
