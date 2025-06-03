using PracticalFuncyness.ConsoleClient.Common.Monads;

namespace PracticalFuncyness.ConsoleClient.Common.Extensions;

public static class OptionExtensions
{

    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> Option, Func<TIn, Option<TOut>> onValue) where TIn : notnull where TOut : notnull

        => Option.Bind(onValue);

    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> Option, Func<TIn, TOut> onValue) where TIn : notnull where TOut : notnull

        => Option.Map(onValue);


    public static Option<T> OrElse<T>(this Option<T> Option, Func<Option<T>> onNone) where T : notnull

        => Option.IsSome ? Option : onNone();

    public static Option<T> OrElse<T>(this Option<T> Option, Func<T> onNone) where T : notnull

        => Option.IsSome ? Option : onNone();//uses implicit conversion so null will be none.
}
