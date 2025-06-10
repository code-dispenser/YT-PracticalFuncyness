using PracticalFuncyness.ConsoleClient.Common.Extensions;
using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Utils;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class Validated_Using_Combine
{
    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("14 - Using the Validated functor using Combine (not yet an applicative functor"));

        var fullName = FullName.Create("Mr", "J", "S");

        foreach (var group in fullName.Failures.ToList().GroupBy(failure => failure.DisplayName))
        {
            Console.WriteLine($"{group.Key}");
            group.ToList().ForEach(failure => Console.WriteLine($"  - {failure.Message}"));
        }

        Console.WriteLine($"\r\nConvert to Result Type if you want to return that instead\r\n");

        var result = fullName.ToResult();

        Console.WriteLine("We can add a specialized failure type to our result type for validation failures.\r\n");

        result.OnFailure(failure => 
        {
            ((FailureBase.ValidationFailure)failure).Failures.GroupBy(failure => failure.DisplayName).ToList().ForEach(group =>
            {
                Console.WriteLine($"{group.Key}");
                group.ToList().ForEach(failure => Console.WriteLine($"  - {failure.Message}"));
            });
        });

        var validatedFullName = FullName.Create("Mr", "John", "Smith");//we know this passes otherwise we would use Match to handle both cases.

        var goodFullName      =  validatedFullName.GetValueOr(default!); 

        Console.WriteLine($"\r\n{goodFullName}");
    }
}
