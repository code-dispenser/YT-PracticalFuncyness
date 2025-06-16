using PracticalFuncyness.ConsoleClient.Common.Functors;
using PracticalFuncyness.ConsoleClient.Common.Models;
using System.Text.RegularExpressions;

namespace PracticalFuncyness.ConsoleClient.Common.Utils;

internal static class ValidationFunctions
{
    public static Func<T, Validated<T>> CreateRegexRule<T>(string pattern, ValidationEntry failureReason, RegexOptions regexOptions = RegexOptions.None) where T : notnull
    {
        var regex = new Regex(pattern, regexOptions | RegexOptions.Compiled);//closure over the compiled regex instance

        return value =>
        {
            try
            {
                return regex.IsMatch(value?.ToString() ?? string.Empty) ? Validated<T>.Valid(value!) : Validated<T>.Invalid(failureReason);
            }
            catch//maybe log ex for inspection
            {
                return Validated<T>.Invalid(failureReason);
            }

        };
    }

    public static Func<T, Validated<T>> CreatePredicateRule<T>(Func<T, bool> predicate, ValidationEntry failureReason) where T : notnull
    {
        return value =>
        {
            try
            {
                var result = predicate(value);

                return result ? Validated<T>.Valid(value) : Validated<T>.Invalid(failureReason);
            }
            catch//maybe log ex for inspection
            {
                return Validated<T>.Invalid(failureReason);
            }
        };
    }
    public static Func<T, Validated<T>> AndThen<T>(this Func<T, Validated<T>> thisFunc, Func<T, Validated<T>> nextFunc) where T : notnull

    => input =>
    {
        var firstResult = thisFunc(input);
        var secondResult = nextFunc(input);

        return (firstResult.IsValid && secondResult.IsValid)
                    ? Validated<T>.Valid(input)
                        : Validated<T>.Invalid(firstResult.Failures.Concat(secondResult.Failures).ToList());
    };



    public static Func<T, Validated<T>> CreateRegexRule<T>(string pattern, string fieldName, string displayName, string failureMessage, RegexOptions regexOptions = RegexOptions.None) where T : notnull
    {
        var regex = new Regex(pattern, regexOptions | RegexOptions.Compiled);//closure over the compiled regex instance

        return value =>
        {
            try
            {
                return regex.IsMatch(value?.ToString() ?? string.Empty) ? Validated<T>.Valid(value!) : Validated<T>.Invalid(new ValidationEntry(fieldName, displayName, failureMessage));
            }
            catch//maybe log ex for inspection
            {
                return Validated<T>.Invalid(new ValidationEntry(fieldName, displayName, failureMessage));
            }

        };
    }

    public static Func<T, Validated<T>> CreatePredicateRule<T>(Func<T, bool> predicate, string fieldName, string displayName, string failureMessage) where T : notnull
    {
        return value =>
        {
            try
            {
                var result = predicate(value);

                return result ? Validated<T>.Valid(value) : Validated<T>.Invalid(new ValidationEntry(fieldName,displayName,failureMessage));
            }
            catch//maybe log ex for inspection
            {
                return Validated<T>.Invalid(new ValidationEntry(fieldName, displayName, failureMessage));
            }
        };
    }
    public static Func<string, string, Func<T, Validated<T>>> AndThen<T>(this Func<string, string, Func<T, Validated<T>>> first, Func<string, string, Func<T, Validated<T>>> second) where T : notnull
    {
        return (fieldName, displayName) =>
        {
            var firstRule  = first(fieldName, displayName);
            var secondRule = second(fieldName, displayName);

            return firstRule.AndThen(secondRule); //Now call original AndThen
        };
    }

}

