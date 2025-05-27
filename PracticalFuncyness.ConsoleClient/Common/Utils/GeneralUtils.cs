using PracticalFuncyness.ConsoleClient.Common.Extensions;
using System.Text;

namespace PracticalFuncyness.ConsoleClient.Common.Utils;

internal static class GeneralUtils
{
    public static string CreateHeading(string headingText)

        => new String('=', headingText.Length + 2).Map(x => $"{x}\r\n{headingText}\r\n{x}\r\n");
    

    public static string BreakOnCaps(string textValue)
    {
        if (string.IsNullOrWhiteSpace(textValue) || textValue.Length < 2) return textValue;
        
        var stringBuilder = new StringBuilder(textValue.Length * 2);

        stringBuilder.Append(textValue[0]); 

        for (int index = 1; index < textValue.Length; index++)
        {
            char currentChar  = textValue[index];
            char previousChar = textValue[index -1];

            if (char.IsLower(textValue[index-1]) && char.IsUpper(textValue[index]))
            {
                stringBuilder.Append(" ");
            }

            stringBuilder.Append(currentChar);
        }

        return stringBuilder.ToString();
    }
}
