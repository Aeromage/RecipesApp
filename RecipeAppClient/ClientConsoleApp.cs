using RecipeManagement.RecipeControl;
using RecipeManagement.RecipeDataTypes;

namespace RecipeAppClient;

public class ClientConsoleApp
{
    private IRecipeManager Manager { get; }

    public ClientConsoleApp()
    {
        Manager = new RecipeManager();
    }

    public void Menu()
    {
        Console.WriteLine("Choose what you wanna do");
        Console.WriteLine("1 - Add recipe");
        Console.WriteLine("2 - Delete recipe");
        Console.WriteLine("3 - Edit recipe");
        Console.WriteLine("4 - Get all recipes");
        Console.WriteLine("5 - Exit app");
        var option = GetNumberFromUser("choose", 1, 2, 3, 4, 5);
        switch (option)
        {
            case 1:
                AddNewRecipe();
                break;
            case 2:
                DeleteRecipe();
                break;
            case 3:
                EditRecipe();
                break;
            case 4:
                GetAllRecipes();
                break;
            case 5:
                Console.WriteLine("Thank`s for using");
                Sleep();
                Environment.Exit(0);
                break;
            default:
                Menu();
                break;
        }
    }

    public void Startup()
    {
        Console.WriteLine("it`s recipe`s app");
        if (!Manager.GetAllRecipes().Any())
        {
            Console.WriteLine("Now it`s empty");
            var message = "Wanna add your first recipe?\n1 - Yes, 2 - No, exit";
            var answer = GetNumberFromUser(message, 1, 2);
            switch (answer)
            {
                case 2:
                    Console.WriteLine("OK, see you soon");
                    Environment.Exit(0);
                    break;
                case 1:
                    AddNewRecipe();
                    break;
                default:
                    break;
            }
        }
        else
        {
            Menu();
        }
    }

    public void AddNewRecipe()
    {
        var name = GetStringFromUser("Right down name of your dish");
        if (Manager.GetAllRecipes().Any(x => CompareString(name, x.Name)))
        {
            Console.WriteLine("Recipe with that name is already exist");
            Sleep();
            AddNewRecipe();
        }
        var desc = GetStringFromUser("Right down description of your dish");
        Console.WriteLine("Here ingridients you can choose");
        var ingridients = GetIngridientsFromUser().ToList();
        Manager.AddRecipe(new Recipe() { Description = desc, Name = name, Ingridients = ingridients });
        Manager.SaveRecipes();
        Startup();
    }

    public RecipeIngridient ChooseIngridient()
    {
        ShowCollection(Manager.Ingridients);
        var index = GetNumberFromUser("Choose number of ingridient", Enumerable.Range(1, Manager.Ingridients.Count()).ToArray());
        var ingridient = new RecipeIngridient() { Ingridient = Manager.Ingridients.ElementAt(index - 1) };
        var weight = GetNumberFromUser("Enter weight of product you need to use", Enumerable.Range(1, 10000).ToArray());
        ingridient.Amount = weight;
        return ingridient;
    }

    public void DeleteRecipe()
    {
        ShowCollection(Manager.GetAllRecipes());
        var index = GetNumberFromUser("Enter number of recipe to delete", 1, Manager.GetAllRecipes().Count());
        var agreement = GetNumberFromUser("Are you sure?\n1 - No\n2 - Yes", 1, 2);
        switch (agreement)
        {
            case 1:
                Startup();
                break;
            case 2:
                Manager.DeleteRecipe(Manager.GetRecipe(index - 1).Name);
                break;
            default:
                break;
        }
        Manager.SaveRecipes();
        Startup();
    }

    public void EditRecipe()
    {
        ShowCollection(Manager.GetAllRecipes());
        var index = GetNumberFromUser("Choose number of recipe to edit", Enumerable.Range(1, Manager.GetAllRecipes().Count()).ToArray());
        var whatToEdit = GetNumberFromUser("Choose what you wanna change\n1 - Name\n2 - Description\n3 - Ingridients", 1, 2, 3);
        var recipeToEdit = Manager.GetAllRecipes().ToList()[index - 1];
        switch (whatToEdit)
        {
            case 1:
                var newName = string.Empty;
                var isExist = true;
                while (isExist)
                {
                    newName = GetStringFromUser("Right down new name of recipe");
                    if (!Manager.GetAllRecipes().ToList().Any(x => CompareString(newName, x.Name)))
                    {
                        isExist = false;
                    }
                    else
                    {
                        Console.WriteLine("Recipe with this name is alredy exist");
                    }
                }
                recipeToEdit.Name = newName;
                break;
            case 2:
                var newDesc = GetStringFromUser("Right down new description of recipe");
                recipeToEdit.Description = newDesc;
                break;
            case 3:
                recipeToEdit.Ingridients = GetIngridientsFromUser().ToList();
                break;
            default:
                break;
        }
        Startup();
    }

    public int GetNumberFromUser(string message, params int[] validValues)
    {
        Console.WriteLine(message);
        if (int.TryParse(Console.ReadLine(), out var answer))
        {
            if (!validValues.Contains(answer))
            {
                Console.WriteLine("Wrong input!!");
                Sleep();
                return GetNumberFromUser(message, validValues);
            }
            return answer;
        }
        else
        {
            Console.WriteLine("Wrong input!!");
            Sleep();
            return GetNumberFromUser(message, validValues);
        }
    }

    public string GetStringFromUser(string message)
    {
        Console.WriteLine(message);
        var messageFromUser = Console.ReadLine();
        if (messageFromUser is null || string.IsNullOrWhiteSpace(messageFromUser))
        {
            Console.WriteLine("Right dowm something");
            return GetStringFromUser(message);
        }
        return messageFromUser;
    }

    public void Sleep()
    {
        Thread.Sleep(2000);
    }

    public void ShowCollection<T>(IEnumerable<T> collection)
    {
        for (var i = 0; i < collection.Count(); i++)
        {
            Console.WriteLine($"{i + 1} - {collection.ToList()[i]}");
        }
    }

    public IEnumerable<RecipeIngridient> GetIngridientsFromUser()
    {
        var ingridients = new List<RecipeIngridient>();
        int answer;
        do
        {
            var newIngridient = ChooseIngridient();
            if (ingridients.Any(x => CompareString(x.Ingridient.Name, newIngridient.Ingridient.Name)))
            {
                Console.WriteLine("It`s already in");
            }
            else
            {
                ingridients.Add(newIngridient);
            }
            answer = GetNumberFromUser("1 - add another ingridient, 2 - exit and add recipe to collection", 1, 2);
            if (answer is 2 && !ingridients.Any())
            {
                Console.WriteLine("Add some ingridients!");
                answer = 1;
            }
        } while (answer is not 2);
        return ingridients;
    }

    public bool CompareString(string first, string second)
    {
        return StringComparer.OrdinalIgnoreCase.Equals(first, second);
    }

    public void GetAllRecipes()
    {
        Console.WriteLine("Choose option");
        Console.WriteLine("1 - Name");
        Console.WriteLine("2 - Nutritional value");
        var answer = GetNumberFromUser("Write, pls", 1, 2);
        switch (answer)
        {
            case 1:
                foreach (var recipe in Manager.GetAllRecipes().OrderBy(x => x.Name))
                {
                    Console.WriteLine($"{recipe.Name} - {recipe.Description}");
                    foreach (var ingridient in recipe.Ingridients)
                    {
                        Console.WriteLine($"{ingridient.Ingridient.Name} - {ingridient.Amount} {ingridient.Ingridient.Unit}");
                    }
                }
                break;
            case 2:
                foreach (var recipe in Manager.GetAllRecipes())
                {
                    Console.WriteLine($"{recipe.Name} - {recipe.Description}");
                    foreach (var ingridient in recipe.Ingridients)
                    {
                        Console.WriteLine($"{ingridient.Ingridient.Name} - {ingridient.Amount} {ingridient.Ingridient.Unit}");
                    }
                    foreach (var nutrition in recipe.GetNutritionalValue())
                    {
                        Console.WriteLine($"{nutrition.Key} - {nutrition.Value} grams");
                    }
                }
                break;
            default:
                Menu();
                break;
        }
        Startup();
    }
}
