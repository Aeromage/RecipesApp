using RecipeManagement.RecipeDataTypes;

namespace RecipeManagement.RecipeFileManagement;

public interface IRecipeFileService
{
    void WriteRecipes(params Recipe[] recipes);
    IEnumerable<Recipe>? GetRecipes();
}
