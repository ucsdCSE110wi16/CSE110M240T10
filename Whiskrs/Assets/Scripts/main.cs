using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class main : MonoBehaviour
{
    public static main Instance { get; private set; }
    public IngredientsManager ingredientsManager;
    public GameObject myIngredientsGrid;
    public delegate void recipeLoaded(List<ingredient> ingredients, string directions);
    public string[] favorites;
    public List<string> favoriteRecipes;
    private bool firstStart = false;

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
        }
    }

    public void markAsFavorite(string id) {
        favoriteRecipes.Add(id);
        PlayerPrefs.SetString("favorites", string.Join(",", favoriteRecipes.ToArray()));
    }

    public void removeFavorite(string id)
    {
        favoriteRecipes.Remove(id);
        PlayerPrefs.SetString("favorites", string.Join(",", favoriteRecipes.ToArray()));
    }

    public bool isFavorite(string id)
    {
        return favoriteRecipes.Contains(id);
    }
    
}