using PracticalFuncyness.ConsoleClient.Common.Utils;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class Mild_Curry_ing
{
    /*
        Currying is a process that transforms a function with multiple arguments into a sequence of functions, each taking a single argument.
        At each step, a new function is returned that expects the next argument from the original function's parameter list.
        Although you can technically execute any of these intermediate functions (since they're valid delegates), in practice, you usually call
        the final function in the chain to get the result.

    */
    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("07 - Mild Currying"));

        // This is a standard method that takes three integers and returns their sum. It represents what we're ultimately trying to calculate through currying.
        static int AddNumbers(int x, int y, int z) => x + y + z;

        /*
            * Here we define a curried version of AddNumbers. Instead of taking all arguments at once, it takes one argument at a time, returning a new function at each step until all arguments are provided.
        */ 

        Func<int, Func<int, Func<int, int>>> curriedAdd = x => y => z => x + y + z;

        Console.WriteLine($"The curriedAdd function with x = 10, y = 20, z = 30: curriedAdd(10)(20)(30): {curriedAdd(10)(20)(30)}\r\n"); // < < < Ugly but works! 

        /*
            *  We partially apply the first argument (x = 10). The resulting function, addValue_x, takes the next argument (y) and returns another function that takes the final argument (z).
            *  So it's a Func<int, Func<int, int>> — one argument at a time.
        */

        Func<int, Func<int, int>> addWith_x = curriedAdd(10); // we can also use var addWith_x = curriedAdd(10)

        Console.WriteLine($"addWith_x = 10 applied, now called with y = 20 and z = 30: addWith_x(20)(30): {addWith_x(20)(30)}");

        /*
            * We now apply the second argument (y = 20) to addValue_x. The result,  addWith_x_y , is a function that only needs one more argument. 
            * A function that takes one integer and returns the final integer result.
        */

        Func<int, int> addWith_x_y = addWith_x(20);

        Console.WriteLine($"addWith_x_y (x = 10, y = 20) applied, providing the last param z = 30: addWith_x_y(30): {addWith_x_y(30)}\r\n");
        
    }

}
/*
    * If you want to investigate currying further you can create a generic extension method with the overloads for the number of parameters you want to support,
    * which can then be applied on a func delegate. 

    * You would create whatever overload you needed. 
    * 
    * Create the initial delegate then / or attach to an existing delegate the Curry extension that just starts the process.
    * 
    *  Func<int, int, int, int> Make = (x, y, z) => x + y + z;
    *  
    *  var curryingFunc = Make.Curry()
    *  
    *  var curriedFunc_with_x = curryingFunc(10); etc etc
*/
public static class CurryExtensions
{
    public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        
        => arg1 => arg2 => func(arg1, arg2);


    public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)

        => arg1 => arg2 => arg3 => func(arg1, arg2, arg3);

    public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
        
        => arg1 => arg2 => arg3 => arg4 => func(arg1, arg2, arg3, arg4);
}