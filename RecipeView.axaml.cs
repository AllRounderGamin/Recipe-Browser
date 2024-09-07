using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recipe_Browser;

public partial class RecipeView : UserControl
{

    private static readonly RecipeContext db = new();
    public Recipe? SelectedRecipe = null;

    public RecipeView()
    {
        InitializeComponent();
    }

    public void SearchRecipes(object sender, KeyEventArgs args){
        if (args.Key != Key.Return || String.IsNullOrEmpty(RecipeSearch.Text)){
            return;
        }
        // Editor prefers String.Contains(String, StringComparison), however EF Framework Core cannot parse that, hence this is done instead
        List<Recipe> queryResult = [.. db.Recipes.Where(recipe => recipe.Name.ToUpper().Contains(RecipeSearch.Text.ToUpper()))];
        if (queryResult.Count > 0){
            SelectedRecipe = queryResult[0];
            LoadRecipe();
        }
    }

    public void ChangeText(object sender, RoutedEventArgs args){
        if (args.Source is not Control source || SelectedRecipe == null)
        {
            return;
        }
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

    public void LoadRecipe(){
        if (SelectedRecipe == null){
            return;
        }
        RecipeName.Text = SelectedRecipe.Name;
        BookName.Text = SelectedRecipe.Book;
        TextWindow.Text = SelectedRecipe.Instructions;
    }

}

