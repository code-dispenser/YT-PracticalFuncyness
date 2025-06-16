using PracticalFuncyness.ConsoleClient.Common.Functors;
using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Monads;

namespace PracticalFuncyness.ConsoleClient.Common.Extensions;

internal static class ValidatedExtensions
{
    public static Validated<TOut> Combine<T1, T2, TOut>(this (Validated<T1> first, Validated<T2> second) validations, Func<T1, T2, TOut> builder) where T1 : notnull where T2 : notnull where TOut : notnull
    {
        var (first, second)   = validations;
        var validationEntries = new List<ValidationEntry>();

        if (true == first.IsInvalid) validationEntries.AddRange(first.Failures);
        if (true == second.IsInvalid) validationEntries.AddRange(second.Failures);

        return validationEntries.Count == 0 ? Validated<TOut>.Valid(builder(first.GetValueOr(default!), second.GetValueOr(default!)))
                                            : Validated<TOut>.Invalid(validationEntries);

    }

    public static Validated<TOut> Combine<T1, T2, T3, TOut>(this (Validated<T1> first, Validated<T2> second, Validated<T3> third) validations, Func<T1, T2, T3, TOut> builder) where T1 : notnull where T2 : notnull where T3 : notnull where TOut : notnull
    {
        var (first, second, third)   = validations;
        var validationEntries = new List<ValidationEntry>();

        if (true == first.IsInvalid) validationEntries.AddRange(first.Failures);
        if (true == second.IsInvalid) validationEntries.AddRange(second.Failures);
        if (true == third.IsInvalid) validationEntries.AddRange(third.Failures);

        return validationEntries.Count == 0 ? Validated<TOut>.Valid(builder(first.GetValueOr(default!), second.GetValueOr(default!), third.GetValueOr(default!)))
                                            : Validated<TOut>.Invalid(validationEntries);

    }

    public static Result<T> ToResult<T>(this Validated<T> validated) where T : notnull
    
        => validated.IsValid ? Result<T>.Success(validated.GetValueOr(default!))
                                 : Result<T>.Failed(new FailureBase.ValidationFailure(validated.Failures, "Validation failed."));


                                                                  // The TOut could be the finished value or the nested Func   
    public static Validated<TOut> Apply<TIn, TOut>(this Validated<Func<TIn, TOut>> validatedFunc, Validated<TIn> validatedItem) where TIn : notnull where TOut : notnull
    {
        if (validatedFunc.IsValid && validatedItem.IsValid)
        {
            var func    = validatedFunc.GetValueOr(default!);
            var value   = validatedItem.GetValueOr(default!);
            var result  = func(value);
           
            return Validated<TOut>.Valid(result);
        }

        var failures = new List<ValidationEntry>();

        if (validatedFunc.IsInvalid) failures.AddRange(validatedFunc.Failures);
        if (validatedItem.IsInvalid) failures.AddRange(validatedItem.Failures);

        return Validated<TOut>.Invalid(failures);
    }
}
