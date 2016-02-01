using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class webParser : MonoBehaviour{
    public static webParser Instance;
    private static string url = "";
    public static main.recipeLoaded callback;

    public void Awake(){
        Instance = this;
    }

    public void parse(string u, main.recipeLoaded c)
    {
      StartCoroutine(JSONClient.Get(url, recipeCallback));
      url = u;
      callback = c;
    }

    private static void recipeCallback(JSONObject response) {
        getURLBase(url);

        callback(new List<ingredient>(),"");
    }

    private static string getURLBase(string url) {
        string[] urlSplit = url.Split(new string[] { "://" }, StringSplitOptions.None);
        return urlSplit[1].Split('/')[0];
    }
}