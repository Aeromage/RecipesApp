using System.Text.Json.Serialization;

namespace DataProvider.JsonDataTypes;

//Format of class we format to work with it
public class FormattedApiNutrient
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    [JsonPropertyName("unitName")]
    public string UnitName { get; set; } = string.Empty;
}
