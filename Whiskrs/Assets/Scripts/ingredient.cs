using UnityEngine;
using System.Collections;

public class ingredient  {
    public string name;
    public Texture2D img;

    public ingredient(string name, Texture2D img = null) {
        this.name = name;
        this.img = img;
    }
}
