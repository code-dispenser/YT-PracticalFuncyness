using PracticalFuncyness.ConsoleClient.Common.Utils;
using static PracticalFuncyness.ConsoleClient.Examples.Partial_Application;

namespace PracticalFuncyness.ConsoleClient.Examples;
 
internal class Partial_Application
{

    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("08 - Partial Application"));

        List<string> users = ["user1@gmail.com", "user2@hotmail.com", "user3@icloud.com"]; // << List of users probably obtained from a database

        EmailSender emailerSender = SendEmail;  // << We now have a delegate type that we can attach extensions to

        KeyedSender keyedSender = emailerSender.CreateKeyedSender("123", 1, "spam_department@spam.com"); // << Create a base email sender with fixed apiKey, priority, and fromAddress

        bool emailSent = await keyedSender("Registration, successful.", "Welcome to . . .", "user@home.com");

        /*
            * You can get even more specific by fixing the subject and body as well. This produces a campaign-specific email sender.
         */
        var campaignEmailer = keyedSender.CreateCampaignEmailer("Summer Specials", "Get 50% off your next purchase. To claim your voucher . . ");
        
        /*
            * Create and await all email sending tasks for the user list 
        */
        var tasks = users.Select(user => campaignEmailer(user)).ToArray();

        _ = await Task.WhenAll(tasks);
    }

    /*
        * This method simulates sending an email. In a real-world app, this would be part of a service (e.g., IEmailService) or in a static Email Module class
    */
    public static async Task<bool> SendEmail(string apiKey, int priority, string fromAddress, string subject, string body, string toAddress)
    {
       await Console.Out.WriteLineAsync($"Sending email to: {toAddress} with subject: {subject} and body: {body} from: {fromAddress} with priority: {priority} using apiKey: {apiKey}");

        return true;
    }
 
}
/*
    Partial application is about fixing values for one or more parameters.
    Unlike currying, which transforms structure into unary chains (one argument per function),
    partial application allows us to "lock in" values without changing the function's shape entirely.
*/

public delegate Task<bool> EmailSender(string apiKey, int priority, string fromAddress, string subject, string body, string toAddress);
public delegate Task<bool> KeyedSender(string subject, string body, string toAddress);

public static class EmailSenderExtensions
{
    public static KeyedSender CreateKeyedSender(this EmailSender emailSender, string apiKey, int priority, string fromAddress)

        => (subject, body, toAddress) => emailSender(apiKey, priority, fromAddress, subject, body, toAddress);

    public static Func<string,Task<bool>> CreateCampaignEmailer(this KeyedSender keyedSender, string subject, string body)
        
        => (toAddress) => keyedSender(subject, body, toAddress);
}

/*
    Generic partial application support for any function with two parameters. You can add more overloads for other arities as needed.
*/
public static class PartialApplicationExtensions //This is a generic partial application method that can be used on any delegate type, create what ever overloads your like.
{
    public static Func<T1, Func<T2, TResult>> Partial<T1, T2, TResult>(this Func<T1, T2, TResult> func)
    
        => arg1 => arg2 => func(arg1, arg2);
    
}
