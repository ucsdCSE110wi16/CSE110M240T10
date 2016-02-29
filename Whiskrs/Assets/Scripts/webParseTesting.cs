using UnityEngine;
using System.Collections;

public class webParseTesting : MonoBehaviour {
    public string url;
    public string[] ingredients;
    public string directions;
    public string name;

    // Use this for initialization
    void Start() {
        webParser.Instance.parse(url, callback);
    }

    public bool foodTester()
    {
        bool toReturn = false;    
        url = "http://www.food.com/recipe/simple-spaghetti-dish-256215";
        Start();
        string[] realIng = { "8 ounces spaghetti", "4 tablespoons butter, to taste", "1/2 teaspoon salt, to taste", "1/4 teaspoon black pepper, to taste" };
        string realDir = "Cook spaghetti according to package directions. When pasta is done, drain and return to pot.Add butter, salt and pepper to taste. Mix well and serve.";
        if (name.Equals("Simple Spaghetti Dish"))
            toReturn = true;
        else
            return false;

        if (ingredients.Equals(realIng) != true)
            return false;
        if (directions.Equals(realDir) != true)
            return false;
 
        return toReturn;
    }

    public bool myRecipesTester()
    {
        bool toReturn = false;
        url = "http://www.myrecipes.com/recipe/breakfast-club";
        Start();
        string[] realIng = { "2 teaspoons mayonnaise", "3 toasted slices English muffin bread (4 in. square)", "3 large eggs", "1 butter lettuce leaf", "1 thick slice firm-ripe tomato", "2 slices crisp-cooked bacon" };
        string realDir = "Spread about 2 teaspoons mayonnaise on 1 side of each of 3 toasted slices English muffin bread (4 in. square). Set 1 slice, mayonnaise side up, on a plate. Top with 2 soft-scrambled large eggs or 1 large egg fried sunny side up; sprinkle lightly with salt and pepper. Top with another slice of toast (mayonnaise side up), 1 butter lettuce leaf (optional), 1 thick slice firm-ripe tomato, 2 slices crisp-cooked bacon, and remaining toast (mayonnaise side down). Secure layers with toothpicks and cut sandwich into halves or quarters. Nutritional analysis per sandwich.";

        if (name.Equals("Breakfast Club"))
        {
            toReturn = true;
        }
        else
            return false;
        if (ingredients.Equals(realIng) != true)
            return false;
        if (directions.Equals(realDir) != true)
            return false;

        return toReturn;
    }





    public void callback(recipe result, string url)
    {
        if( result != null)
        {
            ingredients = result.ingredients.ToArray();
            directions = result.directions;
            name = result.name;
        }
    }
 
    string Awake()
    {
        bool myRecipes = myRecipesTester();
        bool food = foodTester();

        if (myRecipes == false)
            return "myRecipes failed";
        if (food == false)
            return "food.com failed";

        return "All webParse Tests Pass";

    }


}
