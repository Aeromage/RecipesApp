using System.Text.Json.Serialization;

namespace DataProvider.JsonDataTypes;

//Format of class we get from API
public class ApiFoodNutrient
{
    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    [JsonPropertyName("unitName")]
    public string UnitName { get; set; } = string.Empty;
    [JsonPropertyName("dericationCode")]
    public string DerivationCode { get; set; } = string.Empty;
    [JsonPropertyName("dericationDescription")]
    public string DerivationDescription { get; set; } = string.Empty;
}
