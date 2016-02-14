using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class IngredientsManager {

    private bool isInitialized = false;
    public List<string> ingredients;
    public List<string> selectedIngredients;
    public GameObject myIngredientsGrid;
    public bool ingredientsChanged = true;

    public IngredientsManager()
    {
        ensureInitialized();
    }

    public IngredientsManager(bool init)
    {
        if (init)
            ensureInitialized();
    }

    public IngredientsManager(GameObject myGrid)
    {
        myIngredientsGrid = myGrid;
        ensureInitialized();
    }

    // Returns whether it was initialized already or not.
    // If not initialized, it will initialize.
    public bool ensureInitialized()
    {
        if (!isInitialized)
        {
            initialize();
            return false;
        }
        else
        {
            return true;
        }

    }

    public void initialize()
    {
        ingredients = new List<string>();
        string[] strArr = PlayerPrefs.GetString("ingredients").Split(',');
        foreach (string s in strArr)
        {
            if (s != "")
                addIngredient(s);
        }
        selectedIngredients = new List<string>();
        isInitialized = true;
    }

    public void addIngredient(string ingredient)
    {
        ingredients.Add(ingredient);
        newButton(ingredient);
        ingredientsChanged = true;
        PlayerPrefs.SetString("ingredients", string.Join(",", ingredients.ToArray()));
    }

    public bool hasIngredient(string ingredient)
    {
        return ingredients.Contains(ingredient);
    }

    public void unselectAll()
    {
        selectedIngredients = new List<string>();
    }

    public void selectAll()
    {
        selectedIngredients = new List<string>(ingredients);
    }

    private void newButton(string name)
    {
        GameObject button = (GameObject)MonoBehaviour.Instantiate(Resources.Load("myIngredientButton"), Vector3.zero, Quaternion.identity);
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
