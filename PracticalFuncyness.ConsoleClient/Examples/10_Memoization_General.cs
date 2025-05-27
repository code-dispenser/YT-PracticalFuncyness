using PracticalFuncyness.ConsoleClient.Common.Models;
using PracticalFuncyness.ConsoleClient.Common.Utils;
using System.Diagnostics;
using System.Net.Http.Json;

namespace PracticalFuncyness.ConsoleClient.Examples;

internal class Memoization_General
{
    public static async Task Run()
    {
        await Console.Out.WriteLineAsync(GeneralUtils.CreateHeading("10 - Memoization General"));

        /*
            * To keep things simple I am not using the IHttpClientFactory here, but you should in production code. 
            * 
            * To negate adding separate packages for say MemoryCache we can use our memoization utility and memoize functions to cache the results in the functions.
            * Perhaps for API's where you are limited to a number of requests per minute, or where the data does not change often.
        */
        
        HttpClient httpClient = new HttpClient();
      
        string londonUrl    = "https://api.open-meteo.com/v1/forecast?latitude=51.5074&longitude=-0.1278&current=temperature_2m,weathercode";  //London coordinates  
        string parisUrl     = "https://api.open-meteo.com/v1/forecast?latitude=48.8566&longitude=2.3522&current=temperature_2m,weathercode";   //Paris coordinates
        string newYorkUrl   = "https://api.open-meteo.com/v1/forecast?latitude=40.7143&longitude=-74.006&current=temperature_2m,weathercode"; //New York coordinates
        
        var memoizedWeather = MemoizeUtils.Memoize<string, string>(url => GetCurrentWeather(httpClient,url), expireInMins: 2);

        Stopwatch stopwatch = Stopwatch.StartNew();

        Task[] weatherTasks = Enumerable.Range(0, 200).SelectMany(_ => new Task<String>[] { memoizedWeather(londonUrl), memoizedWeather(parisUrl), memoizedWeather(newYorkUrl) }).ToArray();

        await Task.WhenAll(weatherTasks); 

        var londonWeather   = await memoizedWeather(londonUrl);//Call number 601 - limit exceeded without cache.
        var parisWeather    = await memoizedWeather(parisUrl);
        var newYorkWeather  = await memoizedWeather(newYorkUrl);

        stopwatch.Stop();

        Console.WriteLine($"London:\t\t {londonWeather}\r\nParis:\t\t {parisWeather}\r\nNew York:\t {newYorkWeather}\r\n\r\n603 memoized function calls, 3 of which across the internet to the free weather API took: {stopwatch.ElapsedMilliseconds}ms");
    }

    public static async Task<string> GetCurrentWeather(HttpClient httpClient, string weatherUrl)
    {
        var weatherResult = "We are sorry, but this data is currently unavailable.";
        try
        {
            var weatherResponse     = (await httpClient.GetFromJsonAsync<WeatherResponse>(weatherUrl))!;
            var weatherDescription  = GeneralUtils.BreakOnCaps(((WeatherCode)weatherResponse.Current.Weathercode).ToString()).ToLower();

            return $"The weather is {weatherDescription} with a temperature of {weatherResponse!.Current.Temperature_2m}{weatherResponse.Current_Units.Temperature_2m}";
        }
        catch
        {
            return weatherResult; 
        }
    }
    /*
        * For those who watched the video on Partial Application, you could use the GetCurrentWeatherWithClient method to create a function that is bound to a specific HttpClient instance. 
     */
    public static Func<string, Task<string>> GetCurrentWeatherWithClient(HttpClient httpClient)
    
        => weatherUrl => GetCurrentWeather(httpClient, weatherUrl);
    /*
         * And memoize that instead:
          
         *  var weatherClient   = GetCurrentWeatherWithClient(httpClient);
         *  var memoizedWeather = MemoizeUtils.Memoize<string, string>(url => weatherClient(url));
    */

}