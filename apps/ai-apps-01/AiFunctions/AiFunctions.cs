using System.ComponentModel;

namespace ai_apps_01;

public class AiFunctions
{
    [Description("Get the current date and time in a specified time zone using a valid system time zone ID. If not provided, UTC is used.")]
    public static string GetCurrentTime(
        [Description("A valid system time zone ID, such as UTC, Europe/Berlin, or America/New_York. Defaults to UTC.")] string timezone = "UTC")
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timezone);
        var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
        return $"Current time in {timezone}: {time:f}";
    }
    
    [Description("Calculate the tip amount and final total for a bill based on a given tip percentage.")]
    public static string CalculateTip(
        [Description("The original bill amount before tip. Must be greater than zero.")] double billAmount,
        [Description("The tip percentage to apply to the bill amount. Defaults to 18 if not provided. Must be greater than zero.")] double tipPercentage = 18)
    {
        if (billAmount <= 0)
            throw new ArgumentException("billAmount must be greater than zero");

        if (tipPercentage <= 0)
            throw new ArgumentException("tipPercentage must be greater than zero");

        var tip = billAmount * (tipPercentage / 100);
        var total = billAmount + tip;
        return $"Bill: ${billAmount:F2}, Tip ({tipPercentage}%): ${tip:F2}, Total: ${total:F2}";
    }
    
    [Description("Convert an amount from one currency code to another using predefined exchange rates. Supported currencies: USD, EUR, GBP, JPY.")]
    public static string ConvertCurrency(
        [Description("The amount of money to convert. Must be greater than zero.")] double amount,
        [Description("The source currency code, for example USD, EUR, GBP, or JPY.")] string fromCurrency,
        [Description("The target currency code, for example USD, EUR, GBP, or JPY.")] string toCurrency)    
    {
        if(amount <= 0)
            throw new ArgumentException("amount must be greater than zero");
        
        if(string.IsNullOrEmpty(fromCurrency))
            throw new ArgumentException("from must be specified");
        
        if(string.IsNullOrEmpty(toCurrency))
            throw new ArgumentException("to must be specified");
        
        fromCurrency = fromCurrency.Trim().ToUpperInvariant();
        toCurrency = toCurrency.Trim().ToUpperInvariant();
        
        // Simplified - in reality, you'd call an exchange rate API
        var rates = new Dictionary<string, double>
        {
            ["USD"] = 1.0,
            ["EUR"] = 0.85,
            ["GBP"] = 0.73,
            ["JPY"] = 110.0
        };
        
        if (!rates.ContainsKey(fromCurrency))
            throw new ArgumentException($"Unsupported source currency: {fromCurrency}");

        if (!rates.ContainsKey(toCurrency))
            throw new ArgumentException($"Unsupported target currency: {toCurrency}");
    
        var usdAmount = amount / rates[fromCurrency];
        var result = usdAmount * rates[toCurrency];
        return $"{amount} {fromCurrency} = {result:F2} {toCurrency}";
    }
}