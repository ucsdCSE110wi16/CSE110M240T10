using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FallingFoodManager : MonoBehaviour {

    public GameObject[] foodItems;
    public Vector2[] foodItemVelocities;
    public Sprite[] foodImages;
    public const int FallingFoodCount = 10;
    public static Vector2 MinVelocity = new Vector2(-3, -3);
    public static Vector2 MaxVelocity = new Vector2(3, -7);
    public Transform FoodParent;

    // Use this for initialization
    void Start () {
        foodItems = new GameObject[FallingFoodCount];
        foodItemVelocities = new Vector2[FallingFoodCount];
        for(int i = 0; i < FallingFoodCount; i++)
        {
            instanciateNewFood(i);
        }
	}

    public GameObject createFoodItem()
    {
        GameObject fooditem = (GameObject)Instantiate(Resources.Load("food"), Vector3.zero, Quaternion.identity);
        fooditem.GetComponent<Image>().sprite = getRandomImage();
        fooditem.transform.SetParent(FoodParent);
        RectTransform itemTransform = fooditem.GetComponent<RectTransform>();
        fooditem.transform.position = new Vector3(
            Random.value * Screen.width,
            Screen.height + itemTransform.rect.height, 
            0);
        fooditem.transform.localScale = new Vector3(Screen.width / itemTransform.rect.width / 5, 
            Screen.width / itemTransform.rect.width / 5, 0);
        return fooditem;
    }

    public Vector2 getRandomVelocity()
    {
        return new Vector2(MinVelocity.x + (Random.value * (MaxVelocity.x - MinVelocity.x)),
            MinVelocity.y + (Random.value * (MaxVelocity.y - MinVelocity.y)));
    }

    public Sprite getRandomImage()
    {
        int randomIndex = Mathf.RoundToInt((foodImages.Length - 1) * Random.value);
        return foodImages[randomIndex];
    } 

    public void instanciateNewFood(int index)
    {
        foodItems[index] = createFoodItem();
        foodItemVelocities[index] = getRandomVelocity();
    }

    public void updateFoodPosition(int index)
    {
        foodItems[index].transform.Translate(new Vector3(
            foodItemVelocities[index].x, foodItemVelocities[index].y, 0));
    }

    public bool outsideScreen(int index)
    {
        RectTransform itemTransform = foodItems[index].GetComponent<RectTransform>();
        if (foodItems[index].transform.position.x < -itemTransform.rect.width)
            return true;
        else if (foodItems[index].transform.position.x > Screen.width + itemTransform.rect.width)
            return true;
        else if (foodItems[index].transform.position.y < -itemTransform.rect.height)
            return true;
        else
            return false;
    }
	
	// Update is called once per frame
	void Update () {
	    for(int i = 0; i < foodItems.Length; i++)
        {
            updateFoodPosition(i);
            if (outsideScreen(i))
            {
                GameObject.Destroy(foodItems[i]);
                instanciateNewFood(i);
            }
        }
	}
}
