using PracticalFuncyness.ConsoleClient.Common.Functors;
using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Seeds;
using PracticalFuncyness.ConsoleClient.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class Validated_Using_Classical_Apply
{
    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("15 - Validated using Applicative Functor"));

        AddRules();

        var fullName = FullName.ApplicativeCreate("Mr", "J", "S");

        foreach (var group in fullName.Failures.ToList().GroupBy(failure => failure.DisplayName))
        {
            Console.WriteLine($"{group.Key}");
            group.ToList().ForEach(failure => Console.WriteLine($"  - {failure.Message}"));
        }

        var validatedFullName = FullName.ApplicativeCreate("Mr", "John", "Smith");//we know this passes otherwise we would use Match to handle both cases.

        var goodFullName = validatedFullName.GetValueOr(default!);

        Console.WriteLine($"\r\n{goodFullName}");
    }
      

    private static void AddRules()//May be patterns and failure messages from resource/database
    {
        /*
            * Ok a little mind bending. You could if you wanted to get the patterns and failure messages from a resource or database so things are dynamic and use currying and partial application to fill in the gaps
            * The caller asks for a rule saying which field/display its for and gets back a function from cache which then fills in the missing pieces returning a function that just needs the input value.
            * One rule could be used for the same thing but with fields that have different names or display names.
        */

        (string pattern, string message) salutationInfo    = ("^(Mr|Mrs|Ms|Dr|Prof)$", "Must be one of Mr, Mrs, Ms, Dr, Prof");
        (string pattern, string message) capitalLetterInfo = (@"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "Must start with a capital letter");
        (string pattern, string message) generalNameInfo   = (@"^(?=.{2,50}$)[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]+$", "Must start with a capital letter and be between 2 and 50 characters in length");

        var lengthMessage = "Must must be between 2 and 50 characters long.";

        Func<string, string, Func<string, Validated<string>>> salutationRule  = (fieldName, displayName) => ValidationFunctions.CreateRegexRule<string>(salutationInfo.pattern, fieldName, displayName, salutationInfo.message);

        Func<string, string, Func<string, Validated<string>>> generalNameRule = (fieldName, displayName) => ValidationFunctions.CreateRegexRule<string>(generalNameInfo.pattern, fieldName, displayName, generalNameInfo.message);

        Func<string, string, Func<string, Validated<string>>> letterRule      = (fieldName, displayName) => ValidationFunctions.CreateRegexRule<string>(capitalLetterInfo.pattern, fieldName, displayName, capitalLetterInfo.message);

        Func<string, string, Func<string, Validated<string>>> lengthRule      = (fieldName, displayName) => ValidationFunctions.CreatePredicateRule<string>(s => s.Length > 1 && s.Length <= 50, fieldName, displayName, lengthMessage);


        var familyNameRule = lengthRule.AndThen(letterRule);


        ValidationFunctionCache.AddOrUpdate(GlobalValues.Rule_Key_String_Length_2_50, lengthRule);
        ValidationFunctionCache.AddOrUpdate(GlobalValues.Rule_Key_String_Allowed_Salutations, salutationRule);
        ValidationFunctionCache.AddOrUpdate(GlobalValues.Rule_Key_String_Given_Name, generalNameRule);
        ValidationFunctionCache.AddOrUpdate(GlobalValues.Rule_Key_String_Family_Name, familyNameRule);

    }
}
