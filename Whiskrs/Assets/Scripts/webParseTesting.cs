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

    public bool allRecipesTester() {
        // Call the tester on allrecipes.com
        string testUrl = "http://allrecipes.com/recipe/21174/bbq-pork-for-sandwiches/";
        string realName = "BBQ Pork for Sandwiches";
        string[] realIng = {"1 (14 ounce) can beef broth", "3 pounds boneless pork ribs", "1 (18 ounce) bottle barbeque sauce" };
        string realDir = "Pour can of beef broth into slow cooker, and add boneless pork ribs. Cook on High heat for 4 hours, or until meat shreds easily. Remove meat, and shred with two forks. It will seem that it&#39;s not working right away, but it will. Preheat oven to 350 degrees F (175 degrees C). Transfer the shredded pork to a Dutch oven or iron skillet, and stir in barbeque sauce. Bake in the preheated oven for 30 minutes, or until heated through.";
        return tester (testUrl, realName, realIng, realDir);
    }

    public bool epicuriousTester() {
        // Call the tester on Epicurious
        string testUrl = "http://www.epicurious.com/recipes/food/views/muffuletta-sandwich-240766";
        string realName = "Muffuletta Sandwich";
        string[] realIng = { "1 cup each pitted green and black olives, coarsely chopped", "1 tablespoon tiny capers", "1/3 cup diced (1/4 inch) roasted red bell pepper", "1/4 cup diced (1/4 inch) celery, with leaves",
            "2 tablespoons chopped flat-leaf parsley", "2 teaspoons finely minced garlic", "2 tablespoons red-wine vinegar", "2 tablespoons olive oil", "Salt and pepper, to taste" };
        string realDir = "1. Prepare the Olive Salad ahead of time: Combine all the ingredients and set aside in the refrigerator for 4 hours or longer.\n\n" +
            "2. Assemble the sandwich: Spread half of the Olive Salad on the bottom half of the bread. Layer with salami, provolone and mortadella, then top with the remaining Olive Salad. Cover with the top of the bread, press down and let stand for 10 to 15 minutes. Wrap the sandwich in plastic wrap and let stand for 1 hour.\n\n" +
            "3. Unwrap, cut into 6 wedges using a serrated knife, then wrap them for the road. Be sure to hollow out the bread so the salad can fit inside. Doing so also cuts carbs and calories.\n\n";
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
