using PracticalFuncyness.ConsoleClient.Common.Extensions;
using PracticalFuncyness.ConsoleClient.Common.Functors;
using PracticalFuncyness.ConsoleClient.Common.Utils;

using static PracticalFuncyness.ConsoleClient.Common.Utils.ValidationFunctions;

namespace PracticalFuncyness.ConsoleClient.Common.Models;

internal record FullName
{
    public string Title      { get; }
    public string GivenName  { get; }
    public string FamilyName { get; }

    private static readonly Func<string,Validated<string>> _titleRule      = CreateRegexRule<string>(@"^(Mr|Mrs|Ms|Dr|Prof)$", new("Title", "Title", "Must be one of Mr, Mrs, Ms, Dr, Prof"));

    private static readonly Func<string,Validated<string>> _givenNameRule  = CreateRegexRule<string>(@"^(?=.{2,50}$)[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]+$", 
                                                                                                    new("GivenName", "Given Name", "Must start with a capital letter and be between 2 and 50 characters in length"));

    private static readonly Func<string,Validated<string>> _familyNameRule = CreatePredicateRule<string>(name => name.Length > 1 && name.Length <= 50, new("FamilyName", "Family Name", "Must be between 2 and 50 characters long"))
                                                                                .AndThen(CreateRegexRule<string>(@"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", new("FamilyName", "Family Name", "Must start with a capital letter")));

    private FullName(string title, string givenName, string familyName)

        => (Title, GivenName, FamilyName) = (title, givenName, familyName);

    public static Validated<FullName> Create(string title, string givenName, string familyName)

        => (_titleRule(title), _givenNameRule(givenName), _familyNameRule(familyName)).Combine((title, given, family) => new FullName(title, given, family));//or .ToResult() if you want to return a Result type instead of Validated
}