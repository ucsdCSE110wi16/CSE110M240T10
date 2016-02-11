using UnityEngine;
using System.Collections;

public class webParseTest : MonoBehaviour {
    public string url = "http://www.myrecipes.com/recipe/grilled-steak";
    public string[] ingredients;
    public string directions;
    public string name;

	// Use this for initialization
	void Start () {
        StartCoroutine(JSONClient.GetHTML(url,callback));
	}

    public void callback(string response) {
        recipe result = webParser.parse(url, response);
        ingredients = result.ingredients.ToArray();
        directions = result.directions;
        name = result.name;
    }
}
