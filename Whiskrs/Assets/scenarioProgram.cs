using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	/* Set the singleton for testing and feedback, make sure the game object stays around
     * Load the app
     * Set to run tests */
	void Start () {
        e = this;
        Object.DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Scene");
        testObjects.runTests = true;
    }

    /* Testing program
     * General template:
     *      test(boolean function to test, success message, error message)
     */
    public static void runTest(){         
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
