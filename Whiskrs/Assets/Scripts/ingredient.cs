using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ingredient  {
    public string name;
    public Texture2D img;
    public GameObject button;
    public GameObject checkMark;
    public GameObject trashMark;

    public ingredient(string name, GameObject b = null,  Texture2D img = null) {
        this.name = name;
        this.button = b;
        this.img = img;
    }

    public static implicit operator string(ingredient i)
    {
        return i.name;
    }

    public static implicit operator ingredient(string name)
    {
        return new ingredient(name);
    }

    public override bool Equals(object ing)
    {
        if(ing.GetType() == typeof(ingredient))
        {
            ingredient myIng = ing as ingredient;
            return (name == myIng.name);
        }
        return false;
    }
}
