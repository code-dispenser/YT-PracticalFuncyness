using PracticalFuncyness.ConsoleClient.Common.Data;
using PracticalFuncyness.ConsoleClient.Common.Extensions;
using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Utils;
using System.Text.Json;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal static class Map_Tap_For_Chaining
{
    public static async Task Run()
    {
        Console.WriteLine(GeneralUtils.CreateHeading("02 - Map and Tap Chaining Concept Example"));
        Console.WriteLine($"Processing pipeline example using Map and Tap.\r\n");

        _ = await ProcessJsonFile(StaticData.JsonRegistrationText)
                    .Tap(r => Logger($"Received registration for: {r.FirstName} {r.Surname}, Age: {r.Age}"))
                    .Tap(r => Logger($"Address: {r.AddressLine}, {r.Town}, {r.City}, {r.County}, {r.PostCode}"))
                    .Map(r => new RegistrationEmail(r.FirstName, r.EmailAddress))
                    .Tap(email => Logger($"Preparing to send email to: {email.EmailAddress}"))
                    .Map(SendEmail)
                    .Tap(sent =>
                    {
                        var status = sent ? "Email sent successfully." : "Failed to send email.";
                        Logger($"{status}\r\n");
                    });

        Console.WriteLine($"Unit conversion using Map.\r\n");

        var degreesCelsius = "80.4°F".Tap(x => Logger($"Converting {x} to celcius"))
                                     .Map(temp => temp.Replace("°F", "").Trim())
                                     .Map(Double.Parse)
                                     .Map(f => (f - 32) * 5.0 / 9.0)
                                     .Map(c => Math.Round(c, 1))
                                     .Map(c => $"{c}°C");

        Console.WriteLine($"Converted result: {degreesCelsius}\r\n");
    }
    private static async Task<bool> SendEmail(RegistrationEmail registrationEmail)
    {
        await Console.Out.WriteLineAsync($"Sending a registration email to: {registrationEmail.FirstName} using the address: {registrationEmail.EmailAddress}");
        return true;
    }

    private static void Logger(string message)
    
        => Console.WriteLine($"Log: {message}");
    
    private static Registration ProcessJsonFile(string jsonString)

        => JsonSerializer.Deserialize<Registration>(jsonString, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

}
