using System.Text.Json.Serialization;

namespace DataProvider.JsonDataTypes;

//Format of class we get from API
public class ApiFoodItem
{
    [JsonPropertyName("fdcId")]
    public int FdcId { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("dataType")]
    public string DataType { get; set; } = string.Empty;
    [JsonPropertyName("publicationDate")]
    public string PublicationDate { get; set; } = string.Empty;
    [JsonPropertyName("ndbNumber")]
    public string NdbNumber { get; set; } = string.Empty;
    [JsonPropertyName("foodNutrients")]
    public List<ApiFoodNutrient> Nutrients { get; set; } = new List<ApiFoodNutrient>();
}
