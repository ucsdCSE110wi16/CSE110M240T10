using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class favoritesPanel : MonoBehaviour {
    public GameObject resultGrid;
    public int BUTTON_HEIGHT = 250;
    public static favoritesPanel Instance;
    public List<KeyValuePair<string,GameObject>> favorites;

    void Awake() {
        Instance = this;
        favorites = new List<KeyValuePair<string,GameObject>>();
    }

    public void drawFavorite(string url, string id, string title)
    {
        SuperCookRecipe r = new SuperCookRecipe();
        r.url = url; r.id = int.Parse(id); r.title = title;
        GameObject button = (GameObject)Instantiate(Resources.Load("RecipeButton 1"), Vector3.zero, Quaternion.identity);
        button.GetComponent<recipeButton>().initialize(r);

        button.transform.SetParent(resultGrid.transform);
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        shiftResultGrid();
        favorites.Add(new KeyValuePair<string,GameObject>(id,button));
    }

    public void removeFavorite(string id) {
        foreach (KeyValuePair<string, GameObject> kvp in favorites) {
            if (kvp.Key == id) {
                GameObject.Destroy(kvp.Value);
                shrinkGrid();
                favorites.Remove(kvp);
                return;
            }
        }
    }

    private void shiftResultGrid()
    {
        RectTransform rt = resultGrid.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + BUTTON_HEIGHT);
    }

    private void shrinkGrid()
    {
        RectTransform rt = resultGrid.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y - BUTTON_HEIGHT);
    }

    public void togglePanel()
    {
        if (this.gameObject.GetComponent<RectTransform>().localScale.x == 0)
        {
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else
        {
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        }
    }

    internal void closePanel()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }
}
