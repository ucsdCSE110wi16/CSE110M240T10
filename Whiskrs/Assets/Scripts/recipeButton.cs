using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class recipeButton : MonoBehaviour {
    public SuperCookRecipe recipe;

    public void initialize(SuperCookRecipe recipe) {
        this.recipe = recipe;
        Text txt = gameObject.GetComponentInChildren<Text>();
        RawImage img = gameObject.GetComponentInChildren<RawImage>();
        Toggle favButton = gameObject.GetComponentInChildren<Toggle>();
        txt.text = recipe.title;
        Button b = gameObject.GetComponent<Button>();
        b.onClick.AddListener(delegate
        {
            StartCoroutine(JSONClient.GetImage("http://www.supercook.com/thumbs/" + this.recipe.id + ".jpg", setRecipeImage, null));
            webParser.Instance.parse(this.recipe.url, buttonClickCallback);
        });
        favButton.onValueChanged.AddListener(delegate(bool val)
        {
            if (!val)
            {
                main.Instance.removeFavorite(recipe.id.ToString(), recipe.url, recipe.title);
            }
            else
            {
                main.Instance.markAsFavorite(recipe.id.ToString(), recipe.url, recipe.title);
            }
        });
        StartCoroutine(JSONClient.GetImage("http://www.supercook.com/thumbs/" + recipe.id + ".jpg", imageCallback, img));
    }

    private void buttonClickCallback(recipe result, string url)
    {
        if (result == null)
            Application.OpenURL(url);
        else
        {
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
