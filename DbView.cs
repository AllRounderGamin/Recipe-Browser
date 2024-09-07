using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Recipe_Browser;

public partial class DbView : UserControl
{
    public Recipe? SelectedRecipe = null;
    private static readonly RecipeContext db = new();

    public DbView()
    {
        InitializeComponent();
    }


    public void SaveEntry(object sender, RoutedEventArgs args){
            if (SelectedRecipe == null){
                Recipe newRecipe = new();
                if (CreateRecipeObj(newRecipe) == 0){
                    db.Add(newRecipe);
                    db.SaveChanges();
                }
            } else{
                if (CreateRecipeObj(SelectedRecipe) == 0){
                    db.Update(SelectedRecipe);
                    db.SaveChanges();
                }
            }
            ResetForm(sender, args);
        }



    public int CreateRecipeObj(Recipe Obj){
        if (!(String.IsNullOrEmpty(RecipeNameInput.Text) || String.IsNullOrEmpty(BookNameInput.Text) || String.IsNullOrEmpty(IngredientsInput.Text) || String.IsNullOrEmpty(InstructionsInput.Text) || String.IsNullOrEmpty(NotesInput.Text) || String.IsNullOrEmpty(PhotosInput.Text))){
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
                if (Int32.TryParse(ingredientData[0], out int ingredientAmount))
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
        SelectedRecipe = null;
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
        List<Recipe> queryResult = [.. db.Recipes.Where(recipe => recipe.Name.ToUpper().Contains(DbSearch.Text.ToUpper()))];
        if (queryResult.Count > 0){
            SelectedRecipe = queryResult[0];
            RecipeNameInput.Text = SelectedRecipe.Name;
            BookNameInput.Text = SelectedRecipe.Book;
            InstructionsInput.Text = SelectedRecipe.Instructions;
            NotesInput.Text = SelectedRecipe.Notes;
            PhotosInput.Text = SelectedRecipe.Photos;

            db.Entry(SelectedRecipe)
                .Collection(recipe => recipe.Ingredients)
                .Load();
            foreach (Ingredient ingredient in SelectedRecipe.Ingredients){
                IngredientsInput.Text = IngredientsInput.Text + ingredient.Amount + " " + ingredient.Measurement + " " + ingredient.Name + "\n";
            }
        }
    }

    public void DeleteEntry(object sender, RoutedEventArgs args){
        if (SelectedRecipe != null){
            db.Remove(SelectedRecipe);
            db.SaveChanges();
        }
        ResetForm(sender, args);
    }

}

