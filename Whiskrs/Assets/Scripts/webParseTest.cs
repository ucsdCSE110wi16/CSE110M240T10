using UnityEngine;
using System.Collections;

public class webParseTest : MonoBehaviour {
    public string url = "http://www.myrecipes.com/recipe/grilled-steak";
    public string[] ingredients;
    public string directions;
    public string name;

	// Use this for initialization
	void Start () {
        webParser.Instance.parse(url, callback);
	}

    public void callback(recipe result, string url)
    {
        if (result != null)
        {
            ingredients = result.ingredients.ToArray();
            directions = result.directions;
            name = result.name;
        }
    }
}
