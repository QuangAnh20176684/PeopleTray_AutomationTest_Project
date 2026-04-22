using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;


namespace PeoTest;

public class validateHelper
{
    public static async Task<bool> IsValuePresent(List<string> valueList, string value)
    // check all data from list contain the filter value
    {
        foreach (var item in valueList)
        {
            Console.WriteLine($"Checking value: {item} against filter: {value}");
            if (item.Contains(value, StringComparison.OrdinalIgnoreCase))
            {
                continue;

            }
            else
            {
                return false;
            }
        }
        return true;

    }
    public static async Task<bool> IsValuePresentExactly(List<string> valueList, string value)
    // check all data from list match exactly the filter value (strict mode safe)
    {
        foreach (var item in valueList)
        {
            Console.WriteLine($"Checking value: {item} against filter: {value}");
            if (item.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;

    }
    public static async Task<bool> IsNodataPresent(List<string> valueList)
    // check all data from list contain "No data available in table"
    {
        foreach (var value in valueList)
        {
            Console.WriteLine($"Checking value: {value} against filter: ");
            if (value.Contains("No data available in table", StringComparison.OrdinalIgnoreCase))
            {
                continue;

            }
            else
            {
                return false;
            }
        }
        return true;

    }
    // public static async Task<bool> IsEquipmentPresent(List<string> equipmentList, string equipmentName)
    // {
    //     foreach (var equipment in equipmentList)
    //     {
    //         Console.WriteLine($"Checking equipment: {equipment} against filter: {equipmentName}");
    //         if (equipment.Contains(equipmentName, StringComparison.OrdinalIgnoreCase))
    //         {
    //             continue;

    //         }else
    //         {
    //             return false;
    //         }
    //     }
    //     return true;

    // }
    public static async Task<bool> IsDateInRange(List<string> dateList, string startDate, string endDate)
    {
        DateTime start = DateTime.Parse(startDate);
        DateTime end = DateTime.Parse(endDate);
        foreach (var date in dateList)
        {
            Console.WriteLine($"Checking date: {date} against filter: {startDate} to {endDate}");
            DateTime parsedDate = DateTime.Parse(date);
            if (DateTime.TryParse(date, out parsedDate))
            {
                if (parsedDate >= start && parsedDate <= end)
                {
                    continue;
                }
            }
            return false;
        }
        return true;

    }
    public static async Task<bool> IsValuePresentExactly<T>(List<T> valueList, string columnName, string value)
    {
        foreach (var item in valueList)
        {
            var property = item.GetType().GetProperty(columnName);
            if (property != null)
            {
                var propertyValue = property.GetValue(item)?.ToString();
                Console.WriteLine($"Checking value: {propertyValue} against filter: {value}");
                if (propertyValue.Equals(value, StringComparison.Ordinal))
                {
                    
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
    public static async Task<bool> IsValuePresent<T>(List<T> valueList, string columnName, string value)
    {
        foreach (var item in valueList)
        {
            var property = item.GetType().GetProperty(columnName);
            if (property != null)
            {
                var propertyValue = property.GetValue(item)?.ToString();
                Console.WriteLine($"Checking value: {propertyValue} against filter: {value}");
                if (propertyValue.Contains(value, StringComparison.OrdinalIgnoreCase))
                {
                    
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
    // public static async Task<bool> IsNoDataPresent<T>(List<T> valueList)
    // {
    //     foreach (var item in valueList)
    //     {
    //         var property = item.GetType().GetProperty(columnName);
    //         if (property != null)
    //         {
    //             var propertyValue = property.GetValue(item)?.ToString();
    //             Console.WriteLine($"Checking value: {propertyValue} against filter: {string.Join(", ", filterValues)}");
    //             if (filterValues.Any(filter => propertyValue.Contains(filter, StringComparison.OrdinalIgnoreCase)))
    //             {
    //                 continue;
    //             }
    //             else
    //             {
    //                 return false;
    //             }
    //         }
    //     }
    //     return true;
    // }



}