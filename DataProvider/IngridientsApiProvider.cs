using System.Text.Json;
using DataProvider.JsonDataTypes;
using FileSystemProvider;
using Microsoft.Extensions.Configuration;

namespace DataProvider;

public class IngridientsApiProvider
{
    private readonly HttpClient _httpClient = new();
    private readonly IFileProvider _provider = new FileProvider();
    private const string FilePath = @"..\..\..\..\ingridients.json";
    private const string CfgFilePath = @"..\..\..\..\config.json";
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new() { WriteIndented = true };

    public IEnumerable<FormattedApiIngridient>? GetIngridients()
    {
        if (_provider.Exists(FilePath))
        {
            var inputStream = _provider.Read(FilePath);
            return JsonSerializer.Deserialize(inputStream, typeof(List<FormattedApiIngridient>), JsonSerializerOptions) as List<FormattedApiIngridient>;
        }
        var builder = new ConfigurationBuilder().AddJsonFile(Path.GetFullPath(CfgFilePath));
        var cfg = builder.Build();
        var endPoint = cfg["endPoint"];
        var response = _httpClient.GetAsync(endPoint).Result;
        if (response != null)
        {
            if ((int)response.StatusCode is >= 400 and <= 499)
            {
                return null;
            }
            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var listIngridients = JsonSerializer.Deserialize(jsonResponse, typeof(List<ApiFoodItem>), JsonSerializerOptions) as List<ApiFoodItem>;
            if (listIngridients is not null)
            {
                var formattedList = listIngridients.Select(x => new FormattedApiIngridient()
                {
                    Amount = 100,
                    Name = x.Description,
                    Unit = "Grams",
                    Nutrients = x.Nutrients.Where(n => n.Name is "Total lipid (fat)"
                                           or "Protein"
                                           or "Carbohydrate, by summation"
                                           or "Carbohydrate, by difference")
                                           .Select(n => new FormattedApiNutrient()
                                           {
                                               Amount = n.Amount,
                                               Name = n.Name,
                                               UnitName = n.UnitName
                                           })
                });
                var formattesJson = JsonSerializer.Serialize(formattedList.ToList(), typeof(List<FormattedApiIngridient>), JsonSerializerOptions);
                _provider.Write(FilePath, new MemoryStream(System.Text.Encoding.UTF8.GetBytes(formattesJson)));
                return formattedList;
            }
        }
        return null;
    }

}
