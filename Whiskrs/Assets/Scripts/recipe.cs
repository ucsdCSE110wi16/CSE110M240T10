using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class recipe {
    public string name;
    public Texture2D img;
    public List<ingredient> ingredients;
    public string directions;

    public recipe(string nm, List<ingredient> ing, string dir, Texture2D i) {
        name = nm;
        ingredients = ing;
        directions = dir;
        img = i;
    }
}
