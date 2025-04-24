using PracticalFuncyness.ConsoleClient.Examples;

namespace PracticalFuncyness.ConsoleClient;

internal class Program
{
    static async Task Main()
    {
        await Select_Is_Map.Run();
        
        await Map_Tap_For_Chaining.Run();

        Console.ReadLine();
    }
}
