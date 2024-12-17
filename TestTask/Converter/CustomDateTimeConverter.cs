using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
namespace TestTask.Converter;

public class CustomDateTimeConverter : ITypeConverter
{
    private readonly string[] _dateFormats = {
        "dd/MM/yyyy", "MM/dd/yyyy", "d/M/yyyy", "M/d/yyyy", 
        "dd-MM-yyyy", "MM-dd-yyyy"
    };
    public object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (DateTime.TryParseExact(text, _dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return result;
        }

        throw new FormatException($"Unable to parse '{text}' as a valid date.");
    }
    public string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        return ((DateTime)(value ?? throw new ArgumentNullException(nameof(value)))).ToString("dd/MM/yyyy");
    }
}
