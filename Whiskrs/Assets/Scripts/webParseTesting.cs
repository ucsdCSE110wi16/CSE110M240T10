using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class webParseTesting : MonoBehaviour {
    public static webParseTesting Instance;
    public recipe expected;

    void Start() {
        Instance = this;
    }

    private bool tester(string testUrl, string realName, string[] realIng, string realDir) {
        expected = new recipe(realName, new List<string>(realIng), realDir, null);
        // Parse the website
        webParser.Instance.parse(testUrl, callback);
        return true;
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

    public bool chatelaineTester()
    {
        // Call the tester on chatelaine.com    
        string testUrl = "http://www.chatelaine.com/recipe/world-cuisine-2/jerk-chicken-with-mango-slaw/";
        string realName = "Jerk chicken with mango slaw";
        string[] realIng = { "jalapenos,unseeded","small onion, sliced into quarters" , "lime juice ", "dried thyme", "soy sauce", "grated ginger", "allspice", "cinnamon", "canola oil", "salt", "chicken legs, about 2 kg", "ginger beer, or ginger ale", "Mango Slaw, see link below" };
        string realDir = "WHIRL jalapeno with onion, lime juice, thyme, soy, ginger, allspice, cinnamon, oil and salt in a food processor until mixture is puréed." +
            "TRIM and discard excess fat, then cut slashes in chicken.Transfer to slow cooker insert and coat with jalapeno mixture. Pour in ginger beer. Cover and cook until meat is tender, about 21 / 2 hours on high or 5 hours on low.Remove chicken to a platter.Strain sauce into a measuring cup." +
            "PREHEAT barbecue to medium-high.Oil grill and barbecue chicken until skin is crisp, 3 min per side. Using 2 forks, shred chicken and skin. Stir 1 / 4 cup sauce with shredded chicken.Serve over rice or in toasted buns with Mango Slaw. Drizzle with more sauce, if desired.Meat and sauce will keep well, refrigerated, up to 3 days.";
        return tester(testUrl, realName, realIng, realDir);
    }


    public bool eatingWellTester()
    {
        // Call the tester on eatingwell.com    
        string testUrl = "http://www.eatingwell.com/recipes/peanut_tofu_cabbage_wraps.html";
        string realName = "Peanut-Tofu Cabbage Wraps";
        string[] realIng = {"8 small napa or Savoy cabbage leaves or 4 large, cut in half crosswise", "1 tablespoon canola oil", "1 14- to 16-ounce package extra-firm tofu, patted dry and crumbled,1/4 teaspoon salt","5 tablespoons prepared peanut sauce", "1 tablespoon rice vinegar", "1 1/2 teaspoons lime zest", "1 cup julienned Asian pear", "1 cup julienned English cucumber", "1/4 cup finely chopped cilantro"};
        string realDir = "Wash and dry cabbage leaves well and cut out any tough ribs or stems." +
        "Heat oil in a large nonstick skillet over medium-high heat.Add tofu, season with salt and cook, stirring often, until just golden brown, 4 to 6 minutes." +
        "Meanwhile, whisk peanut sauce, vinegar and lime zest in a small bowl." +
        "Remove the pan from the heat, add the sauce mixture and stir to combine.Serve the tofu in the cabbage leaves, topped with pear, cucumber and cilantro.";
        return tester(testUrl, realName, realIng, realDir);
    }

    public bool rrTester()
    {
        // Call the tester on rachaelRaymag.com    
        string testUrl = "http://www.rachaelraymag.com/recipe/hearty-chicken-soup/";
        string realName = "Hearty Chicken Soup";
        string[] realIng = { "low sodium chicken broth", "small carrots, peeled and thinly sliced", "celery, thinly sliced", "small onion, chopped", "shredded cooked chicken"};
        string realDir = "In a large pot, bring broth, carrots, celery and onion to a boil. Simmer until vegetables are tender, 8 to 10 minutes.Add chicken and cook until warmed through, about 3 minutes; season with salt and pepper.Serve immediately or use as a base for the variations.";
        return tester(testUrl, realName, realIng, realDir);
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
        string[] realIng = { "1 (14 ounce) can beef broth", "3 pounds boneless pork ribs", "1 (18 ounce) bottle barbeque sauce" };
        string realDir = "Pour can of beef broth into slow cooker, and add boneless pork ribs. Cook on High heat for 4 hours, or until meat shreds easily. Remove meat, and shred with two forks. It will seem that it&#39;s not working right away, but it will. Preheat oven to 350 degrees F (175 degrees C). Transfer the shredded pork to a Dutch oven or iron skillet, and stir in barbeque sauce. Bake in the preheated oven for 30 minutes, or until heated through.";
        return tester (testUrl, realName, realIng, realDir);
    }

    public bool marthaStewartTester() {
        // Call the tester on marthastewart.com
        string testUrl = "http://www.marthastewart.com/925229/chicken-mushrooms";
        string realName = "Chicken with Mushrooms";
        string[] realIng = { "1/4 cup all-purpose flour", "1 1/2 pounds chicken cutlets", "Salt and pepper", "1 tablespoon olive oil", "3 tablespoons unsalted butter, divided", "2 tablespoons fresh thyme leaves, chopped",
            "1 pound button mushrooms, trimmed and quartered", "1/4 cup dry white wine", "1/4 cup chicken broth", "1/4 cup chopped fresh parsley" };
        string realDir = "Place flour in a shallow dish. Season chicken with salt and pepper, then coat with flour, shaking off excess.\nIn a large skillet, heat oil and 1 tablespoon butter over medium-high. In batches, cook chicken until browned and cooked through, about 3 minutes per side. Transfer to a plate and tent loosely with foil.\nReduce heat to medium, add thyme, mushrooms, and remaining 2 tablespoons butter, and cook until softened, 6 minutes.\nAdd wine and broth and cook, stirring, until reduced by half, 3 minutes. Season with salt and pepper. Return chicken to pan along with any accumulated juices; top with parsley.";
        return tester (testUrl, realName, realIng, realDir);
    }

    public bool foodNetworkTester() {
        // Call the tester on foodnetwork.com
        string testUrl = "http://www.foodnetwork.com/recipes/a-bowl-of-gluten-free-oatmeal-recipe.html";
        string realName = "A Bowl of Gluten-Free Oatmeal";
        string[] realIng = { "1 cup whole milk", "1 cup water", "1/2 teaspoon kosher salt", "1 teaspoon vanilla extract", "1 cup whole rolled gluten-free oats" };
        string realDir = "Set a saucepan over high heat. Pour in the milk and water. Add the salt and vanilla extract. Bring the liquids to a boil.\n\n" +
                         "When the milky water is boiling, pour in the oats. Stir quite vigorously. When the water returns to a boil, turn down the heat to low. Simmer the oats, stirring every few minutes, until the oats are creamy and plump, the liquid fully absorbed, about 15 minutes.\n\n" +
                         "Turn off the heat and cover the pan. Let the oatmeal sit for five minutes to fully absorb the liquid.\n\n" +
                         "Top with your favorite sweetener and fruit. (This one is maple syrup, peaches, and blackberries.)\n\n" +
                         "Variations: If you cannot eat dairy, almond milk or hemp milk work well here too.\n\n" +
                         "If you have a fresh vanilla bean, scrape the insides of it into the pot instead of vanilla extract. This will be the best oatmeal you have ever eaten.\n\n";
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

    public bool delishTester() {
        // Call the tester on Delish
        string testUrl = "http://www.delish.com/cooking/recipe-ideas/recipes/a46245/churro-waffles-recipe/";
        string realName = "Churro Waffles";
        string[] realIng = { "Prepared waffle batter", "Melted butter, for brushing", "1/4 c. sugar", "2 tbsp. ground cinnamon", "Dulce de leche, for drizzling" };
        string realDir = "Prepare waffle batter and cook in waffle maker.\n\n" +
            "Transfer to a cutting board and brush with melted butter. In a shallow dish, combine sugar and cinnamon. Dunk waffle in cinnamon sugar.\n\n" +
            "Drizzle in dulce de leche and cut into triangles and serve.\n\n";
        return tester (testUrl, realName, realIng, realDir);
    }

    public bool wegmansTester() {
        // Call the tester on Wegmans
        string testUrl = "http://www.wegmans.com/webapp/wcs/stores/servlet/ProductDisplay?langId=-1&storeId=10052&catalogId=10002&productId=723033";
        string realName = "Arugula Salad";
        string[] realIng = { "Juice of 1/2  lemon (about 1 1/2 Tbsp)", "1 Tbsp  Italian Classics Tuscan White Wine Vinegar", "1/4 cup  Italian Classics Toscano Extra Virgin Olive Oil", "1 container (5 oz)  Wegmans Organic Baby Arugula",
            "Salt and pepper to taste", "Italian Classics Parmigiano Reggiano (Cheese Shop), shaved, for garnish" };
        string realDir = "Add lemon juice, vinegar, and olive oil to large bowl; whisk to combine. Add arugula to bowl; toss to combine. Season with salt and pepper; garnish with cheese shavings.\n\n";
        return tester (testUrl, realName, realIng, realDir);
    }

    public void callback(recipe r, string url){
        if (expected == null) {
            scenarioProgram.e.websitesParsed.Add(webParser.getURLBase(url) + ": Failed");
            scenarioProgram.runNextWebParsingTests();
            return; 
        }
        if (r == null)
        {
            scenarioProgram.e.websitesParsed.Add(webParser.getURLBase(url) + ": Website is necessary");
            scenarioProgram.runNextWebParsingTests();
            return; 
        }
        if(r.name.IndexOf(expected.name) > -1)
            scenarioProgram.e.websitesParsed.Add(webParser.getURLBase(url) + ": Succeeded");
        else
        {
            scenarioProgram.e.websitesParsed.Add(webParser.getURLBase(url) + ": Names do not match");
        }
        scenarioProgram.runNextWebParsingTests();
    }
}
