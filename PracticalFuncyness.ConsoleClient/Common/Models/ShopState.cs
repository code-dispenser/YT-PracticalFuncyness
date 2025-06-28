namespace PracticalFuncyness.ConsoleClient.Common.Models;

public record ShopState(Dictionary<string, int> Inventory, decimal Cash)
{
    public ShopState WithInventory(string item, int quantity)
    {
        Dictionary<string, int> newInventory = new(Inventory);

        newInventory[item]  = quantity;

        return this with { Inventory = newInventory };
    }
    public ShopState WithCash(decimal amount)

        => this with { Cash = amount };

}
