using PracticalFuncyness.ConsoleClient.Common.Monads;

namespace PracticalFuncyness.ConsoleClient.Common.Extensions;

public static class OptionExtensions
{

    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> option, Func<TIn, Option<TOut>> onSome) where TIn : notnull where TOut : notnull

        => option.Bind(onSome);

    public static Option<TOut> AndThen<TIn, TOut>(this Option<TIn> option, Func<TIn, TOut> onSome) where TIn : notnull where TOut : notnull

        => option.Map(onSome);


    public static Option<T> OrElse<T>(this Option<T> option, Func<Option<T>> onNone) where T : notnull

        => option.IsSome ? option : onNone();

    public static Option<T> OrElse<T>(this Option<T> option, Func<T> onNone) where T : notnull

        => option.IsSome ? option : onNone();//uses implicit conversion so null will be none.


    public static Option<IEnumerable<T>> Sequence<T>(this IEnumerable<Option<T>> options) where T : notnull
    {
        if (options is null) throw new ArgumentNullException(nameof(options));

        List<T> list = [];

        foreach (var option in options)
        {
            if (option.IsNone) return Option<IEnumerable<T>>.None();

            list.Add(option.GetValueOr(default!));

        }

        return Option<IEnumerable<T>>.Some(list);
    }

    public static Option<IEnumerable<TOut>> Traverse<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, Option<TOut>> transformer) where TIn : notnull where TOut : notnull
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        List<TOut> transformerResults = [];

        foreach (var item in source)
        {
            var newOption = transformer(item);

            if (newOption.IsNone) return Option<IEnumerable<TOut>>.None();

            transformerResults.Add(newOption.GetValueOr(default!));
        }

        return Option<IEnumerable<TOut>>.Some(transformerResults);
    }
}
