package com.cs110.team10.whiskrs;

import org.json.JSONObject;

/**
 * Created by John on 1/24/2016.
 */
public class SuperCookRecipe {
    int hasthumb = 1;
    int id = 4;
    String title= "";
    String url="";
    String uses= "";

    public SuperCookRecipe(JSONObject jO){
        hasthumb = jO.optInt("hasthumb");
        id = jO.optInt("id");
        title = jO.optString("title");
        url = jO.optString("url");
        uses = jO.optString("uses");
    }
}
