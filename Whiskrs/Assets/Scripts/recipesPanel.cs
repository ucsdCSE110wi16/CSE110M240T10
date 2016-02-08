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

    private void clearGrid()
    {
        foreach (Transform child in resultGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void searchRecipes()
    {
        clearGrid();
        SuperCook.Instance.getRecipes(main.Instance.ingredients, callback);
    }

    void callback(SuperCookResult result)
    {
        this.result = result;
        Debug.Log(this.result.total_can_make_right_now);
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
                GameObject button = (GameObject)Instantiate(Resources.Load("RecipeButton"), Vector3.zero, Quaternion.identity);
                Text txt = button.GetComponentInChildren<Text>();
                RawImage img = button.GetComponentInChildren<RawImage>();
                txt.text = recipe.title;
                Button b = button.GetComponent<Button>();
                b.onClick.AddListener(delegate { openWebPage(recipe.url); });
                StartCoroutine(JSONClient.GetImage("http://www.supercook.com/thumbs/" + recipe.id + ".jpg", imageCallback, img));
                button.transform.SetParent(resultGrid.transform);
                RectTransform rt = resultGrid.GetComponent<RectTransform>();
                rt.Translate(new Vector3(0, -211, 0));
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
