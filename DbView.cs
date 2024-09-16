using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Recipe_Browser;

public partial class DbView : UserControl
{
    private List<Recipe> SelectedRecipes = [];
    private int SelectedIndex = 0;
    private static readonly RecipeManager RM = RecipeManager.Instance;

    public DbView()
    {
        InitializeComponent();
    }


    public void SaveEntry(object sender, RoutedEventArgs args){
        Recipe SelectedRecipe = SelectedRecipes[SelectedIndex];
            if (SelectedRecipe == null){
                Recipe newRecipe = new();
                if (CreateRecipeObj(newRecipe) == 0){
                    RecipeManager.DB.Add(newRecipe);
                    RecipeManager.DB.SaveChanges();
                }
            } else{
                if (CreateRecipeObj(SelectedRecipe) == 0){
                    RecipeManager.DB.Update(SelectedRecipe);
                    RecipeManager.DB.SaveChanges();
                }
            }
            ResetForm(sender, args);
        }


   public void ChangeRecipe(object sender, RoutedEventArgs args){
        if (args.Source is not Control source || SelectedRecipes.Count < 1)
        {
            return;
        }
        switch (source.Name){
            case "PrevRecipeButton":
                if(SelectedIndex == 0){
                    SelectedIndex = SelectedRecipes.Count - 1;
                } else {SelectedIndex--;}
                LoadRecipe();
                return;
            case "NextRecipeButton":
                if (SelectedIndex == SelectedRecipes.Count - 1){
                    SelectedIndex = 0;
                } else {SelectedIndex++;}
                LoadRecipe();
                return;
        }
    }


    public int CreateRecipeObj(Recipe Obj){
        if (!(string.IsNullOrEmpty(RecipeNameInput.Text) || string.IsNullOrEmpty(BookNameInput.Text) || string.IsNullOrEmpty(IngredientsInput.Text) || string.IsNullOrEmpty(InstructionsInput.Text) || string.IsNullOrEmpty(NotesInput.Text) || string.IsNullOrEmpty(PhotosInput.Text))){
            Obj.Name = RecipeNameInput.Text;
            Obj.Book = BookNameInput.Text;
            Obj.Instructions = InstructionsInput.Text;
            Obj.Notes = NotesInput.Text;
            Obj.Photos = PhotosInput.Text;
            Obj.Ingredients.Clear();

            foreach (string ingredient in IngredientsInput.Text.Split("\r\n")){
                string[] ingredientData = ingredient.Split(" ");
                if (ingredientData.Length > 3){
                    for (int index = 3; index < ingredientData.Length; index++){
                        ingredientData[2] = ingredientData[2] + " " + ingredientData[index];
                    }
                }
                if (int.TryParse(ingredientData[0], out int ingredientAmount))
                {
                    Ingredient newIngredient = new()
                    {
                        Amount = ingredientAmount,
                        Measurement = ingredientData[1],
                        Name = ingredientData[2]
                    };
                    Obj.Ingredients.Add(newIngredient);
                }
                else {
                    Console.WriteLine("Invalid Ingredient Entry");
                    Console.WriteLine(ingredientData[0] + " " + ingredientData[1] + " " + ingredientData[2]);
                    return 1;
                }
            }
        }
        else {
            Console.WriteLine("Null Field Detected");
            return 1;
        }
        return 0;
    }

    public void ResetForm(object sender, RoutedEventArgs args){
        SelectedRecipes.Clear();
        DbSearch.Text = null;
        RecipeNameInput.Text = null;
        BookNameInput.Text = null;
        IngredientsInput.Text = null;
        InstructionsInput.Text = null;
        NotesInput.Text = null;
        PhotosInput.Text = null;
    }

    public void SearchRecipes(object sender, KeyEventArgs args){
        if (args.Key != Key.Return || String.IsNullOrEmpty(DbSearch.Text)){
            return;
        }
        // Editor prefers String.Contains(String, StringComparison), however EF Framework Core cannot parse that, hence this is done instead
        List<Recipe> queryResult = [.. RecipeManager.DB.Recipes.Where(recipe => recipe.Name.ToUpper().Contains(DbSearch.Text.ToUpper()))];
        if (queryResult.Count > 0){
            SelectedRecipes = queryResult;
            SelectedIndex = 0;
            LoadRecipe();
        }
    }

    private void LoadRecipe(){
        if (SelectedRecipes.Count < 1){
            return;
        }
        Recipe SelectedRecipe = SelectedRecipes[SelectedIndex];
        RecipeNameInput.Text = SelectedRecipe.Name;
        BookNameInput.Text = SelectedRecipe.Book;
        InstructionsInput.Text = SelectedRecipe.Instructions;
        NotesInput.Text = SelectedRecipe.Notes;
        PhotosInput.Text = SelectedRecipe.Photos;

        RecipeManager.DB.Entry(SelectedRecipe)
            .Collection(recipe => recipe.Ingredients)
            .Load();
        IngredientsInput.Text = "";
        foreach (Ingredient ingredient in SelectedRecipe.Ingredients){
            IngredientsInput.Text += ingredient.Amount + " " + ingredient.Measurement + " " + ingredient.Name + "\n";
        }        
    }

    public void DeleteEntry(object sender, RoutedEventArgs args){
        if (SelectedRecipes[SelectedIndex] != null){
            RecipeManager.DB.Remove(SelectedRecipes[SelectedIndex]);
            RecipeManager.DB.SaveChanges();
        }
        ResetForm(sender, args);
    }

    public void UpdateImagePath(object sender, KeyEventArgs args){
        if (args.Key != Key.Return || String.IsNullOrEmpty(ImageFolderInput.Text)){
            return;
        }
        string? folderPath = ImageFolderInput.Text;
        if (!folderPath.EndsWith('/') || !folderPath.EndsWith('\\')){
            folderPath += "/";
        }
        if (Directory.Exists(folderPath)){
            RecipeManager.Instance.ImageFolderPath = folderPath;
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            string SettingsPath = Path.Join(path, "RecipeBrowser/RecipeBrowserSettings.txt");
            StreamWriter SettingsWriter = new(SettingsPath, false);
            SettingsWriter.Write(folderPath);
            SettingsWriter.Close();
            ImageFolderInput.Text = "";
        }
        else {
            Console.WriteLine(ImageFolderInput + " is not a valid directory");
        }
    }

}

