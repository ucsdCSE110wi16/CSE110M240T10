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
        SuperCookRequest data = new SuperCookRequest(1, string.Join(",",ingredients.ToArray()), skip);
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
        public SuperCookRequest(int ni, string kit, int st) {
            needsimage = ni; kitchen = kit; start = st;
        }
        // custom json convertion
        public override string ToString()
        {
            return "{\"needsimage\":" + needsimage + ",\"kitchen\":\"" + kitchen + "\",\"start\":" + start + "}";
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