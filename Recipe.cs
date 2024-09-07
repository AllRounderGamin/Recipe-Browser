using System.Collections.Generic;



namespace Recipe_Browser{
    public class Recipe()
    {
        public int RecipeId { get; set; }
        public string Name { get; set; } = "";
        public string Book { get; set; } = "";
        public List<Ingredient> Ingredients { get; } = [];
        public string Instructions { get; set; } = "";
        public string Notes { get; set; } = "";
        public string Photos { get; set; } = "";
    }
    public class Ingredient(){
        public int IngredientId { get; set; }
        public string Name {get; set;} = "";
        public string Measurement {get; set;} = "";
        public int Amount {get; set;} = -1;
        public List<Recipe> Recipes { get; } = [];

    }
}