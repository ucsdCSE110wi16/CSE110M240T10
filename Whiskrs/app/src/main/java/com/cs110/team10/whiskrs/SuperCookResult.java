package com.cs110.team10.whiskrs;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;

/**
 * Created by John on 1/21/2016.
 */
public class SuperCookResult {
    int c = 1;
    String category = "";
    String hash= "results_773411589";
    int index= 0;
    ArrayList<SuperCookRecipe> results;
    int total_can_make_right_now=0;
    int total_results=0;

    public SuperCookResult(JSONObject jsonObject){
        c = jsonObject.optInt("c");
        category = jsonObject.optString("category");
        hash = jsonObject.optString("hash");
        index = jsonObject.optInt("index");
        JSONArray jA = jsonObject.optJSONArray("results");
        results = new ArrayList<SuperCookRecipe>();
        for(int i=0;i<jA.length();i++){
            results.add(i,new SuperCookRecipe(jA.optJSONObject(i)));
        }
        total_can_make_right_now = jsonObject.optInt("total_can_make_right_now");
        total_results = jsonObject.optInt("total_results");
    }
}
