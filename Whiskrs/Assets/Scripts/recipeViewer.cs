using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class recipeViewer : MonoBehaviour {
    public Text ingredientsObject;
    public Text directionsObject;
    public RawImage imageObject;
    public Text nameObject;
    public static recipeViewer Instance;

    void Awake()
    {
        Instance = this;
    }

    public void setImage(Texture2D img) {
        imageObject.texture = img;
    }

    public void draw(recipe result) {
        main.Instance.openPanel(gameObject);
        main.Instance.closePanel(favoritesPanel.Instance.gameObject);
        if (result.img != null) setImage(result.img);
        ingredientsObject.text = string.Join(",", result.ingredients.ToArray());
        directionsObject.text = result.directions;
        nameObject.text = result.name;
    }
}
