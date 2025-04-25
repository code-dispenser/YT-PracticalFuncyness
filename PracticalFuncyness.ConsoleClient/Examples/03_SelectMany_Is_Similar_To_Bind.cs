using PracticalFuncyness.ConsoleClient.Common.Monads;
using PracticalFuncyness.ConsoleClient.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal static class SelectMany_Is_Similar_To_Bind
{
    public static async Task Run()
    {
        Console.WriteLine(GeneralUtils.CreateHeading("03 - Linq List SelectMany is the equivalent of Bind"));

        List<int>  numberInList  = [10];//collection syntax
        Chain<int> numberInChain = new(10);

        var newNumberInList = numberInList.SelectMany(n => new List<int> { n * 2 }) // <<< This is the LINQ operation that flattens the result, like Bind, 
                                          .ToList();                                //     so we get a List of int's instead of a List of List of int's.

        var newNumberInChain = numberInChain.Bind(n => new Chain<int>(n * 2)); // <<< This is the Chain operation that flattens the result, like SelectMany,
                                                                               //     so we get a Chain of int instead of a Chain of Chain of int.


        Console.WriteLine($"Number in list: {numberInList[0]}");
        Console.WriteLine($"Number in container: {numberInChain}\r\n");

        Console.WriteLine($"New number in list: {newNumberInList[0]}");
        Console.WriteLine($"New number in container: {newNumberInChain}\r\n");

        Console.WriteLine($"Original variable values after 'SelectMany \\ Bind':\r\n");
        Console.WriteLine($"\tnumberInList: {numberInList[0]}");
        Console.WriteLine($"\tnumberInContainer: {numberInChain}\r\n");

        Console.WriteLine($"A lot of Monads such as Either (Result Type), Option and Maybe for example do not allow direct access to the values they contain. " +
                          $"Our Chain class allows direct access to the Value property. " +
                          $"We will discuss this in the next video when the function Match is introduced\r\n");


        await Task.CompletedTask;

    }
}
