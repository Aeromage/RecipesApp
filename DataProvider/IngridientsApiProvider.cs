using System.Text.Json;
using DataProvider.FileProvider;
using DataProvider.JsonDataTypes;
using Microsoft.Extensions.Configuration;

namespace DataProvider;

public class IngridientsApiProvider
{
    private readonly HttpClient _httpClient = new();
    private readonly IFileSystemProvider _provider = new FileSystemProvider();
    private const string FilePath = "ingridients.json";

    public IEnumerable<DishIngridient>? GetIngridients()
    {
        if (_provider.Exists(FilePath))
        {
            var inputStream = _provider.Read(FilePath);
            return JsonSerializer.Deserialize(inputStream, typeof(List<DishIngridient>)) as List<DishIngridient>;
        }
        var builder = new ConfigurationBuilder().AddJsonFile("jsconfig.json");
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
            var listIngridients = JsonSerializer.Deserialize(jsonResponse, typeof(List<ApiFoodItem>)) as List<ApiFoodItem>;
            if (listIngridients is not null)
            {
                var formattedList = listIngridients.Select(x => new DishIngridient()
                {
                    Amount = 100,
                    Name = x.Description,
                    Unit = "Grams",
                    Nutrients = x.Nutrients
                                       .Where(n => n.Name is "Total lipid (fat)"
                                       or "Protein"
                                       or "Carbohydrate, by summation"
                                       or "Carbohydrate, by difference")
                                       .Select(n => new IngridientNutrient()
                                       {
                                           Amount = n.Amount,
                                           Name = n.Name,
                                           UnitName = n.UnitName
                                       })
                });
                var formattesJson = JsonSerializer.Serialize(formattedList.ToList(), typeof(List<DishIngridient>));
                _provider.Write(FilePath, new MemoryStream(System.Text.Encoding.UTF8.GetBytes(formattesJson)));
                return formattedList;
            }
        }
        return null;
    }

}
