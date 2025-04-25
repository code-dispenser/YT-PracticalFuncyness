using PracticalFuncyness.ConsoleClient.Common.Monads;
using PracticalFuncyness.ConsoleClient.Common.Utils;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal static class Bind_For_Chaining_Monads
{
    public static async Task Run()
    {
        Console.WriteLine(GeneralUtils.CreateHeading("04 - Bind is for Chaining Monads"));

        var bindResult = ProcessTransaction(1000)
                            .Bind(amount => DeductTransactionFee(amount))
                            .Bind(amount => ApplyDiscount(amount))
                            .Bind(amount => CompleteTransaction(amount))
                            .Bind(FormatInvoice);//<<< You can shorten the function composition by removing the lambda. Bind can also transform the value to a different type.


        Console.WriteLine($"\r\nYour invoice: {bindResult.Value}\r\n");//Our return is a Chain<string> with direct access to the Value property.

        var bindResultWithMap = ProcessOrder(1000)
                                    .Bind(ApplyDiscount)
                                    .Map(ApplyTax) // !!! ApplyTax returns a decimal, so we use Map to lift that pure function into the monad Chain<decimal> (Elevated World) 
                                    .Bind(CompleteOrder)
                                    .Bind(FormatInvoice);

        Console.WriteLine($"\r\nYour receipt: {bindResultWithMap.Value}\r\n");

        await Task.CompletedTask;
    }

    public static decimal ApplyTax(decimal amount)
    {
        decimal tax = amount * 0.15m;
        Console.WriteLine($"Tax applied: {tax}. New amount after tax: {amount + tax}");
        return amount + tax; // <<< Returning just a value, not put in our Chain monad
    }
    public static Chain<string> FormatInvoice(decimal amount)

        => new Chain<string>($"Transaction Invoice: {amount:C2}");

    public static Chain<decimal> ProcessTransaction(decimal initialAmount)
    
        => new Chain<decimal>(initialAmount);
  
    public static Chain<decimal> DeductTransactionFee(decimal amount)
    {
        decimal fee = amount * 0.05m; 
        decimal newAmount = amount - fee;
        Console.WriteLine($"Transaction fee deducted: {fee}. New amount: {newAmount}");
        return new Chain<decimal>(newAmount);
    }

    public static Chain<decimal> ApplyDiscount(decimal amount)
    {
        decimal discount   = amount * 0.1m; 
        decimal newAmount  = amount - discount;
        Console.WriteLine($"Discount applied: {discount}. New amount: {newAmount}");
        return new Chain<decimal>(newAmount);
    }
    public static Chain<decimal> CompleteTransaction(decimal amount)
    {
        Console.WriteLine($"Transaction completed. Final amount: {amount}");
        return new Chain<decimal>(amount);
    }
    public static Chain<decimal> ProcessOrder(decimal initialAmount)
    
        => new Chain<decimal>(initialAmount);

    public static Chain<decimal> CompleteOrder(decimal amount)
    {
        Console.WriteLine($"Order completed. Final amount: {amount}");
        return new Chain<decimal>(amount); // Returning the completed order in a new container
    }
}
