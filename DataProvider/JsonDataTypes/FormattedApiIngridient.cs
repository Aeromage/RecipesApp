using System.Text.Json.Serialization;

namespace DataProvider.JsonDataTypes;

//Format of class we format to work with it
public class FormattedApiIngridient
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    [JsonPropertyName("unitName")]
    public string Unit { get; set; } = string.Empty;
    [JsonPropertyName("nutrients")]
    public IEnumerable<FormattedApiNutrient> Nutrients { get; set; } = new List<FormattedApiNutrient>();

    public override string ToString()
    {
        return $"{Name}";
    }
}
