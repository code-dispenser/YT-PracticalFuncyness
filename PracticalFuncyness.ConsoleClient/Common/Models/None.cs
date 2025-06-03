namespace PracticalFuncyness.ConsoleClient.Common.Models;

public readonly record struct None
{
    public static None Value => new(); 
    public override string ToString() => "Ø";
}