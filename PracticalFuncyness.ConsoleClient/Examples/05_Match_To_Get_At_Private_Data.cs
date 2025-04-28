using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Monads;
using PracticalFuncyness.ConsoleClient.Common.Utils;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal static class Match_To_Get_At_Private_Data
{
    public static async Task Run()
    {
        Console.WriteLine(GeneralUtils.CreateHeading("05 - Match function example"));

        Result<int> numberInResult      = 42;
        Result<int> failedNumberResult  = new FailureBase.ApplicationFailure("Failed to get the number");


        numberInResult.Match(failure     => Console.WriteLine($"Failure: {failure.Message}"), success => Console.WriteLine($"Success: {success}"));
        failedNumberResult.Match(failure => Console.WriteLine($"Failure: {failure.Message}\r\n"), success => Console.WriteLine($"Success: {success}"));

        var useDefaultIfFailed           = failedNumberResult.Match(failure => 10,success => success);

        await Console.Out.WriteLineAsync($"The default number value is: {useDefaultIfFailed}\r\n");

    }
}
