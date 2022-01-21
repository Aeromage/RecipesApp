using DataProvider;
using RecipeManagement;
using RecipeManagement.RecipeDataTypes;

var service = new RecipeFileService();
var ingridients = new IngridientsApiProvider().GetIngridients();
var ingridientsWithAmount = new List<RecipeIngridient>
{
    new RecipeIngridient(ingridients.First(), 50),
    new RecipeIngridient(ingridients.Last(), 50)
};
var recipe = new Recipe() { Description = "aaa", Ingridients = ingridientsWithAmount, Name = "Vkusna" };
service.WriteRecipes(recipe);
