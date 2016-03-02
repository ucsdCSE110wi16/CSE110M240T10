using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class testObjects : MonoBehaviour {
    public static testObjects e;
    public static bool runTests;

    /* Buttons */
    public Button openPantryButton;
    public Button openRecipesButton;
    public Button openHomeButton_FromPantry;
    public Button openHomeButton_FromRecipes;
    public Button openHomeButton_FromRecipeViewer;
    public Button openHomeButton_FromFavorites;
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
