using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

public class webParser : MonoBehaviour{
    public static webParser Instance;
    public delegate void recipeLoaded(recipe result, string url);
    private recipeLoaded func;

    public void Awake(){
        Instance = this;
    }

    /*
     * Send the url and downloaded html to web parser and it handles the rest
     */
    public void parse(string url, recipeLoaded func) {
        this.func = func;
        StartCoroutine(JSONClient.GetHTML(url,callback));
    }

    private static void callback(string url, string html){
        string baseURL = getURLBase(url);
        recipe result = null;
        Debug.Log(baseURL);
        switch (baseURL)
        {
            case "www.myrecipes.com":
                result = parseMyRecipes(html);
                break;
            case "www.food.com":
                result = parseFoodDotCom(html);
                break;
            case "allrecipes.com":
                result = parseAllRecipes(html);
                break;
        }
        webParser.Instance.func(result, url);
    }

    /*
     * Example parse (it is funcitonal)
     * Gets name, ingredients, and directions based on itemprop attribute used by MyRecipes.com
     */
    private static recipe parseMyRecipes(string html)
    {
        string name = removeTags(getElementsByAttr(html, "div", "itemprop", "name")[0]);
        string directions = removeTags(getElementsByAttr(html, "div", "itemprop", "instructions")[0]);
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "div", "itemprop", "ingredient")) {
            ingredients.Add(removeTags(ing));
        }
        return new recipe(name, ingredients, directions, null);
    }

    private static recipe parseAllRecipes(string html) {
        string name = removeTags(getElementsByAttr(html, "div", "itemprop", "name")[0]);
        string directions = removeTags(getElementsByAttr(html, "div", "itemprop", "recipeInstructions")[0]);
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "div", "itemprop", "ingredients"))
        {
            string initial = removeTags(ing);
            initial = initial.Split('<')[0];
            ingredients.Add(initial);
        }
        return new recipe(name, ingredients, directions, null);
    }

    /*
     * Gets name, ingredients, and directions from Food.com
     */
    private static recipe parseFoodDotCom(string html)
    {
        // Parse the name of the recipe
        string name = removeTags(getElementsByAttr(html, "h1", "class", "fd-recipe-title")[0]);

        // Parse the directions
        string directions = removeTags(getElementsByAttr(html, "ol", "class", "expanded")[0]);

        // Parse the ingredients
        List<string> ingredients = new List<String>();
        string ingredientsHtml = getElementsByAttr (html, "div", "data-module", "ingredients")[0];
        foreach (string ing in getElementsByAttr(ingredientsHtml, "li", "data-ingredient", "*")) {
            ingredients.Add (removeTags(ing));
        }

        // Return the recipe object
        return new recipe(name, ingredients, directions, null);
    }

    /*
     * Returns everything inside of the <body></body>
     */
    private static string getBody(string html)
    {
        int index = html.IndexOf("<body");
        int endOfBody = html.IndexOf("</body>") + 7;
        html = html.Substring(index, endOfBody - index);
        return html;
    }

    /*
     * Removes all new lines from html
     */
    private static string removeNewline(string html) {
        return Regex.Replace(html, @"\t|\n|\r", "");
    }

    /*
     * !!! Most used !!!
     * Search for a specific div type with attribute and value
     */
    private static List<string> getElementsByAttr(string html, string divType, string attr, string val) {
        List<string> divs = new List<string>();
        string query;

        // Wildcard val
        if (val.Equals ("*")) {
            query = attr;
        } else {
            query = attr + "=\"" + val + "\"";
        }
        while (html.Contains(query))
        {
            int startIndex = html.IndexOf(query);
            int i = startIndex;
            /* find beginning of div content */
            while (html[i] != '>')
            {
                i++;
            }
            int j = ++i;
            string tmp = html.Substring(j);
            j = tmp.IndexOf("</" + divType + ">") + 6;
            while (tmp.IndexOf("<" + divType + "") < tmp.IndexOf("</" + divType + ">"))
            {
                j = tmp.IndexOf("</" + divType + ">") + 6;
                tmp = tmp.Substring(tmp.IndexOf("</" + divType + ">") + 6);
            }
            divs.Add(html.Substring(i, j));
            html = html.Substring(i + j);
        }
        return divs;
    }

    /*
     * Remove everything inside of a <script></script>
     */
    private static string removeScript(string html) {
        return removeBlock(html, "<script", "</script>");
    }

    /*
     * Remove the tags themselves and leave only the inner HTML
     */
    private static string removeTags(string html) {
        return Regex.Replace(Regex.Replace(html, @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ");
    }

    /*
     * Remove all type of content between an opening and closing tag, dangerous, may crash computer if not careful
     */
    private static string removeBlock(string html, string removeOpen, string removeClose)
    {
        int i = 0;
        while (html.Contains(removeOpen) && i < 100)
        {
            int index = html.IndexOf(removeOpen);
            string tmp = html.Substring(0, index);
            int endOfScript = html.IndexOf(removeClose) + removeClose.Length;
            html = tmp + html.Substring(endOfScript);
            i++;
        }
        return html;        
    }

    /*
     * Returns the Base URL ex. www.github.com
     */
    private static string getURLBase(string url)
    {
        string[] urlSplit = url.Split(new string[] { "://" }, StringSplitOptions.None);
        return urlSplit[1].Split('/')[0];
    }
}
