using PracticalFuncyness.ConsoleClient.Common.Functors;
using PracticalFuncyness.ConsoleClient.Common.Utils;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal static class Select_Is_Map
{
    public static async Task Run()
    {
        Console.WriteLine(GeneralUtils.CreateHeading("01 - Linq 'Select' Is Just 'Map' Renamed"));

        List<int>      numberInList      = [10];//collection syntax
        Container<int> numberInContainer = new(10);
        
        var newNumberInList       = numberInList.Select(x => x + 10).ToList();
        var newNumberInContainer  = numberInContainer.Select(x => x + 10);

        Console.WriteLine($"Number in list: {numberInList[0]}");
        Console.WriteLine($"Number in container: {numberInContainer}\r\n");

        Console.WriteLine($"New number in list: {newNumberInList[0]}");
        Console.WriteLine($"New number in container: {newNumberInContainer}\r\n");

        Console.WriteLine($"Original variable values after 'Select' aka Map:\r\n");
        Console.WriteLine($"\tnumberInList: {numberInList[0]}");
        Console.WriteLine($"\tnumberInContainer: {numberInContainer}\r\n");

        Console.WriteLine($"This concept of operating on a Container is important to understand, it could be any type of container, " +
                          $"Map applies a function to the value and returns a new container of the same kind with the transformation.\r\n");

        Console.WriteLine("Operating on a known type allows you to chain operations together, like Linq does with IEnumerable.\r\n");

        await Task.CompletedTask;
    }
}
