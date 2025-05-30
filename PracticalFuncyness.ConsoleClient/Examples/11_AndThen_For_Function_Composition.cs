using PracticalFuncyness.ConsoleClient.Common.Extensions;
using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Utils;

using static PracticalFuncyness.ConsoleClient.Common.Extensions.FunctionalExtensions;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class AndThen_For_Function_Composition
{
    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("11 - AndThen for Function Composition"));

        var person = new Person(new("John"), new ("Doe"), new (new DateOnly(1960, 1, 1)));
        
        const decimal basePrice = 150.00m;

        /*
            * Compose a pipeline of functions left-to-right using AndThen (a functional programming concept).
            * Instead of nested calls like:
            *     ApplyDiscount(basePrice, GetDiscountRate(DetermineAgeBand(CalculateAge(dob, currentDate))))
            * ...which read right-to-left (innermost function is called first),
            * we build a clear, readable pipeline using partial application:
            * 
            *     currentDate -> Age -> AgeBand -> DiscountRate -> FinalPrice
            *
            * `ageFromCurrentDate` is a partially applied function that captures the person's DOB.
            * The rest of the pipeline transforms the age into a final discounted price.
        */


        Func<DateOnly, Age> ageFromCurrentDate = currentDate => CalculateAge(person.DateOfBirth, currentDate);
        
        var quoteCalculator = ageFromCurrentDate
                                .AndThen(DetermineAgeBand)
                                    .AndThen(GetDiscountRate)
                                        .AndThen(discount => ApplyDiscount(basePrice, discount));
        
        var finalPrice = quoteCalculator(DateOnly.FromDateTime(DateTime.Now));

        Console.WriteLine($"The quotation for {person.FirstName.Value} {person.Surname.Value} is priced at: {finalPrice:C2}\r\n");
    }


    public static Age CalculateAge(DateOfBirth dateOfBirth, DateOnly currentDate)

        => new(currentDate.Year - dateOfBirth.Value.Year - (currentDate.DayOfYear < dateOfBirth.Value.DayOfYear ? 1 : 0));

    // Map Age to an AgeBand (e.g., Child, Adult)
    public static AgeBand DetermineAgeBand(Age personAge)

        => personAge.Value switch
        {
            < 13 => AgeBand.Child,
            < 20 => AgeBand.Teenager,
            < 65 => AgeBand.Adult,
            _    => AgeBand.Senior
        };

    // Determine discount rate from AgeBand
    public static decimal GetDiscountRate(AgeBand band) 

        =>  band switch
        {
            AgeBand.Child    => 0.50m,
            AgeBand.Teenager => 0.25m,
            AgeBand.Adult    => 0.0m,
            _                => 0.3m
        };

    // Apply the discount rate to the base price
    public static decimal ApplyDiscount(decimal basePrice, decimal discountRate)
        
        => basePrice * (1 - discountRate);

    
}