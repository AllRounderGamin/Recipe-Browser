using Avalonia.Controls;
using System.IO;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;

namespace Recipe_Browser;

public partial class RecipeView : UserControl
{

    private static readonly RecipeContext db = new();
    private List<Recipe> SelectedRecipes = [];
    private int SelectedIndex = 0;
    private string? ImageFolderPath;

    public RecipeView()
    {
        getImageFolder();
        InitializeComponent();
    }

    public void SearchRecipes(object sender, KeyEventArgs args){
        if (args.Key != Key.Return || String.IsNullOrEmpty(RecipeSearch.Text)){
            return;
        }
        // Editor prefers String.Contains(String, StringComparison), however EF Framework Core cannot parse that, hence this is done instead
        List<Recipe> queryResult = [.. db.Recipes.Where(recipe => recipe.Name.ToUpper().Contains(RecipeSearch.Text.ToUpper()))];
        if (queryResult.Count > 0){
            SelectedRecipes = queryResult;
            SelectedIndex = 0;
            LoadRecipe();
        }
    }

    public void ChangeText(object sender, RoutedEventArgs args){
        if (args.Source is not Control source || SelectedRecipes.Count < 1)
        {
            return;
        }
        Recipe SelectedRecipe = SelectedRecipes[SelectedIndex];
        switch (source.Name)
        {
            case "IngredientsButton":
                TextWindow.Text = "";
                if (SelectedRecipe.Ingredients.Count == 0){
                    db.Entry(SelectedRecipe)
                        .Collection(recipe => recipe.Ingredients)
                        .Load();
                }
                foreach (Ingredient ingredient in SelectedRecipe.Ingredients){
                    TextWindow.Text += ingredient.Amount + " " + ingredient.Measurement + " " + ingredient.Name + "\n";
                }
                return;

            case "RecipeButton":
                TextWindow.Text = SelectedRecipe.Instructions;
            return;

            case "NotesButton":
                TextWindow.Text = SelectedRecipe.Notes;
                return;
        }
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

    public void ChangeImage(object sender, RoutedEventArgs args){
        if (args.Source is not Control source || SelectedRecipes.Count < 1)
        {
            return;
        }
        switch(source.Name){
            case "PreviousImage":
                RecipeImages.Previous();
                return;
            case "NextImage":
                RecipeImages.Next();
                return;
        }

    }

    public void LoadRecipe(){
        if (SelectedRecipes.Count < 1){
            return;
        }
        Recipe SelectedRecipe = SelectedRecipes[SelectedIndex];
        LoadImages(SelectedRecipe);
        RecipeName.Text = SelectedRecipe.Name;
        BookName.Text = SelectedRecipe.Book;
        TextWindow.Text = SelectedRecipe.Instructions;
    }

    private void LoadImages(Recipe recipe){
        RecipeImages.Items.Clear();
        string[] Photos = recipe.Photos.Split("\n");
        if (ImageFolderPath == null){
            return;
        }
        foreach (string photo in Photos){
            if (File.Exists(ImageFolderPath+photo)){
                Image newImage = new(){Source= new Bitmap(ImageFolderPath+photo)};
                RecipeImages.Items.Add(newImage);
            }
        }
    }

    private void getImageFolder(){
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        string SettingsPath = Path.Join(path, "RecipeBrowser/RecipeBrowserSettings.txt");
        if (!File.Exists(SettingsPath)){
            ImageFolderPath = null;
            StreamWriter sw = File.AppendText(SettingsPath);
            sw.Close();
            Console.WriteLine("Created Settings File");
            return;
        }
        StreamReader file = new(SettingsPath);
        string? imageUrl = file.ReadLine();
        if (Directory.Exists(imageUrl)){
            ImageFolderPath = imageUrl;
        } else {
            ImageFolderPath = null;
            Console.WriteLine("Invalid Image Path");
        }
        file.Close();
    }

}

