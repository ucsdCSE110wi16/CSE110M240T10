using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class recipesPanel : MonoBehaviour
{
    public SuperCookResult result;
    public GameObject resultGrid;
    public static recipesPanel Instance { get; private set; }
    private int index = 0;
    public List<GameObject> recipeButtons;
    public GameObject nextButton;
    public GameObject prevButton;

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
            child.gameObject.SetActive(false);
        }
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
        index = 0;
        draw();
    }

    private void draw()
    {
        if (result.results.Count > 0)
        {
            for (int i=index;i<index+4;i++)
            {
                SuperCookRecipe recipe = result.results[i];
                GameObject button = recipeButtons[i - index];
                button.SetActive(true);
                button.GetComponent<recipeButton>().initialize(recipe);
            }
        }
    }

    public void next() {
        if (index + 4 < result.results.Count)
        {
            index += 4;
            prevButton.SetActive(true);
        }
        else
        {
            index = result.results.Count - 1;
            nextButton.SetActive(false);
        }
        draw();
    }

    public void previous()
    {
        if (index - 4 >= 0)
        {
            index -= 4;
            nextButton.SetActive(true);
        }
        else
        {
            index = 0;
            prevButton.SetActive(false);
        }
        draw();
    }
}
