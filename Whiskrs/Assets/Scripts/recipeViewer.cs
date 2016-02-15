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

    public void draw(recipe result) {
        togglePanel();
        if (result.img != null) setImage(result.img);
        ingredientsObject.text = string.Join(",", result.ingredients.ToArray());
        directionsObject.text = result.directions;
        nameObject.text = result.name;
    }
}
