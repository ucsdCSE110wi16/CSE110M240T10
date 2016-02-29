using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class SuperCook : MonoBehaviour{

    public delegate void superCookDelegate(SuperCookResult result);
    private superCookDelegate finished;
    private int skip = 0;
    private SuperCookResult final;
    private List<string> ingredients;
    public static SuperCook Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    // Main call
    public void getRecipes(List<string> ingredients, superCookDelegate responseFunction)
    {
        this.ingredients = ingredients;
        string cat = "";
        if (main.Instance.cuisine != "")
        {
            cat += main.Instance.cuisine;
            if (main.Instance.mealtype != "")
                cat += "," + main.Instance.mealtype;
        }
        else if (main.Instance.mealtype != "")
            cat += main.Instance.mealtype;

        SuperCookRequest data = new SuperCookRequest(1, string.Join(",", ingredients.ToArray()), skip, cat, string.Join("", main.Instance.restrictions.ToArray()));
        JSONObject json = new JSONObject(data.ToString());
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Cookie", "myIngredients=" + WWW.EscapeURL(string.Join(",", ingredients.ToArray())));
        StartCoroutine(JSONClient.Post("http://www.supercook.com/dyn/results", json, callback, headers));
        finished = responseFunction;
    }

    // Network call completed
    private void callback(JSONObject result) {
        if (final == null) {
            // Create Result container
            final = new SuperCookResult();
            final.total_can_make_right_now = int.Parse(result.GetField("total_can_make_right_now").ToString());
            final.results = new List<SuperCookRecipe>();
        }
        // Convert results
        JSONArray arr = JSON.Parse(result.GetField("results").ToString()).AsArray;
        foreach (JSONNode jn in arr) {
            SuperCookRecipe scRecipe = new SuperCookRecipe();
            scRecipe.title = jn["title"];
            scRecipe.url = jn["url"];
            scRecipe.uses = jn["uses"];
            scRecipe.id = jn["id"].AsInt;
            final.results.Add(scRecipe);
        }
        if (final.results.Count >= final.total_can_make_right_now)
            finished(final);
        else
        {
            skip += 40;
            getRecipes(ingredients, finished);
        }
    }

    // Request class for super cook, formatted img, kitchen, start
    protected class SuperCookRequest { 
        public int needsimage;
        public string kitchen;
        public int start;
        public string kw;
        public string focus;
        public string catname;
        public string exclude;
        public SuperCookRequest(int ni, string kit, int st, string catname="", string exclude = "") {
            needsimage = ni; kitchen = kit; start = st; this.catname = catname; this.exclude = exclude;
        }
        // custom json convertion
        public override string ToString()
        {
            return "{\"needsimage\":" + needsimage + ",\"kitchen\":\"" + kitchen + "\",\"catname\":\"" + catname + "\",\"exclude\":\"" + exclude + "\",\"start\":" + start + "}";
        }
    }
}

// SuperCookResult format
public class SuperCookResult
{
    public int total_can_make_right_now;
    public List<SuperCookRecipe> results;
}

// SuperCookRecipe format
public class SuperCookRecipe
{
    public string title;
    public string url;
    public string uses;
    public int id;
    public Texture2D img;
}

public class categories {
    public static string[] meals = { "Breads", "Breakfast", "Cakes", "Casseroles", 
                                       "Cookies", "Desserts", "Dinner", "Dips", "Drinks","Fish recipes","Grilling & BBQ","Kid Friendly",
                                       "Meat recipes","Poultry recipes","Quick & Easy","Salad Dressings","Salads","Sandwiches","Sauces",
                                       "Seafood recipes","Slow Cooker","Soups","Veggie recipes" };
    public static string[] cuisines = {"Asian Caribbean","Chinese","French","German","Indian & Thai","Italian","Mediterranean",
                                          "Mexican","Tex-Mex & Southwest"};
    public static string[] restrictions = { "Poultry", "Nuts", "Meat", "Gluten", "Dairy", "Shellfish", "Fish" };
}