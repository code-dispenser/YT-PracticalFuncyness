using PracticalFuncyness.ConsoleClient.Common.Monads;

namespace PracticalFuncyness.ConsoleClient.Common.Extensions;

public static class OptionExtensions
{

    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> option, Func<TIn, Option<TOut>> onValue) where TIn : notnull where TOut : notnull

        => option.Bind(onValue);

    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> option, Func<TIn, TOut> onValue) where TIn : notnull where TOut : notnull

        => option.Map(onValue);


    public static Option<T> OrElse<T>(this Option<T> option, Func<Option<T>> onNone) where T : notnull

        => option.IsSome ? option : onNone();

    public static Option<T> OrElse<T>(this Option<T> option, Func<T> onNone) where T : notnull

        => option.IsSome ? option : onNone();//uses implicit conversion so null will be none.




}
