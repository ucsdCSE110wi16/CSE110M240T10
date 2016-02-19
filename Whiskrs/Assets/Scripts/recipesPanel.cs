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
    public List<GameObject> recipeButtons;
    public int BUTTON_HEIGHT = 250;

    void Awake()
    {
        Instance = this;
    }

    public void display() {
        if (main.Instance.ingredientsManager.ingredientsChanged)
            searchRecipes();
    }

    private void clearGrid()
    {
        foreach (Transform child in resultGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        RectTransform rt = resultGrid.GetComponent<RectTransform>();
        recipeButtons = new List<GameObject>();
        rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
    }

    private void searchRecipes()
    {
        main.Instance.ingredientsManager.ingredientsChanged = false;
        clearGrid();
        if(main.Instance.ingredientsManager.selectedIngredients.Count > 0)
            SuperCook.Instance.getRecipes(main.Instance.ingredientsManager.selectedIngredients.ConvertAll<string>(x => x.name), callback);
    }

    void callback(SuperCookResult result)
    {
        this.result = result;
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
                button.GetComponent<recipeButton>().initialize(recipe);

                button.transform.SetParent(resultGrid.transform);
                button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                shiftResultGrid();
                recipeButtons.Add(button);
            }
        }
    }

    private void shiftResultGrid() {
        RectTransform rt = resultGrid.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + BUTTON_HEIGHT);
    }
}
