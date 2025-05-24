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

        await Match_To_Get_At_Private_Data.Run();

        await Using_The_Return_Type.Run();

        await Mild_Curry_ing.Run();

        await Partial_Application.Run();

        Console.ReadLine();
    }
}
