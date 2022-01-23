using System.Text.Json;
using RecipeManagement.RecipeDataTypes;
using FileSystemProvider;

namespace RecipeManagement.RecipeFileManagement;

public class RecipeFileService : IRecipeFileService
{
    private readonly IFileProvider _provider = new FileProvider();
    private const string FilePath = @"..\..\..\..\recipes.json";
    private static JsonSerializerOptions JsonSerializerOptions { get; } = new() { WriteIndented = true };

    public void WriteRecipes(params Recipe[] recipes)
    {
        var jsonRecipes = string.Empty;
        if (!_provider.Exists(FilePath))
        {
            jsonRecipes = JsonSerializer.Serialize(recipes.ToList(), JsonSerializerOptions);
        }
        else
        {
            var savedRecipes = GetRecipes();
            if (savedRecipes != null)
                jsonRecipes = JsonSerializer.Serialize(recipes, JsonSerializerOptions);
        }
        if (jsonRecipes != null)
        {
            using var outputStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonRecipes));
            _provider.Write(FilePath, outputStream);
        }
        else
        {
            throw new ArgumentException("error while handling params", nameof(recipes));
        }
    }

    public IEnumerable<Recipe>? GetRecipes()
    {
        try
        {
            if (!_provider.Exists(FilePath))
                return null;
            using var inputStream = new StreamReader(_provider.Read(FilePath));
            var jsonRecipes = inputStream.ReadToEnd();
            var returnList = JsonSerializer.Deserialize(jsonRecipes, typeof(List<Recipe>), JsonSerializerOptions) as List<Recipe>;
            return returnList;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
