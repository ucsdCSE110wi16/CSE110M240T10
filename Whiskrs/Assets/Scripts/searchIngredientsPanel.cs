using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class searchIngredientsPanel : MonoBehaviour {
    public InputField input;
    public GameObject resultGrid;
    public static searchIngredientsPanel Instance { get; private set; }

    void Awake() {
        Instance = this;
    }

    public void loadIngredients()
    {
        StartCoroutine(JSONClient.Post("http://www.supercook.com/dyn/sugg", new JSONObject("{\"needsimage\":1}"), suggestionsLoaded));
        foreach (string s in main.Instance.strArr)
        {
            if(s != "") main.Instance.addIngredient(s);
        }
    }

    public void togglePanel()
    {
        if (this.gameObject.GetComponent<RectTransform>().localScale.x == 0)
        {
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }
        else
        {
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
        }
    }

    public void autocomplete() {
        foreach (Transform child in resultGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        RectTransform rt = resultGrid.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
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
        RectTransform rt = resultGrid.GetComponent<RectTransform>();
        b.onClick.AddListener(() =>
        {
            main.Instance.addIngredient(b.name);
            GameObject.Destroy(b.gameObject);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x - 500, rt.sizeDelta.y);
        });
        button.transform.SetParent(resultGrid.transform);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x + 500, rt.sizeDelta.y);
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
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
