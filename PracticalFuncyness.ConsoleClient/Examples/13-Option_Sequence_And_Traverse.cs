using PracticalFuncyness.ConsoleClient.Common.Extensions;
using PracticalFuncyness.ConsoleClient.Common.Monads;
using PracticalFuncyness.ConsoleClient.Common.Utils;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class Option_Sequence_And_Traverse
{
    private static readonly List<string> _pantry = ["Eggs", "Flour", "Rice", "Pasta", "Olive Oil", "Sugar", "Tomatoes", "Basil", "Vanilla"];

    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("13 - Sequence and Traverse "));

        List<string> recipeOneIngredients = ["Eggs", "Flour", "Sugar", "Milk", "Vanilla"];// Note: "Milk" is not in the pantry
        List<string> recipeTwoIngredients = ["Pasta", "Olive Oil", "Tomatoes", "Basil"];  

        IEnumerable<Option<string>> availableItems = recipeOneIngredients.Select(GetItemFromPantry);

        availableItems.Sequence().Match(
                                    act_onNone: () => Console.WriteLine($"Not all ingredients for Recipe One are available in the pantry.\r\n"),
                                    act_onSome: items => Console.WriteLine($"Recipe One can be made with the following ingredients from the pantry: {string.Join(",", items)}\r\n")
                                );
        
        // Or simply recipeOneIngredients.Select(GetItemFromPantry).Sequence().Match . . . 

        recipeTwoIngredients.Traverse(GetItemFromPantry).Match(
                                    act_onNone: () => Console.WriteLine("Not all ingredients for Recipe Two are available in the pantry."),
                                    act_onSome: items => Console.WriteLine($"Recipe Two can be made with the following ingredients from the pantry: {string.Join(", ", items)}")
                                );

    }

    private static Option<string> GetItemFromPantry(string itemName)
        
        => String.IsNullOrWhiteSpace(itemName) 
            ? Option<string>.None() 
                : _pantry.Contains(itemName) ? Option<string>.Some(itemName) : Option<string>.None();
}
