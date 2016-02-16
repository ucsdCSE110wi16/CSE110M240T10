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
                Text txt = button.GetComponentInChildren<Text>();
                RawImage img = button.GetComponentInChildren<RawImage>();
                Toggle favButton = button.GetComponentInChildren<Toggle>();
                txt.text = recipe.title;
                Button b = button.GetComponent<Button>();
                b.onClick.AddListener(delegate
                {
                    StartCoroutine(JSONClient.GetImage("http://www.supercook.com/thumbs/" + recipe.id + ".jpg", setRecipeImage, null));
                    webParser.Instance.parse(recipe.url, buttonClickCallback);
                });
                favButton.onValueChanged.AddListener(delegate(bool val)
                {
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

    private void buttonClickCallback(recipe result, string url)
    {
        if (result == null)
            Application.OpenURL(url);
        else {
            recipeViewer.Instance.draw(result);
        }
    }

    void imageCallback(Texture2D img, object place)
    {
        ((RawImage)place).texture = img;
    }

    void setRecipeImage(Texture2D img, object place)
    {
        recipeViewer.Instance.setImage(img);
    }
}
