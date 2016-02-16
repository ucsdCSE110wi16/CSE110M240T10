using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class IngredientsManager {

    private bool isInitialized = false;
    public List<ingredient> ingredients;
    public List<ingredient> selectedIngredients;
    public List<ingredient> trashedIngredients = new List<ingredient>();
    public GameObject myIngredientsGrid;
    public RectTransform rt;
    public bool ingredientsChanged = true;
    public bool trashMode = false;

    public IngredientsManager()
    {
        ensureInitialized();
    }

    public IngredientsManager(bool init)
    {
        if (init)
            ensureInitialized();
    }

    public IngredientsManager(GameObject myGrid)
    {
        myIngredientsGrid = myGrid;
        rt = myIngredientsGrid.GetComponent<RectTransform>();
        ensureInitialized();
    }

    // Returns whether it was initialized already or not.
    // If not initialized, it will initialize.
    public bool ensureInitialized()
    {
        if (!isInitialized)
        {
            initialize();
            return false;
        }
        else
        {
            return true;
        }

    }

    public void initialize()
    {
        ingredients = new List<ingredient>();

        // Load ingredients
        string[] strArr = PlayerPrefs.GetString("ingredients").Split(',');
        foreach (string s in strArr)
        {
            if (s != "")
                addIngredient(new ingredient(s));
        }
        selectedIngredients = new List<ingredient>();
        isInitialized = true;
    }

    public void addIngredient(ingredient ingred)
    {
        ingredients.Add(ingred);
        newButton(ingred);
        saveIngredients();
    }

    public bool hasIngredient(ingredient ingredient)
    {
        return ingredients.Contains(ingredient);
    }

    public void selectIngredient(ingredient ing)
    {
        selectedIngredients.Add(ing);
        ing.checkMark.SetActive(true);
        ingredientsChanged = true;
    }

    public void deselectIngredient(ingredient ing)
    {
        if (selectedIngredients.Remove(ing))
            ingredientsChanged = true;
        ing.checkMark.SetActive(false);
    }

    public void unselectAll()
    {
        updateSelected(false);
        if (trashMode)
        {
            trashedIngredients = new List<ingredient>();
        }
        else
        {
            selectedIngredients = new List<ingredient>();
        }
        ingredientsChanged = true;
    }

    public void selectAll()
    {
        if (trashMode)
        {
            trashedIngredients = new List<ingredient>(ingredients);
        }
        else
        {
            selectedIngredients = new List<ingredient>(ingredients);
        }
        updateSelected();
        ingredientsChanged = true;
    }

    public void updateSelected(bool select = true)
    {
        foreach(ingredient ing in selectedIngredients)
        {
            if (trashMode)
            {
                ing.checkMark.SetActive(false);
            }
            else
            {
                if (select)
                    ing.checkMark.SetActive(true);
                else
                    ing.checkMark.SetActive(false);
            }
        }
        if (trashMode)
        {
            foreach(ingredient ing in trashedIngredients)
            {
                if (select)
                    ing.trashMark.SetActive(true);
                else
                    ing.trashMark.SetActive(false);
            }
        }
    }

    private void newButton(ingredient ing)
    {
        string name = ing.name;
        GameObject button = (GameObject)MonoBehaviour.Instantiate(Resources.Load("myIngredientButton"), Vector3.zero, Quaternion.identity);
        ing.button = button;
        Text txt = button.GetComponentInChildren<Text>();
        txt.text = name;
        ing.checkMark = button.transform.GetChild(2).gameObject;
        ing.trashMark = button.transform.GetChild(3).gameObject;
        ing.checkMark.SetActive(false);
        ing.trashMark.SetActive(false);
        Button b = button.GetComponent<Button>();
        b.name = name;
        b.onClick.AddListener(() =>
        {
            if (trashMode)
            {
                if (trashedIngredients.Contains(ing))
                {
                    trashedIngredients.Remove(ing);
                    ing.trashMark.SetActive(false);
                }
                else
                {
                    trashedIngredients.Add(ing);
                    ing.trashMark.SetActive(true);
                }
            }
            else
            {
                if (selectedIngredients.Contains(ing))
                {
                    deselectIngredient(ing);
                }
                else
                {
                    selectIngredient(ing);
                }
            }
        });
        button.transform.SetParent(myIngredientsGrid.transform);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 250);
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void beginTrashMode()
    {
        trashMode = true;
        updateSelected();
    }

    public void endTrashMode()
    {
        // Clear the trashbin
        trashMode = false;
        foreach(ingredient ing in trashedIngredients)
        {
            if (selectedIngredients.Remove(ing))
                ingredientsChanged = true;
            removeIngredient(ing);
        }
        trashedIngredients = new List<ingredient>();
        updateSelected();
    }

    // Returns whether item was removed or not (I.E. was not in list anyway)
    public bool removeIngredient(ingredient ing)
    {
        bool itemRemoved = ingredients.Remove(ing);
        if (itemRemoved)
        {
            GameObject.Destroy(ing.button);
            saveIngredients();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y - 250);
        }
        return itemRemoved;
    }

    public void saveIngredients()
    {
        PlayerPrefs.SetString("ingredients", string.Join(",", ingredients.ConvertAll<string>(x => x.name).ToArray()));
    }

}
