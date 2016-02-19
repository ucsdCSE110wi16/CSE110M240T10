﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class main : MonoBehaviour
{
    public static main Instance { get; private set; }
    public IngredientsManager ingredientsManager;
    public GameObject myIngredientsGrid;
    public GameObject filterPanel;
    public GameObject cuisineGrid;
    public GameObject mealGrid;
    public GameObject restrictionGrid;
    public delegate void recipeLoaded(List<ingredient> ingredients, string directions);
    public string[] favorites;
    public List<string> favoriteRecipes;
    public Sprite[] trashImages;
    private bool firstStart = false;
    public string cuisine;
    public string mealtype;
    public List<string> restrictions;

    void Awake()
    {
        if (!firstStart)
        {
            firstStart = true;
            Instance = this;
            ingredientsManager = new IngredientsManager(myIngredientsGrid);
            favoriteRecipes = new List<string>();
            /*
            // Make sure we have an instance of webParser before attempting to use it.
            // May need only add component here since Instance is set in webParser Awake function
            webParser.Instance = gameObject.AddComponent<webParser>();
            webParser.Instance.parse("", delegate { });*/

            favorites = PlayerPrefs.GetString("favorites").Split(',');
            Debug.Log(string.Join(",", favorites));
            foreach (string s in favorites)
            {
                if (s != "") favoriteRecipes.Add(s);
            }
            loadCategories();
        }
    }

    public void markAsFavorite(string id) {
        if (!favoriteRecipes.Contains(id))
        {
            favoriteRecipes.Add(id);
            PlayerPrefs.SetString("favorites", string.Join(",", favoriteRecipes.ToArray()));
        }
    }

    public void removeFavorite(string id)
    {
        favoriteRecipes.Remove(id);
        PlayerPrefs.SetString("favorites", string.Join(",", favoriteRecipes.ToArray()));
    }

    private void loadCategories() {
        foreach (string s in categories.cuisines) {
            newCategoryButton(s, cuisineGrid, "cuisines");
        }
        foreach (string s in categories.meals)
        {
            newCategoryButton(s, mealGrid, "meals");
        }
        foreach (string s in categories.restrictions)
        {
            newCategoryButton(s, restrictionGrid, "restrictions");
        }
    }

    private void newCategoryButton(string s, GameObject grid, string category) {
        GameObject button = (GameObject)Instantiate(Resources.Load("categoryButton"), Vector3.zero, Quaternion.identity);
        button.GetComponent<categoryButton>().initialize(s, category);
        button.transform.SetParent(grid.transform);
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        shiftGridHorizontally(grid, 250);
    }

    private void shiftGridHorizontally(GameObject grid, int amount)
    {
        RectTransform rt = grid.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x + amount, rt.sizeDelta.y);
    }

    public bool isFavorite(string id)
    {
        return favoriteRecipes.Contains(id);
    }

    // These ar called on click from buttons in the ingredients Panel
    public void selectAllPassAlong()
    {
        ingredientsManager.selectAll();
    }
    public void unselectAllPassAlong()
    {
        ingredientsManager.unselectAll();
    }
    public void trashBinPassAlong(GameObject trashBin)
    {
        if (ingredientsManager.trashMode)
        {
            ingredientsManager.endTrashMode();
            trashBin.GetComponent<Image>().sprite = trashImages[0];
        }
        else
        {
            ingredientsManager.beginTrashMode();
            trashBin.GetComponent<Image>().sprite = trashImages[1];
        }
    }
    
}