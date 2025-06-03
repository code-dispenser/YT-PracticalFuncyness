using PracticalFuncyness.ConsoleClient.Common.Models;
using System.Text.Json.Serialization;

namespace PracticalFuncyness.ConsoleClient.Common.Monads;

public readonly record struct Option<T> where T : notnull
{
    private readonly T _value;

    public bool IsSome { get; }
    public bool IsNone => !IsSome;

    public Option(T value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        _value = value;
        IsSome = true;
    }

    public static implicit operator Option<T>(T value) => value is null ? default : new Option<T>(value);
    public static implicit operator Option<T>(None _) => default;

    public static Option<T> None() => default;
    public static Option<T> Some(T value) => new Option<T>(value);

    public T GetValueOr(T fallback)

        => IsSome ? _value : fallback;

    public void Match(Action act_onNone, Action<T> act_onSome)
    {
        if (IsSome) act_onSome(_value); else act_onNone();
    }
    public TOut Match<TOut>(Func<TOut> onNone, Func<T, TOut> onSome)

        => IsSome ? onSome(_value) : onNone();

    public Option<TOut> Map<TOut>(Func<T, TOut> onSome) where TOut : notnull

        => IsSome ? new Option<TOut>(onSome(_value)) : default;

    public Option<TOut> Bind<TOut>(Func<T, Option<TOut>> onSome) where TOut : notnull

        => IsSome ? onSome(_value) : default;

    public override string ToString()

        => IsSome ? $"Some({_value})" : "Ø";
}