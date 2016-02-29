using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class recipe {
    public string name;
    public Texture2D img;
    public List<string> ingredients;
    public string directions;
    public string url;

    public recipe(string nm, List<string> ing, string dir, Texture2D i)
    {
        name = nm;
        ingredients = ing;
        directions = dir;
        img = i;
    }
}
