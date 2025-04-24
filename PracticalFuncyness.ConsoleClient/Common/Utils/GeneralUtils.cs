using PracticalFuncyness.ConsoleClient.Common.Extensions;

namespace PracticalFuncyness.ConsoleClient.Common.Utils;

internal static class GeneralUtils
{
    public static string CreateHeading(string headingText)

        => new String('=', headingText.Length + 2).Map(x => $"{x}\r\n{headingText}\r\n{x}\r\n");
    

    public static string GetString(this object? obj, string defaultValue)
    {
        return obj?.ToString() ?? defaultValue;
    }
}
