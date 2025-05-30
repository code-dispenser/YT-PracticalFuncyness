namespace PracticalFuncyness.ConsoleClient.Common.Extensions;

internal static class FunctionalExtensions
{
    #region Map
    /*
        * This is a general transformation function, not a functor-style Map.
        * 
        * In functional programming, Map is defined for structures (like List, Option, Task),
        * where it applies a function to the inner value and returns a new structure of the same kind.
        * 
        * This version enables chaining on any value, but it does not preserve or operate within a structure,
        * so it's not a Functor Map in the formal sense.
        * 
        * The real power of Map is when we use it on 'Structured types' (custom wrappers) where the transformation 
        * happens within the context, without unwrapping or breaking the structure. List<T> is a perfect example that you most likely use every day.
        *
        * Tap lets us run a side effect — like logging, validation, or triggering something — without touching the actual value. It should passes the value through unchanged.
        * 
        * You can name these functions/overloads however you like, in my own Result Type library called Flow I use names such as OnSuccess, OnFailure, Then and ReturnAs etc.
        * https://github.com/code-dispenser/Flow
    */
    public static TOut Map<TIn, TOut>(this TIn thisValue, Func<TIn, TOut> do_Map)
        
        => do_Map(thisValue);

    #region Async Maps
    public static async Task<TOut> Map<TIn, TOut>(this TIn thisValue, Func<TIn, Task<TOut>> do_Map)

        => await do_Map(thisValue);

    public static async Task<TOut> Map<TIn, TOut>(this Task<TIn> thisValue, Func<TIn, TOut> do_Map)

        => do_Map(await thisValue);

    public static async Task<TOut> Map<TIn, TOut>(this Task<TIn> thisValue, Func<TIn, Task<TOut>> do_Map)
    
        => await do_Map(await thisValue);

    #endregion

    #endregion

    #region Tap
    public static T Tap<T>(this T thisValue, Action<T> act_On)
    {
        act_On(thisValue);

        return thisValue;
    }

    #region Async Taps

    public static async Task<T> Tap<T>(this Task<T> thisValue, Action<T> act_On)
    {
        var awaitedValue = await thisValue;
     
        act_On(awaitedValue);
        
        return awaitedValue;
    }
    public static async Task<T> Tap<T>(this T thisValue, Func<T, Task> act_On)
    {
        await act_On(thisValue);
        
        return thisValue;
    }

    public static async Task<T> Tap<T>(this Task<T> thisValue, Func<T, Task> act_On)
    {
        var awaitedValue = await thisValue;
        
        await act_On(awaitedValue);
        
        return awaitedValue;
    }

    #endregion

    #endregion

    #region AndThen extension + Componse util

    public static Func<TIn,TOut> AndThen<TIn,TMid, TOut>(this Func<TIn,TMid> thisFunc, Func<TMid, TOut> nextFunc)
        
       => inputVal => nextFunc(thisFunc(inputVal));

    public static Func<TIn, TOut> Compose<TIn, TOut>(Func<TIn, TOut> thisFunc)

        => thisFunc;

    #endregion


}
