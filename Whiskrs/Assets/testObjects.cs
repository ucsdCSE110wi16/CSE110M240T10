using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class testObjects : MonoBehaviour {
    public static testObjects e;
    public static bool runTests;

    /* Buttons on Menu*/
    public Button openPantryButton;
    public Button openRecipesButton;
    public Button openFavoritesButton;
    /* Go Home Buttons */
    public Button openHomeButton_FromPantry;
    public Button openHomeButton_FromRecipes;
    public Button openHomeButton_FromRecipeViewer;
    public Button openHomeButton_FromFavorites;
    /* Go To Pantry Buttons */
    public Button openPantryButton_FromRecipes;
    public Button openPantryButton_FromSearchIngredients;
    /* Go To Recipes Buttons */
    public Button openRecipesButton_FromPantry;
    public Button openRecipesButton_FromRecipe;
    public Button openRecipesButton_FromFavorites;
    public Button openRecipesButton_FromFilters;
    /* Go To Filters Buttons */
    public Button openFiltersButton_FromRecipes;
    /* Go To Search Ingredients */
    public Button openSearchIngredients_FromPantry;
    /* Panels */
    public GameObject pantryPanel;
    public GameObject homePanel;
    public GameObject recipesPanel;
    public GameObject recipeViewerPanel;
    public GameObject favoritesPanel;
    public GameObject ingredientSelectPanel;
    public GameObject filterPanel;

    void Start() {
        e = this;
        if (runTests)
            scenarioProgram.runTest();
    }
}
