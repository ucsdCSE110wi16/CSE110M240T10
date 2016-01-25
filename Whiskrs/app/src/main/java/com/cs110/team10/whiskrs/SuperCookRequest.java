package com.cs110.team10.whiskrs;

import android.os.AsyncTask;

import com.goebl.david.Response;
import com.goebl.david.Webb;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONTokener;


/**
 * Created by John on 1/21/2016.
 */
public class SuperCookRequest {

    public static JSONObject getRecipes(String kitchen){
        Webb webb = Webb.create();
        webb.setBaseUri("http://www.supercook.com");
        Response<JSONObject> result;
        try {
            String jsonObject = toURLString(kitchen);
            result = webb.post("/dyn/results").body(jsonObject).asJsonObject();
            return result.getBody();
        } catch (Exception ex) {
            return null;
        }
    }

    private static String toURLString(String kitchen){
        String json = "needsimage=1&kitchen=" + kitchen.toLowerCase().replaceAll(" ", "+").replaceAll(",","%2C") + "&focus=&kw=&catname=&exclude=&start=0";
        return json;
    }

    private String replaceAll(String in, String match){
        while(in.indexOf(match) > -1){
            in = in.replace(in,match);
        }
        return in;
    }
}