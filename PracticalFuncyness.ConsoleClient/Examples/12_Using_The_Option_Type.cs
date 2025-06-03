using PracticalFuncyness.ConsoleClient.Common.Extensions;
using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Monads;
using PracticalFuncyness.ConsoleClient.Common.Utils;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class Using_The_Option_Type
{
    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("12 - Using the Option type for optional values and/or instead of null"));

        var userPreferences    = GetUserPreferences();
        var updatedPreferences = GetUpdatedUserPreferences();


        var accentColour = userPreferences.Theme.OrElse(() => GetDefaultThemeSettings()).AndThen(s => s.AccentColor).GetValueOr("Green");

        Console.WriteLine($"The user's preferred accent colour is: {accentColour}");

        accentColour = updatedPreferences.Theme.OrElse(() => GetDefaultThemeSettings()).AndThen(s => s.AccentColor).GetValueOr("Green");

        Console.WriteLine($"The user's (updated) preferred accent colour is: {accentColour}");

        var userEmail = userPreferences.ContactMethods.AndThen(cm => cm.Email).GetValueOr("No email was entered");

        Console.WriteLine($"The user's preferred contact email: {userEmail}");

        userPreferences.ContactMethods.Match(() => { }, cm => cm.Email.Match(() => { }, email => SendEmail(email)));

        Option<string> updatedEmail = GetPreferredEmail(updatedPreferences);

        Console.WriteLine($"The user's (updated) preferred contact email: {updatedEmail.GetValueOr("no email was entered")}");

        updatedPreferences.ContactMethods.Match(()=> { }, cm => cm.Email.Match(() => { }, email => SendEmail(email)));

    }
    public static void SendEmail(string emailAddress)
        
        => Console.WriteLine($"Sending email to: {emailAddress}");

    public static ThemeSettings GetDefaultThemeSettings()

        => new(DisplayMode.Light, None.Value);

    public static Option<string> GetPreferredEmail(UserPreferences userPreferences)
        
        => userPreferences.ContactMethods.AndThen(c => c.Email);

    public static UserPreferences GetUserPreferences()
    {
        /*
            * Build preferences from some source.
            * We can use the Options implicit operator to simplify the creation of Option<T> types.
        */
        Option<ThemeSettings>        themeSettings  = null!;//i.e using EFCore dbContext.ThemeSettings.Where(etc).SingleOrDefault();
        Option<ContactMethods>       contactMethods = new ContactMethods(None.Value, "Tel: 12345678");

        return new UserPreferences("en-GB", None.Value, None.Value, themeSettings, contactMethods);
                                            //implicitly set options
    }
    public static UserPreferences GetUpdatedUserPreferences()
    {
        Option<ThemeSettings> themeSettings     = new ThemeSettings(DisplayMode.Dark, "Red");
        Option<ContactMethods> contactMethods   = new ContactMethods("john.doe@gmail.com", "Tel: 12345678");

        return new UserPreferences("en-GB", "GMT", 14, themeSettings, contactMethods);
    }
}
