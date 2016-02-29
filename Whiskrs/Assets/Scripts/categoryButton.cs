using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class categoryButton : MonoBehaviour {
    string catName = "";
    string type;
    public Text title;
    private bool active = false;

    public void initialize(string s, string type) {
        catName = s;
        title.text = catName;
        this.type = type;
        gameObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            active = !active;
            main.Instance.ingredientsChanged();
            select();
            if (this.type == "meals")
                main.Instance.mealtype = active?catName:"";
            else if (this.type == "cuisines")
                main.Instance.cuisine = active ? catName : "";
            else
            {
                if (active)
                    main.Instance.restrictions.Add(catName);
                else
                    main.Instance.restrictions.Remove(catName);
            }
        });
    }

    private void select() {
        if(this.type != "restrictions")
            clearAll();
        else if(!active)
            this.gameObject.GetComponentInChildren<RawImage>().color = new Color(0, 0, 0, 0);
        if(active)
            this.gameObject.GetComponentInChildren<RawImage>().color = new Color(1,1,1,1);
    }

    private void clearAll() {
        Button[] siblings = gameObject.transform.parent.GetComponentsInChildren<Button>();
        foreach (Button b in siblings) {
            b.gameObject.GetComponentInChildren<RawImage>().color = new Color(0, 0, 0, 0);
        }
    }
}