using DataProvider.JsonDataTypes;
using RecipeManagement.RecipeDataTypes;

namespace RecipeManagement.RecipeControl;

public interface IRecipeManager
{
    IEnumerable<FormattedApiIngridient> Ingridients { get; }
    void AddRecipe(Recipe recipe);
    void DeleteRecipe(string recipeName);
    void UpdateRecipe(Recipe recipe);
    IEnumerable<Recipe> GetAllRecipes();
    Recipe GetRecipe(int index);
    void SaveRecipes();
}
