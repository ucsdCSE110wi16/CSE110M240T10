using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class tutorial : MonoBehaviour {
    public static tutorial instance;
    public string[] instructionTexts;
    public Sprite[] instructionImages;
    public Image img;
    public Text txt;
    private int index = 0;

    void Awake() {
        instance = this;
    }

    public void nextInstruction() {
        if (index < instructionTexts.Length - 1)
        {
            img.sprite = instructionImages[++index];
            txt.text = instructionTexts[index];
        }
        else {
            main.Instance.closePanel(this.gameObject);
        }
    }
    public void previousInstruction()
    {
        if (index > 0)
        {
            img.sprite = instructionImages[--index];
            txt.text = instructionTexts[index];
        }
        else
        {
            main.Instance.closePanel(this.gameObject);
        }
    }
}
