namespace RecipeManagement.RecipeDataTypes;

public class Recipe
{
    [NonSerialized()] public const string Carbo = "Carbohydrates";
    [NonSerialized()] public const string Fats = "Total lipid (fat)";
    [NonSerialized()] public const string Protein = "Protein";
    //Carbs, fats and protein are calculated for 100 grams
    [NonSerialized()] public const double StandartPortion = 100D;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RecipeIngridient> Ingridients { get; set; } = new();

    public Dictionary<string, double> GetNutritionalValue()
    {
        var fats = Ingridients.Select(x => x.GetFats()).Select(x => x.Value).Sum();
        var carbs = Ingridients.Select(x => x.GetCarbs()).Select(x => x.Value).Sum();
        var prot = Ingridients.Select(x => x.GetProtein()).Select(x => x.Value).Sum();
        return new Dictionary<string, double>()
        {
            {Fats, fats},
            {Protein, prot},
            {Carbo, carbs}
        };
    }
}

