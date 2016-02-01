using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class recipesPanel : MonoBehaviour
{
    public SuperCookResult result;
    public GameObject resultGrid;
    public static recipesPanel Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void searchRecipes()
    {
        foreach (Transform child in resultGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        // Trying to search with negative or 0 ingredients throws an error.
        if(main.Instance.ingredients.Count > 0)
            SuperCook.Instance.getRecipes(main.Instance.ingredients, callback);
        // else: Handle searching for recepies with no selected ingredients.
    }

    void callback(SuperCookResult result)
    {
        if (result.results.Count > 0)
        {
            // Can't call Coroutines in an inactive instance, make sure it's active
            if (Instance.isActiveAndEnabled) { 
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
    }

    private void openWebPage(string url)
    {
        Application.OpenURL(url);
    }

    void imageCallback(Texture2D img, object place)
    {
        ((RawImage)place).texture = img;
    }
}
