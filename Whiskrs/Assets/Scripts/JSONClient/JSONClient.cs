using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JSONClient{
    public delegate void responseDelegate(JSONObject response);
    public delegate void responseImageDelegate(Texture2D image, object place);
    public delegate void responseHTMLDelegate(string url, string html);

    public static IEnumerator Get(string url, responseDelegate responseFunction)
    {
        return Generic(url, "GET", null, responseFunction);
    }

    public static IEnumerator GetImage(string url, responseImageDelegate callback, object place){
        // Start a download of the given URL
        WWW www = new WWW(url);
        
		// Wait for download to complete
        yield return www;
        
		callback(www.texture, place);
    }

    public static IEnumerator GetHTML(string url, responseHTMLDelegate callback)
    {
        // Start a download of the given URL
        WWW www = new WWW(url);

        // Wait for download to complete
        yield return www;

        callback(url, www.text);
    }

    public static IEnumerator Post(string url, JSONObject payload, responseDelegate responseFunction, Dictionary<string, string> headers = null)
    {
        return Generic(url, "POST", payload, responseFunction, headers);
    }

    private static IEnumerator Generic(string url, string method, JSONObject payload, responseDelegate responseFunction,Dictionary<string,string> headers=null) {
        WWW w;
        if (method == "POST")
        {
            if (headers == null)
                w = new WWW(url, payload);
            else
            {
                WWWForm form = (WWWForm)payload;
                w = new WWW(url, form.data, headers);
            }
        }
        else
        {
            w = new WWW(url);
        }
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.Log(w.error);
        }
        else
        {
            responseFunction(new JSONObject(w.text));
        }
    }
}
