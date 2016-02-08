using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class searchIngredientsPanel : MonoBehaviour {
    public InputField input;
    public GameObject resultGrid;
    public static searchIngredientsPanel Instance { get; private set; }
    private string[] strArr;

    void Awake() {
        Instance = this;
        strArr = PlayerPrefs.GetString("ingredients").Split(',');
    }

    public void loadIngredients()
    {
        StartCoroutine(JSONClient.Post("http://www.supercook.com/dyn/sugg", new JSONObject("{\"needsimage\":1}"), suggestionsLoaded));
        foreach (string s in strArr)
        {
            main.Instance.addIngredient(s);
        }
    }

    public void autocomplete() {
        foreach (Transform child in resultGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        StartCoroutine(JSONClient.Get("http://www.supercook.com/dyn/autoc?term=" + WWW.EscapeURL(input.text), autocompleteCallback));
    }

    private void suggestionsLoaded(JSONObject response)
    {
        // Convert results
        JSONArray arr = JSON.Parse(response.GetField("results").ToString()).AsArray;
        foreach (JSONNode jn in arr)
        {
            if (!main.Instance.hasIngredient(jn))
                newButton(jn["ingredient"]);
        }
    }

    private void newButton(string name) {
        GameObject button = (GameObject)Instantiate(Resources.Load("IngredientButton"), Vector3.zero, Quaternion.identity);
        Text txt = button.GetComponentInChildren<Text>();
        txt.text = name;
        Button b = button.GetComponent<Button>();
        b.name = name;
        b.onClick.AddListener(() =>
        {
            main.Instance.addIngredient(b.name);
            GameObject.Destroy(b.gameObject);
        });
        button.transform.SetParent(resultGrid.transform);
        RectTransform rt = resultGrid.GetComponent<RectTransform>();
        rt.Translate(new Vector3(-350,0, 0));
    }

    private void autocompleteCallback(JSONObject response)
    {
        JSONArray arr = JSON.Parse(response.ToString()).AsArray;
        foreach (JSONNode jn in arr)
        {
            if (!main.Instance.hasIngredient(jn))
                newButton(jn);   
        }
    }
}
