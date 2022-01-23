using DataProvider;
using DataProvider.JsonDataTypes;
using RecipeManagement.RecipeDataTypes;
using RecipeManagement.RecipeFileManagement;

namespace RecipeManagement.RecipeControl;

public class RecipeManager : IRecipeManager
{
    private IEnumerable<Recipe> Recipes { get; set; }
    private IRecipeFileService FileService { get; set; }

    public IEnumerable<FormattedApiIngridient> Ingridients { get; }

    public RecipeManager()
    {
        FileService = new RecipeFileService();
        try
        {
            Recipes = FileService.GetRecipes() ?? new List<Recipe>();
        }
        catch (Exception)
        {
            Recipes = new List<Recipe>();
        }
        Ingridients = new IngridientsApiProvider().GetIngridients() ?? new List<FormattedApiIngridient>();
    }

    public void AddRecipe(Recipe recipe)
    {
        if (recipe == null)
            throw new ArgumentNullException(nameof(recipe));
        if (Recipes.Any(x => x.Name == recipe.Name))
            throw new ArgumentException("recipe is already in recipe`s book");
        Recipes = Recipes.Append(recipe);
    }

    public void DeleteRecipe(string recipeName)
    {
        if (recipeName == null)
            throw new ArgumentNullException(nameof(recipeName));
        if (!Recipes.Any(x => x.Name == recipeName))
            throw new ArgumentException($"no {recipeName} in recipe`s book");
        var newCollection = Recipes.ToList();
        _ = newCollection.RemoveAll(x => x.Name == recipeName);
        Recipes = newCollection;
    }

    public IEnumerable<Recipe> GetAllRecipes()
    {
        return Recipes;
    }

    public Recipe GetRecipe(int index)
    {
        return index < 0 || index >= Recipes.Count()
            ? throw new ArgumentOutOfRangeException(nameof(index))
            : Recipes.ElementAt(index);
    }

    public void SaveRecipes()
    {
        FileService.WriteRecipes(Recipes.ToArray());
    }

    public void UpdateRecipe(Recipe recipe)
    {
        if (recipe is null)
            throw new ArgumentNullException(nameof(recipe));
        if (!Recipes.Any(x => x.Name == recipe.Name))
            throw new ArgumentException("there`s no recipe to update", nameof(recipe));
        DeleteRecipe(recipe.Name);
        AddRecipe(recipe);
    }
}
