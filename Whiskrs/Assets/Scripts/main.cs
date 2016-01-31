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

    void Awake()
    {
        Instance = this;
        ingredients = new List<string>();
        webParser.Instance.parse("", delegate { });
    }

    public void addIngredient(string ingredient) {
        ingredients.Add(ingredient);
        newButton(ingredient);
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
        b.onClick.AddListener(() =>
        {
            ingredients.Remove(b.name);
            GameObject.Destroy(b.gameObject);
        });
        button.transform.SetParent(myIngredientsGrid.transform);
        RectTransform rt = myIngredientsGrid.GetComponent<RectTransform>();
        rt.Translate(new Vector3(0, -211, 0));
    }
}
