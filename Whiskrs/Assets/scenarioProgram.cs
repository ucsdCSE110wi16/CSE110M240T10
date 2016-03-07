using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/**************************************************
 * This is the main testing framework for Whiskr
 * 
 * Scenario Testing:
 * Several key panel change button scenarios are tested
 * 
 * Unit Testing:
 * Adding ingredients
 * Removing ingredients
 * Parsing multiple websites
 **************************************************/

public class scenarioProgram : MonoBehaviour {
    /* Similar to assertEquals */
    delegate bool expectedOutcome();

    /* So it doesn't run all test if one fails */
    private static bool canContinue = true;

    /* Singleton */
    public static scenarioProgram e;

    /* Test Completed Bools, used only for Inspector */
    public bool pantryButtonTest = false;
    public bool homeButtonFromPantryTest = false;
    public bool recipesButtonTest = false;
    public bool homeButtonFromRecipesTest = false;

    public bool addIngredientTest = false;
    public bool removeIngredientTest = false;

    /* Parsing Tests */
    public List<string> websitesParsed;
    private List<expectedOutcome> websiteParsingFunctions;
    private static int testIndex = -1;

	/* Set the singleton for testing and feedback, make sure the game object stays around
     * Load the app
     * Set to run tests */
	void Start () {
        e = this;
        websitesParsed = new List<string>();
        Object.DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Scene");
        testObjects.runTests = true;
    }

    /* Testing program
     * General template:
     *      test(boolean function to test, success message, error message)
     */
    public static void runTest()
    {
        e.websiteParsingFunctions = new List<expectedOutcome>();
        e.websiteParsingFunctions.Add(webParseTesting.Instance.foodTester);
        e.websiteParsingFunctions.Add(webParseTesting.Instance.marthaStewartTester);
        e.websiteParsingFunctions.Add(webParseTesting.Instance.foodNetworkTester);
        e.websiteParsingFunctions.Add(webParseTesting.Instance.epicuriousTester);
        e.websiteParsingFunctions.Add(webParseTesting.Instance.delishTester);
        e.websiteParsingFunctions.Add(webParseTesting.Instance.wegmansTester);
        writeMessage("Testing Pantry Button...");
        test(
            delegate{
                return e.pantryButtonTest = testButton(testObjects.e.openPantryButton, delegate
                {
                    return isOpen(testObjects.e.pantryPanel) && isClosed(testObjects.e.homePanel);
                });
            }
        , "Pantry Button opens Pantry", "Pantry button unsuccessful");
        writeMessage("Testing Home Button in Pantry...");
        test(
            delegate
            {
                return e.homeButtonFromPantryTest = testButton(testObjects.e.openHomeButton_FromPantry, delegate
                {
                    return isClosed(testObjects.e.pantryPanel) && isOpen(testObjects.e.homePanel);
                });
            }
        , "Home Button closes Pantry", "Home button from pantry unsuccessful");
        writeMessage("Testing Recipes Button...");
        test(
            delegate
            {
                return e.recipesButtonTest = testButton(testObjects.e.openRecipesButton, delegate
                {
                    return isOpen(testObjects.e.recipesPanel) && isClosed(testObjects.e.homePanel);
                });
            }
        , "Recipes Button opens Recipes", "Recipes button unsuccessful");
        writeMessage("Testing Home Button in Recipes...");
        test(
            delegate
            {
                return e.homeButtonFromRecipesTest = testButton(testObjects.e.openHomeButton_FromRecipes, delegate
                {
                    return isClosed(testObjects.e.recipesPanel) && isOpen(testObjects.e.homePanel);
                });
            }
        , "Home Button closes Recipes", "Home button from Recipes unsuccessful");

        /* Running unit tests */
        ingredient testIngredient = new ingredient("Test Ingredient");
        main.Instance.ingredientsManager.addIngredient(testIngredient);
        foreach (ingredient i in main.Instance.ingredientsManager.ingredients)
        {
            if (i.name == "Test Ingredient")
            {
                e.addIngredientTest = true;
                break;
            }
        }
        e.removeIngredientTest = main.Instance.ingredientsManager.removeIngredient(testIngredient);
        runNextWebParsingTests();
	}

    public static void runNextWebParsingTests()
    {
        if (testIndex < e.websiteParsingFunctions.Count - 1) {
            e.websiteParsingFunctions[++testIndex]();
        }
    }

    /* Prints results of test, may need to also write to text file */
    private static void writeMessage(string message)
    {
        Debug.Log(message);
    }

    private static bool isOpen(GameObject gO)
    {
        return gO.activeInHierarchy && (gO.GetComponent<RectTransform>().localScale == new Vector3(1, 1, 1));
    }

    private static bool isClosed(GameObject gO)
    {
        return !gO.activeInHierarchy || (gO.GetComponent<RectTransform>().localScale == new Vector3(0, 0, 0));
    }

    private static void test(expectedOutcome function, string success, string error)
    {
        if (canContinue)
        {
            if (function())
                writeMessage(success);
            else
            {
                writeMessage(error);
                canContinue = false;
            }
        }
    }

    private static bool testButton(Button button, expectedOutcome expected)
    {
        button.onClick.Invoke();
        return expected();
    }
}
