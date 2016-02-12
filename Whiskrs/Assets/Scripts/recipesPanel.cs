using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class recipesPanel : MonoBehaviour
{
    public SuperCookResult result;
    public GameObject resultGrid;
    public static recipesPanel Instance { get; private set; }
    private int index;
    public int numToDisplay = 40;

    void Awake()
    {
        Instance = this;
    }

    public void display() {
        if (main.Instance.ingredientsChanged)
            searchRecipes();
    }

    private void clearGrid()
    {
        foreach (Transform child in resultGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        RectTransform rt = resultGrid.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
    }

    private void searchRecipes()
    {
        main.Instance.ingredientsChanged = false;
        clearGrid();
        SuperCook.Instance.getRecipes(main.Instance.ingredients, callback);
    }

    void callback(SuperCookResult result)
    {
        this.result = result;
        index = 0;
        draw();
    }

    public void handleScroll(Vector2 change)
    {
        if (result != null)
        {
            if (change.y == 1 && index != 0)
                drawPrevious();
            else if (change.y == 0)
                drawNext();
        }
    }

    private void drawNext()
    {
        index += numToDisplay;
        draw();
    }

    private void drawPrevious()
    {
        index = (index - numToDisplay > 0) ? (index - numToDisplay) : 0;
        draw();
    }

    private void draw()
    {
        if (result.results.Count > 0)
        {
            foreach (SuperCookRecipe recipe in result.results)
            {
                string res = (main.Instance.isFavorite(recipe.id.ToString())) ? "RecipeButton 1" : "RecipeButton";
                GameObject button = (GameObject)Instantiate(Resources.Load(res), Vector3.zero, Quaternion.identity);
                Text txt = button.GetComponentInChildren<Text>();
                RawImage img = button.GetComponentInChildren<RawImage>();
                Toggle favButton = button.GetComponentInChildren<Toggle>();
                txt.text = recipe.title;
                Button b = button.GetComponent<Button>();
                b.onClick.AddListener(delegate { openWebPage(recipe.url); });
                favButton.onValueChanged.AddListener(delegate(bool val) {
                    if (!val)
                    {
                        main.Instance.removeFavorite(recipe.id.ToString());
                    }
                    else
                    {
                        main.Instance.markAsFavorite(recipe.id.ToString());                        
                    }
                });
                StartCoroutine(JSONClient.GetImage("http://www.supercook.com/thumbs/" + recipe.id + ".jpg", imageCallback, img));
                button.transform.SetParent(resultGrid.transform);
                RectTransform rt = resultGrid.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 250);
                button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void openWebPage(string url)
    {
        //StartCoroutine(JSONClient.Get(url, recipeCallback));
        Application.OpenURL(url);
    }

    void imageCallback(Texture2D img, object place)
    {
        ((RawImage)place).texture = img;
    }
}
