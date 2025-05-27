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

public class WeatherResponse
{
    public double Latitude              { get; set; }
    public double Longitude             { get; set; }
    public double GenerationtimeMs      { get; set; }
    public int    UtcOffsetSeconds      { get; set; }
    public string Timezone              { get; set; }
    public string TimezoneAbbreviation  { get; set; }
    public double Elevation             { get; set; }
    public CurrentUnits   Current_Units { get; set; }
    public CurrentWeather Current       { get; set; }
}

public class CurrentUnits
{
    public string Time { get; set; }
    public string Interval { get; set; }
    public string Temperature_2m { get; set; }
    public string Weathercode { get; set; }
}

public class CurrentWeather
{
    public string Time { get; set; }
    public int Interval { get; set; }
    public double Temperature_2m { get; set; }
    public int Weathercode { get; set; }
}


public enum WeatherCode
{
    ClearSky                    = 0,
    MainlyClear                 = 1,
    PartlyCloudy                = 2,
    Overcast                    = 3,
    Fog                         = 45,
    DepositingRimeFog           = 48,
    LightDrizzle                = 51,
    ModerateDrizzle             = 53,
    DenseDrizzle                = 55,
    LightFreezingDrizzle        = 56,
    DenseFreezingDrizzle        = 57,
    SlightRain                  = 61,
    ModerateRain                = 63,
    HeavyRain                   = 65,
    LightFreezingRain           = 66,
    HeavyFreezingRain           = 67,
    SlightSnowFall              = 71,
    ModerateSnowFall            = 73,
    HeavySnowFall               = 75,
    SnowGrains                  = 77,
    SlightRainShowers           = 80,
    ModerateRainShowers         = 81,
    ViolentRainShowers          = 82,
    SlightSnowShowers           = 85,
    HeavySnowShowers            = 86,
    Thunderstorm                = 95,
    ThunderstormWithSlightHail  = 96,
    ThunderstormWithHeavyHail   = 99
}