using UnityEngine;
using System.Collections;
using System.Linq;

public class webParseTesting : MonoBehaviour {
    public string url;
    public string[] ingredients;
    public string directions;
    public string name;

    // Use this for initialization
    void Start() {
        webParser.Instance.parse(url, callback);
    }

    private bool tester(string testUrl, string realName, string[] realIng, string realDir) {
        // Parse the website
        url = testUrl;
        Start ();

        // Return whether everything matches up
        return name.Equals(realName) && ingredients.SequenceEqual(realIng) && directions.Equals(realDir);
    }

    public bool foodTester()
    {
        // Call the tester on food.com    
        string testUrl = "http://www.food.com/recipe/simple-spaghetti-dish-256215";
        string realName = "Simple Spaghetti Dish";
        string[] realIng = { "8 ounces spaghetti", "4 tablespoons butter, to taste", "1/2 teaspoon salt, to taste", "1/4 teaspoon black pepper, to taste" };
        string realDir = "Cook spaghetti according to package directions. When pasta is done, drain and return to pot.Add butter, salt and pepper to taste. Mix well and serve.";
        return tester (testUrl, realName, realIng, realDir);
    }

    public bool myRecipesTester()
    {
        // Call the tester on myrecipes.com
        string testUrl = "http://www.myrecipes.com/recipe/breakfast-club";
        string realName = "Breakfast Club";
        string[] realIng = { "2 teaspoons mayonnaise", "3 toasted slices English muffin bread (4 in. square)", "3 large eggs", "1 butter lettuce leaf", "1 thick slice firm-ripe tomato", "2 slices crisp-cooked bacon" };
        string realDir = "Spread about 2 teaspoons mayonnaise on 1 side of each of 3 toasted slices English muffin bread (4 in. square). Set 1 slice, mayonnaise side up, on a plate. Top with 2 soft-scrambled large eggs or 1 large egg fried sunny side up; sprinkle lightly with salt and pepper. Top with another slice of toast (mayonnaise side up), 1 butter lettuce leaf (optional), 1 thick slice firm-ripe tomato, 2 slices crisp-cooked bacon, and remaining toast (mayonnaise side down). Secure layers with toothpicks and cut sandwich into halves or quarters. Nutritional analysis per sandwich.";
        return tester (testUrl, realName, realIng, realDir);
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
