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
        try
        {
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
                case "www.foodnetwork.com":
                    result = parseFoodNetwork(html);
                    break;
                case "www.marthastewart.com":
                    result = parseMartha(html);
                    break;
                case "www.epicurious.com":
                    result = parseEpicurious(html);
                    break;
                /*case "www.thekitchn.com":
                    result = parseTheKitchn(html);
                    break;*/
                case "www.delish.com":
                    result = parseDelish(html);
                    break;
                /*case "www.oprah.com":
                    result = parseOprah(html);
                    break;*/
                /*case "albertsons.mywebgrocer.com":
                    result = parseAlbertsons(html);
                    break;*/
                case "www.wegmans.com":
                    result = parseWegmans(html);
                    break;
                /*case "www.rachaelraymag.com":
                    result = parseRachaelRay(html);
                    break;
                case "www.traderjoes.com":
                    result = parseTraderJoes(html);
                    break;*/
                case "www.cookstr.com":
                    result = parseCookstr(html);
                    break;

            }
            result.url = url;
            webParser.Instance.func(result, url);
        }
        catch
        {
            webParser.Instance.func(null, url);
        }
    }

    /*//parse raychaelraymag.com
    private static recipe parseRachaelRay(string html)
    {
        string name = removeTags(getElementsByAttr(html, "div", "itemprop", "name")[0]);
        string instructions = removeTags(getElementsByAttr(html, "div", "class", "recipe-instructions")[0]);
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "li", "itemprop", "recipe-instructions"))
        {
            ingredients.Add(removeTags(ing));
        }

        return new recipe(name, ingredients, instructions, null);
    }

    //parse traderjoes.com
    private static recipe parseTraderJoes(string html)
    {
        string name = removeTags(getElementsByAttr(html, "div", "itemprop", "name")[0]);
        string directions = removeTags(getElementsByAttr(html, "div", "class", "article")[0]);
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "li", "itemprop", "bullet-list"))
        {
            ingredients.Add(removeTags(ing));
        }

        return new recipe(name, ingredients, directions, null);
    }*/

    //parse cookstr.com
    private static recipe parseCookstr(string html)
    {
        string name = removeTags(getElementsByAttr(html, "div", "itemprop", "name")[0]);
        string directions = removeTags(getElementsByAttr(html, "div", "itemprop", "recipeInstructions")[0]);
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "div", "itemprop", "ingredients"))
        {
            ingredients.Add(removeTags(ing));
        }
        return new recipe(name, ingredients, directions, null);
    }
    

    private static recipe parseMartha(string html)
    {
        string name = removeTags(getElementsByAttr(html, "div", "itemprop", "name")[0]);
        string directions = removeTags(getElementsByAttr(html, "div", "class", "directions-list")[0]);
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "li", "itemprop", "ingredients"))
        {
            ingredients.Add(removeTags(ing));
        }
        return new recipe(name, ingredients, directions, null);
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
            ingredients.Add(initial.Replace("ADVERTISEMENT",""));
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
        string directions = removeTags(getElementsByAttr(html, "div", "data-module", "recipeDirections")[0].Replace("Directions",""));
        // Parse the ingredients
        List<string> ingredients = new List<String>();
        foreach (string ing in getElementsByAttr(html, "li", "data-ingredient", "*")) {
            string s = removeTags(ing);
            if (s != "") ingredients.Add(s);
        }

        // Return the recipe object
        return new recipe(name, ingredients, directions, null);
    }

    /*
     * Gets name, ingredients, and directions from foodnetwork.com
     */
    private static recipe parseFoodNetwork(string html) {
        // Parse the name of the recipe
        string name = removeTags(getElementsByAttr(html, "h1", "itemprop", "name")[0]);

        // Parse the directions
        string directionsHtml = getElementsByAttr(html, "div", "class", "col10 directions")[0];
        string directions = "";

        // Last 2 paragraphs are not directions
        List<string> paragraphs = getElementsByTag(directionsHtml, "p");
        for (int i = 0; i < paragraphs.Count - 2; i++) {
            directions += removeTags (paragraphs [i]) + "\n\n";
        }

        // Parse the ingredients
        List<string> ingredients = new List<String>();
        foreach (string ing in getElementsByAttr(html, "li", "itemprop", "ingredients")) {
            ingredients.Add (removeTags(ing));
        }

        // Return the recipe object
        return new recipe(name, ingredients, directions, null);
    }

    /*
     * Gets name, ingredients, and directions from epicurious.com
     */
    private static recipe parseEpicurious(string html) {
        // Parse the name of the recipe
        string nameHtml = getElementsByAttr(html, "div", "class", "title-source")[0];
        string name = removeTags(getElementsByAttr(nameHtml, "h1", "itemprop", "name")[0]);

        // Parse the directions
        string directions = "";
        foreach (string dir in getElementsByAttr(html, "li", "class", "preparation-step")) {
            string direction = removeTags (dir);
            directions += direction.Substring(0, direction.Length - 1) + "\n\n";
        }

        // Parse the ingredients
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "li", "class", "ingredient")) {
            string ingredient = removeTags (ing);
            ingredients.Add (ingredient.Substring(0, ingredient.Length - 1));
        }

        // Return the recipe object
        return new recipe(name, ingredients, directions, null);
    }

    /*
     * NOT IMPLEMENTED YET
     * Gets name, ingredients, and directions from thekitchn.com
     */
    private static recipe parseTheKitchn(string html) {
        // Return null for now
        return null;

        // Get the html associated with the recipe
        string recipeHtml = getElementsByAttr(html, "div", "id", "recipe")[0];

        // Parse the name of the recipe
        string name = removeTags(getElementsByTag(html, "h2")[0]);

        // Parse the directions
        string directions = "";
    }
     
    /*
     * Gets name, ingredients, and directions from delish.com
     */
    private static recipe parseDelish(string html) {
        // Parse the name of the recipe
        string name = removeTags(getElementsByAttr(html, "h1", "class", "article-title")[0]);
        name = name.Substring (0, name.Length - 1);

        // Parse the directions
        string directions = "";
        foreach (string dir in getElementsByAttr(html, "li", "class", "recipe-directions-item")) {
            string direction = removeTags(dir);
            directions += direction.Substring (0, direction.Length - 1) + "\n\n";
        }

        // Parse the ingredients
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "li", "itemprop", "ingredients")) {
            string ingredient = removeTags (ing);
            ingredient = ingredient.Substring (0, ingredient.Length - 1);
            ingredients.Add(ingredient);
        }

        // Return the recipe object
        return new recipe(name, ingredients, directions, null);
    }

    /*
     * NOT IMPLEMENTED YET
     * Gets name, ingredients, and directions from oprah.com
     */
    private static recipe parseOprah(string html) {
        // Return null for now
        return null;

        // Parse the name of the recipe
        string name = removeTags(getElementsByAttr(html, "h1", "class", "article-text-title")[0]);

        // Parse the directions
    }

    /*
     * NOT IMPLEMENTED YET
     * Gets name, ingredients, and directions from albertsons.mywebgrocer.com
     */
    private static recipe parseAlbertsons(string html) {
        // Return null for now
        // Parse the name of the recipe
        string name = removeTags(getElementsByAttr(html, "h1", "class", "recipe-title")[0]);

        // Parse the directions
        string directionsHtml = getElementsByAttr(html, "di", "id", "RecipeDirections")[0];
        string directions = "";
        foreach (string dir in getElementsByAttr(html, "p", "class", "recipe-details-text")) {
            string direction = removeTags (dir);
            directions += direction.Substring (0, direction.Length) + "\n\n";
        }

        // Parse the ingredients
        List<string> ingredients = new List<string>();
        foreach (string ing in getElementsByAttr(html, "p", "class", "recipe-ingredient")) {
            string ingredient = removeTags (ing);
            ingredients.Add (ingredient);
        }

        // Return the recipe object
        return new recipe(name, ingredients, directions, null);
    }

    /*
     * Gets name, ingredients, and directions from www.wegmans.com
     */
    private static recipe parseWegmans(string html) {
        // Parse the name of the recipe
        string name = removeTags(getElementsByTag(html, "h1")[0]);

        // Parse the directions
        string directionsHtml = getElementsByAttr(html, "div", "id", "directions")[0];
        string directions = "";
        foreach (string dir in getElementsByTag(directionsHtml, "LI")) {
            string direction = removeTags (dir);
            directions += direction.Substring (0, direction.Length) + "\n\n";
        }

        // Parse the ingredients
        string ingredientsHtml = getElementsByAttr(html, "div", "class", "contentEntry")[0];
        List<string> ingredients = new List<string>(ingredientsHtml.Split (new string[] { "<br/>" }, StringSplitOptions.None));
        for (int i = 0; i < ingredients.Count; i++) {
            string ing = removeTags (ingredients [i]);
            ing = ing.Trim ();
            if (ing != "" && ing != "</div>") {
                ingredients [i] = ing;
            } else {
                ingredients.Remove (ingredients [i]);
                i--;
            }
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
     * Returns every instance of the tag passed in.
     */
    private static List<string> getElementsByTag(string html, string tag) {
        List<string> tags = new List<string> ();

        // Loop until none of the tags are left
        string query = "<" + tag;
        while (html.Contains(query)) {
            // Get the tag
            int startIndex = html.IndexOf(query);
            int i =  startIndex;
            while (html[i] != '>') {
                i++;
            }
            int j = ++i;
            string tmp = html.Substring(j);
            j =  tmp.IndexOf("</" + tag + ">") + (tag.Length + 3);
            while (tmp.IndexOf(query) < tmp.IndexOf("</" + tag + ">")) {
                j = tmp.IndexOf("</" + tag + ">") + (tag.Length + 3);
                tmp = tmp.Substring(tmp.IndexOf("</" + tag + ">") + (tag.Length + 3));
            }

            // Add the tag to the list
            tags.Add(html.Substring(i, j));

            // Remove the html we are done with
            html = html.Substring(i + j);
        }

        // Return the tags we found
        return tags;
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
