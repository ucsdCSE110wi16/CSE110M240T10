using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class main : MonoBehaviour
{
    public static main Instance { get; private set; }
    public List<string> ingredients;
    public GameObject myIngredientsGrid;
    public delegate void recipeLoaded(List<ingredient> ingredients, string directions);
    public bool ingredientsChanged = true;
    public string[] strArr;
    public string[] favorites;
    public List<string> favoriteRecipes;

    void Awake()
    {
        Instance = this;
        ingredients = new List<string>();
        favoriteRecipes = new List<string>();
        /*
        // Make sure we have an instance of webParser before attempting to use it.
        // May need only add component here since Instance is set in webParser Awake function
        webParser.Instance = gameObject.AddComponent<webParser>();
        webParser.Instance.parse("", delegate { });*/
        strArr = PlayerPrefs.GetString("ingredients").Split(',');
        favorites = PlayerPrefs.GetString("favorites").Split(',');
        Debug.Log(string.Join(",", favorites));
        foreach (string s in favorites)
        {
            if (s != "") favoriteRecipes.Add(s);
        }
    }

    public void addIngredient(string ingredient) {
        ingredients.Add(ingredient);
        newButton(ingredient);
        ingredientsChanged = true;
        PlayerPrefs.SetString("ingredients", string.Join(",", ingredients.ToArray()));
    }

    public void markAsFavorite(string id) {
        favoriteRecipes.Add(id);
        PlayerPrefs.SetString("favorites", string.Join(",", favoriteRecipes.ToArray()));
    }

    public void removeFavorite(string id)
    {
        favoriteRecipes.Remove(id);
        PlayerPrefs.SetString("favorites", string.Join(",", favoriteRecipes.ToArray()));
    }

    public bool isFavorite(string id)
    {
        return favoriteRecipes.Contains(id);
    }

    public bool hasIngredient(string ingredient) {
        return ingredients.Contains(ingredient);
    }

    private void newButton(string name)
    {
        GameObject button = (GameObject)Instantiate(Resources.Load("myIngredientButton"), Vector3.zero, Quaternion.identity);
        Text txt = button.GetComponentInChildren<Text>();
        txt.text = name;
        Button b = button.GetComponent<Button>();
        b.name = name;
        RectTransform rt = myIngredientsGrid.GetComponent<RectTransform>();
        b.onClick.AddListener(() =>
        {
            ingredients.Remove(b.name);
            GameObject.Destroy(b.gameObject);
            PlayerPrefs.SetString("ingredients", string.Join(",", ingredients.ToArray()));
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y - 250);
        });
        button.transform.SetParent(myIngredientsGrid.transform);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 250);
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}