using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Monads;
using PracticalFuncyness.ConsoleClient.Common.Utils;
using System.Net.Sockets;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class Using_The_State_Monad
{
    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("16 - Using the State Monad"));

        var initialShopState = new ShopState(new Dictionary<string, int> { ["Apples"] = 10, ["Bananas"] = 5, ["Oranges"] = 7 } ,100.00m);

        Console.WriteLine("First Run\r\n--- Initial Shop State ---");
        Console.WriteLine($"Inventory: {string.Join(", ", initialShopState.Inventory.Select(kv => $"{kv.Key}: {kv.Value}"))}");
        Console.WriteLine($"Cash in register: {initialShopState.Cash:C}\r\n");

        var salesProcess = SellItem("Apples", 2, 1.50m) 
                            .Bind(_ => SellItem("Bananas", 3, 0.75m)) // << binds NOT nested as we do not need the intermediate TValue item sales value
                            .Bind(_ => SellItem("Oranges", 1, 2.00m)) 
                            .Bind(_ => SellItem("Apples", 10, 1.50m));

        var (lastSaleValue, firstShopState) = salesProcess.Run(initialShopState);

        Console.WriteLine("\n--- First Run Shop State After Sales ---");
        Console.WriteLine($"Inventory: {string.Join(", ", firstShopState.Inventory.Select(kv => $"{kv.Key}: {kv.Value}"))}");
        Console.WriteLine($"Cash in register: {firstShopState.Cash:C}");
        Console.WriteLine($"Last (Operation) Sale Value: {lastSaleValue:C}\r\n");

        Console.WriteLine("Second Run Capturing Intermediate Values\r\n");

        var (runSales, finalShopState)  = SellItem("Apples", 2, 1.50m)
                                            .Bind(appleSales  => SellItem("Bananas", 3, 0.75m) //<< Nested bind/lambda as we want to capture intermediate sale values
                                            .Bind(bananaSales => SellItem("Oranges", 1, 2.00m)
                                            .Bind(orangeSales => SellItem("Apples", 10, 1.50m)
                                            .Map(moreAppleSales => appleSales + bananaSales + orangeSales + moreAppleSales))))
                                            .Run(firstShopState with { });

        Console.WriteLine($"\r\nSales in run: {runSales:C}\r\n");
        Console.WriteLine("\n--- Second Run Shop State After Sales ---");
        Console.WriteLine($"Inventory: {string.Join(", ", finalShopState.Inventory.Select(kv => $"{kv.Key}: {kv.Value}"))}");
        Console.WriteLine($"Cash in register: {finalShopState.Cash:C}");
       
    }

    public static State<ShopState, decimal> SellItem (string item, int quantity, decimal pricePerUnit) 
        
        => State<ShopState, decimal>.Get.Bind(shopState => // Don't forget the Get sets both the TValue and TState to TState.
            {
                if (!shopState.Inventory.ContainsKey(item) || shopState.Inventory[item] < quantity)
                {
                    Console.WriteLine($"Not enough {item} in stock to sell {quantity}. Available: {shopState.Inventory.GetValueOrDefault(item, 0)}");
                    return State<ShopState, decimal>.Return(0.00m); // Do not update state only return what is the current sale value
                }

                var newQuantity   = shopState.Inventory[item] - quantity;
                var itemSaleValue = quantity * pricePerUnit;
                var newCash       = shopState.Cash + itemSaleValue;

                Console.WriteLine($"Sold {quantity} {item} for: {itemSaleValue:C}");

                var updatedState = shopState
                    .WithInventory(item, newQuantity)
                    .WithCash(newCash);

                return State<ShopState, decimal>.Put(updatedState).Map(_ => itemSaleValue); // Update State and then return the current sale value
            });
}



