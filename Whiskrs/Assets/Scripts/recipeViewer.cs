using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class recipeViewer : MonoBehaviour {
    public Image imageObject;
    public Text recipeInstructions;
    public RectTransform scrollPanel;
    public static recipeViewer Instance;
    public int textSize = 77;
    public int imageSize = 330;
    int numLines = 0;
    public GameObject panel;

    public void toTop()
    {
        scrollPanel.sizeDelta = new Vector2(scrollPanel.sizeDelta.x, textSize * numLines + imageSize);
        scrollPanel.position = new Vector2(scrollPanel.position.x, 0);
    }

    void Awake()
    {
        Instance = this;
    }

    public void setImage(Texture2D img) {
        imageObject.sprite = Sprite.Create(img, new Rect(0,0,img.width,img.height),new Vector2(0.5f,0.5f));
    }

    public void draw(recipe result)
    {
        main.Instance.openPanel(panel);
        if (result.img != null) setImage(result.img);
        numLines = 0;
        recipeInstructions.text = "";
        addLine(result.name);
        addNewLine();
        addLine("Ingredients:");
        foreach(string ing in result.ingredients)
        {
            addLine(" - " + ing);
        }
        addNewLine();
        addLine("Directions:");
        addLine(result.directions);
        addNewLine();
        addLine("Source: " + result.url);
        toTop();
    }

    private void addLine(string text) {
        recipeInstructions.text += text + "\n";
        int width = (int)scrollPanel.rect.width;
        int textLength = textSize * text.Length;
        numLines += Mathf.CeilToInt(textLength / width) - 1;
    }

    private void addNewLine()
    {
        recipeInstructions.text += "\n";
        numLines++;
    }
}
