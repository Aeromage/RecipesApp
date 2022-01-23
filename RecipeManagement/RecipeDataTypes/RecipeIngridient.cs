using System.Text.Json.Serialization;
using DataProvider.JsonDataTypes;

namespace RecipeManagement.RecipeDataTypes;


public class RecipeIngridient
{
    [JsonPropertyName("ingridient")]
    public FormattedApiIngridient Ingridient { get; set; }
    //Amount in grams
    [JsonPropertyName("ingridientAmount")]
    public double Amount { get; set; }

    public RecipeIngridient()
    {
        Ingridient = new FormattedApiIngridient();
        Amount = 0;
    }

    public RecipeIngridient(FormattedApiIngridient ingridient, double amount)
    {
        Ingridient = ingridient;
        Amount = amount;
    }

    public KeyValuePair<string, double> GetFats()
    {
        var nutrient = Ingridient.Nutrients.FirstOrDefault(x => x.Name == Recipe.Fats);
        var fats = nutrient is null
            ? 0
            : nutrient.Amount / Recipe.StandartPortion * Amount;
        return new KeyValuePair<string, double>(Recipe.Fats, fats);
    }

    public KeyValuePair<string, double> GetProtein()
    {
        var nutrient = Ingridient.Nutrients.FirstOrDefault(x => x.Name.Contains(Recipe.Carbo));
        var prot = nutrient is null
            ? 0
            : nutrient.Amount / Recipe.StandartPortion * Amount;
        return new KeyValuePair<string, double>(Recipe.Protein, prot);
    }

    public KeyValuePair<string, double> GetCarbs()
    {
        var nutrient = Ingridient.Nutrients.FirstOrDefault(x => x.Name == Recipe.Protein);
        var prot = nutrient is null
            ? 0
            : nutrient.Amount / Recipe.StandartPortion * Amount;
        return new KeyValuePair<string, double>(Recipe.Protein, prot);
    }

}
