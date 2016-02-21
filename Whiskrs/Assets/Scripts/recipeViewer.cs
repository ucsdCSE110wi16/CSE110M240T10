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
        int ingLines = (int)((ingredientsObject.text.Length * 25) / Screen.width);
        RectTransform ingrt = ingredientsObject.gameObject.GetComponent<RectTransform>();
        ingrt.sizeDelta = new Vector2(ingrt.sizeDelta.x, ingLines * 40);

        directionsObject.text = result.directions;
        int dirLines = (int)((directionsObject.text.Length * 25) / Screen.width);
        RectTransform dirrt = directionsObject.gameObject.GetComponent<RectTransform>();
        dirrt.sizeDelta = new Vector2(dirrt.sizeDelta.x, dirLines * 40);

        nameObject.text = result.name;
    }
}
