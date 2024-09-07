using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;


namespace Recipe_Browser;

public partial class App : Application
{
    public override void Initialize()
    {
        PrepareDatabase();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static void PrepareDatabase()
    {
        RecipeContext db = new();

        db.Database.EnsureCreated();
        
/* TEST DATA
        if (db.Find<Recipe>(1) != null)
        {
           return; 
        }

        Recipe chocFudge = new()
        {
            Name = "Very Best Chocolate Fudge Cake",
            Book = "Mary Berry's Baking Bible",
            Instructions = "Preheat oven to 180c, grase and line the base of 2 8 inch circle cake pans\nBlend the cocoa powder and water together in a large bowl\nAdd the rest of the ingredients to this mixture and beat until a smooth thick batter, divide batter between prepared tins\nBake in the preheated oven for 25-30 minutes, leave to cool in the tins for a few minutes before cooling on a wire rack\nFor the icing warm the apricot jam in a small pan and spread ove the base of one cake and top of another\nBreak the chocolate into pieces and gently heat together with the cream over a bowl of hot water\nRemove from heat and stir to ensure fully melted before leaving to cool to the point of settings\nSpread on top of the apricot jam on both cakes and sandwich together",
            Notes = "Relatively easy but had some ingredient issues \n Had to roughly double water used as cocoa became clay like \n Be sure to not add baking powder into milk as this seemed to impact rise \n Needed a 3:2 choc cream ratio for ganache to actually set",
            Photos = "ChocFudgeMB\nChocFudgeMBCut"
        };
        List<Ingredient> stuff = [
            new Ingredient(){Name="Cocoa Powder", Amount=50, Measurement="grams"},
            new Ingredient(){Name="Boiling Water", Amount=6, Measurement="tbsp"},
            new Ingredient(){Name="Eggs", Amount=3, Measurement=""},
            new Ingredient(){Name="Milk", Amount=50, Measurement="ml"},
            new Ingredient(){Name="Self Raising Flour", Amount=175, Measurement="grams"},
            new Ingredient(){Name="Baking Powder", Amount=1, Measurement="tsp"},
            new Ingredient(){Name="Butter", Amount=100, Measurement="grams"},
            new Ingredient(){Name="Caster Sugar", Amount=275, Measurement="grams"},
            new Ingredient(){Name="Apricot Jam", Amount=3, Measurement="tbsp"},
            new Ingredient(){Name="Plain Chocolate", Amount=150, Measurement="g"},
            new Ingredient(){Name="Double Cream", Amount=150, Measurement="ml"}
        ];
        foreach(Ingredient ingredient in stuff){
            chocFudge.Ingredients.Add(ingredient);
        }

        Recipe GingerLoaf = new()
        {
            Name = "Ginger Loaf Cake with Orange Icing",
            Book = "https://thebakingexplorer.com/gingerbread-loaf-cake/",
            Instructions = "Preheat oven to 150c, grease and line a 2lb loaf tin\nMelt butter, Golden Syrup and Light Brown Sugar in a saucepan on low heat, leave to cool for 5 min\nAdd eggs and milk to pan and whisk\nCombine flour, Bicarbonate of Soda and spices together\nPour melted mixture into flour and whisk\nPour into tin and bake for 1 hour, leave to cool\nFor the icing combine the Milk, Orange Extract and Icing sugar with an electric whisk, drizzle over the cake - piping bags work best\nStore in an airtight container for up to 5 days",
            Notes = "Mix icing for at least 5 minutes to ensure fully mixed in \n Cake is particularly nice warmed up but overheating will melt icing",
            Photos = "GingerLoafBE.jpg\nGingerLoafBECut.jpg"
        };
        stuff = [
            new Ingredient (){Name = "Butter", Amount = 100, Measurement = "grams"},
            new Ingredient (){Name = "Golden Syrup", Amount = 225, Measurement = "grams"},
            new Ingredient(){Name="Light Brown Sugar", Amount=50, Measurement="grams"},
            new Ingredient(){Name="Eggs", Amount=2, Measurement=""},
            new Ingredient(){Name="Milk", Amount=150, Measurement="ml"},
            new Ingredient(){Name="Flour", Amount=225, Measurement="grams"},
            new Ingredient(){Name = "Bicarbonate of Soda", Amount=1, Measurement="tsp"},
            new Ingredient(){Name="Ground Ginger", Amount=1, Measurement="Tbsp"},
            new Ingredient(){Name="Mixed Spice", Amount=2, Measurement="tsp"},
            new Ingredient(){Name="Icing Sugar", Amount=75, Measurement="grams"},
            new Ingredient(){Name="Orange Extract", Amount=1, Measurement="tsp"},
            new Ingredient(){Name="Milk", Amount=2, Measurement="tsp"}
        ];
        foreach(Ingredient ingredient in stuff){
            GingerLoaf.Ingredients.Add(ingredient);
        }
        db.Add(chocFudge);
        db.Add(GingerLoaf);
        db.SaveChanges();
        */
    }
}