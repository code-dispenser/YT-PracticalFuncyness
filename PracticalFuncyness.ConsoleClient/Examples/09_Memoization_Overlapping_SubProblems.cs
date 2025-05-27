using PracticalFuncyness.ConsoleClient.Common.Utils;
using System.Diagnostics;
using System.Numerics;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class Memoization_Overlapping_SubProblems
{
    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("09 - Memoization Overlapping Subproblems"));
        /*
            * Memoization is a technique to cache the results of function calls to avoid redundant calculations.
            * This is particularly useful for expensive operations or recursive functions with overlapping subproblems.
            * In this example, we will demonstrate memoization using a simple Fibonacci function, with and without memoization. 
        */

        static long FibonacciSlow(int value)
        {
            if (value <= 1) return value;
            return FibonacciSlow(value - 1) + FibonacciSlow(value - 2);
        }
        
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        var fibonacciSlowResult = FibonacciSlow(40); //FibonacciSlow(100) Approx 72 THOUSAND YEARS.

        stopwatch.Stop();

        Console.WriteLine($"FibonacciSlow(40)\t = {fibonacciSlowResult:N0} in {stopwatch.ElapsedMilliseconds}ms");

        // Now let's restructure the routine so we can use our memoization utility to optimize the Fibonacci function

        Func<int, BigInteger> memoizedFibonacci = null!;
        
        static BigInteger Fibonacci(Func<int, BigInteger> fib, int value)
        {
            if (value <= 1) return value;
            return fib(value - 1) + fib(value - 2);
        }

        stopwatch.Restart();

        memoizedFibonacci = MemoizeUtils.Memoize<int,BigInteger>(n => Fibonacci(memoizedFibonacci, n));

        var memoizedFibonacciResult = memoizedFibonacci(200);

        stopwatch.Stop();

        Console.WriteLine($"memoizedFibonacci(200)\t = {memoizedFibonacciResult:N0} in {stopwatch.ElapsedMilliseconds}ms");
        /*
            * Now go ask ChatGpt how long a standard pc would take to calculate fibonacci(200) without memoization - there are better, but our util is versatile!. 
        */
    }
}
