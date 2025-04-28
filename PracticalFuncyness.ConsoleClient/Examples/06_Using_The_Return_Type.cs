using PracticalFuncyness.ConsoleClient.Common.Data;
using PracticalFuncyness.ConsoleClient.Common.Extensions;
using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Monads;
using PracticalFuncyness.ConsoleClient.Common.Utils;
using System.Text.Json;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal static class Using_The_Return_Type
{
    public static async Task Run()
    {
        Console.WriteLine(GeneralUtils.CreateHeading("06 - Result Monad in Action"));

        var result = GetFileForProcessing(StaticData.JsonBadRegistrationText)
                            .OnFailure(do_onFailure: _ => GetFileForProcessing(StaticData.JsonRegistrationText))//or rename method to remove param name, needed to resolve overload that uses an action.
                                .OnSuccessTry(file => ProcessJsonFile(file), JsonExceptionHandler<Registration>)
                                    .OnSuccess(registration => new RegistrationEmail(registration.FirstName, registration.EmailAddress))
                                        .OnSuccess(registrationEmail => SendEmail(registrationEmail))
                                            .OnFailure(failure => Console.WriteLine(failure.Message))
                                                .Finally(failure => false, success => success);
        

        await Console.Out.WriteLineAsync($"Did the processing succeed successfully: {result}\r\n");

    }

    private static Result<string> GetFileForProcessing(string filePath)
    
        => filePath != StaticData.JsonBadRegistrationText ? StaticData.JsonRegistrationText : new FailureBase.FileProcessingFailure("Failed to locate the file.");


    private static Result<Registration> ProcessJsonFile(string jsonString)

        => JsonSerializer.Deserialize<Registration>(jsonString, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

    private static bool SendEmail(RegistrationEmail registrationEmail)
    {
        Console.WriteLine($"Sending a registration email to: {registrationEmail.FirstName} using the address: {registrationEmail.EmailAddress}");
        return true;
    }

    private static Result<T> JsonExceptionHandler<T>(Exception exceptionToHandle)//could be a Handle method in an IExceptionHandler Interface, with implementations per category for dependency injection.
    
        => exceptionToHandle switch
            {
                JsonException jsonException => new FailureBase.FileProcessingFailure($"Json Exception: {jsonException.Message}"),
                _ => new FailureBase.ApplicationFailure($"Unknown Exception: {exceptionToHandle.Message}")
            };

}


