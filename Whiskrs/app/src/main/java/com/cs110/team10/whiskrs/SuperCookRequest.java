package com.cs110.team10.whiskrs;

import com.goebl.david.Response;
import com.goebl.david.Webb;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;


/**
 * Created by John on 1/21/2016.
 */
public class SuperCookRequest {
    int needsimage;
    String[] kitchen;
    int start = 0;

    public SuperCookRequest(String[] ingredients){
        needsimage = 1;
        kitchen = ingredients;
        start = 0;
    }

    public JSONObject getRecipes(){
        Webb webb = Webb.create();
        webb.setBaseUri("http://www.supercook.com");
        Response<JSONObject> result;
        try {
            result = webb.post("/dyn/results").body(this.toJSONObject()).asJsonObject();
            return result.getBody();
        } catch (Exception ex) {
            return null;
        }
    }

    private JSONObject toJSONObject(){
        String json = "{\"needsimage\":" + needsimage + ",";
        json += "\"kitchen\":[";
        for(int i=0;i<kitchen.length;i++){
            json += kitchen[i];
            if(i < kitchen.length -1)
                json += ",";
        }
        json += "],\"start\":" + start + "}";
        try {
            return (JSONObject) new JSONTokener(json).nextValue();
        }
        catch (JSONException e) {
            return null;
        }
    }
}
