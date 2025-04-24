namespace PracticalFuncyness.ConsoleClient.Common.Models;

internal record class RegistrationEmail(string FirstName, string EmailAddress);
internal class Registration
{
    public required DateTime RegistrationDate { get; set; }
    public required string   FirstName        { get; set; }
    public required string   Surname          { get; set; }
    public required int      Age              { get; set; }
    public required string   EmailAddress     { get; set; }
    public required string   AddressLine      { get; set; }
    public required string   Town             { get; set; }
    public required string   City             { get; set; }
    public required string   County           { get; set; }
    public required string   PostCode         { get; set; }
}
