using PracticalFuncyness.ConsoleClient.Examples;

namespace PracticalFuncyness.ConsoleClient;

internal class Program
{
    static async Task Main()
    {
        await Select_Is_Map.Run();
        
        await Map_Tap_For_Chaining.Run();

        await SelectMany_Is_Similar_To_Bind.Run();

        await Bind_For_Chaining_Monads.Run();
        
        Console.ReadLine();
    }
}
