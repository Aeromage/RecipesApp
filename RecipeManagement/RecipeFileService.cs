using System.Text.Json;
using RecipeManagement.RecipeDataTypes;
using FileSystemProvider;

namespace RecipeManagement;

public class RecipeFileService
{
    private readonly IFileProvider _provider = new FileProvider();
    private const string FilePath = @"..\..\..\..\recipes.json";

    public void WriteRecipes(params Recipe[] recipes)
    {
        var jsonRecipes = string.Empty;
        if (!_provider.Exists(FilePath))
        {
            jsonRecipes = JsonSerializer.Serialize(recipes.ToList());
        }
        else
        {
            var savedRecipes = GetRecipes();
            if (savedRecipes != null)
            {
                jsonRecipes = JsonSerializer.Serialize(savedRecipes.Union(recipes).ToList());
            }
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
            using var inputStream = new StreamReader(_provider.Read(FilePath));
            var jsonRecipes = inputStream.ReadToEnd();
            return JsonSerializer.Deserialize(jsonRecipes, typeof(List<Recipe>)) as List<Recipe>;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}
