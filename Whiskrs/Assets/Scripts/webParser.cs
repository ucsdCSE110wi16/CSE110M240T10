using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

public class webParser : MonoBehaviour{
    public static webParser Instance;

    public void Awake(){
        Instance = this;
    }

    public static recipe parse(string url, string html) {
        string baseURL = getURLBase(url);
        switch (baseURL) { 
            case "www.myrecipes.com":
                return parseMyRecipes(html);
        }
        return null;
    }

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

    private static string getBody(string html)
    {
        int index = html.IndexOf("<body");
        int endOfBody = html.IndexOf("</body>") + 7;
        html = html.Substring(index, endOfBody - index);
        return html;
    }

    private static string removeNewline(string html) {
        return Regex.Replace(html, @"\t|\n|\r", "");
    }

    private static List<string> getElementsByAttr(string html, string divType, string attr, string val) {
        List<string> divs = new List<string>();
        string query = attr + "=\"" + val + "\"";
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

    private static string removeScript(string html) {
        return removeBlock(html, "<script", "</script>");
    }

    private static string removeTags(string html) {
        return Regex.Replace(Regex.Replace(html, @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ");
    }

    private static string removeBlock(string html, string removeOpen, string removeClose)
    {
        int i = 0;
        while (html.Contains(removeOpen) && i < 20)
        {
            int index = html.IndexOf(removeOpen);
            string tmp = html.Substring(0, index);
            int endOfScript = html.IndexOf(removeClose) + removeClose.Length;
            html = tmp + html.Substring(endOfScript);
            i++;
        }
        return html;        
    }

    private static string getURLBase(string url)
    {
        string[] urlSplit = url.Split(new string[] { "://" }, StringSplitOptions.None);
        return urlSplit[1].Split('/')[0];
    }
}