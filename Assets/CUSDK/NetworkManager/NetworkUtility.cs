using Newtonsoft.Json.Linq;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/** 
 * Utils for Better Network management and Single responsibility principle
 * **/
public class NetworkUtility
{
    public static bool isError(UnityWebRequest webRequest)
    {
        return webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.DataProcessingError;
    }

    public enum RequestType
    {
        GET,
        POST
    }

    public static Dictionary<string, string> fromArray(string[] keys, string[] values)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        if(keys.Length == values.Length)
        {
            for (int i=0; i < keys.Length;i++)
            {
                result.Add(keys[i], values[i]);
            }
        }

        return result;
    }

    public static Dictionary<string, string> fromListOriented(params string[] list)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

            for (int i = 0; i < list.Length && i + 1 < list.Length; i+=2)
            {
                result.Add(list[i], list[i+1]);
            }
        

        return result;
    }


    public static Dictionary<string, string> fromArrayRange(Dictionary<string, string> original, string[] keys, string[] values)
    {
   
        if (keys.Length == values.Length)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                original.Add(keys[i], values[i]);
            }
        }

        return original;
    }

    public static Dictionary<string, string> fromListOriented(Dictionary<string, string> original,  params string[] list)
    {
     

        for (int i = 0; i < list.Length && i + 1 < list.Length; i += 2)
        {
            original.Add(list[i], list[i + 1]);
        }


        return original;
    }


    public static List<T> getFrom<T>(string json, string root = "")
    {
        List<T> values = new List<T>();

        JSONNode node = JSON.Parse(json);
        JSONArray array;


        if (root != "")
            array = node[root].AsArray;
        else
            array = node.AsArray;


      //  Debug.Log("json is " + json + " text-> "+ array[0]["name"] + " count "+array.Count);
        for(int i=0; i < array.Count; i++)
        {
          //  Debug.Log(array[i]+" is? "+(string.IsNullOrEmpty(array[i])));
            T m = JsonUtility.FromJson<T>(array[i].ToString());
          
            values.Add(m);
        }

        return values;

    }

    public static WWWForm getParamsOf(RequestData requestData)
    {

        WWWForm wwwForm = new WWWForm();

        if (requestData.getRequestParams().Count > 0)
        {
            foreach (KeyValuePair<string, string> kvp in requestData.getRequestParams())
            {
                wwwForm.AddField(kvp.Key, kvp.Value);
            }
        }

        return wwwForm;

    }

    public static string getUrlFormatted(RequestData requestData)
    {
        string ur = requestData.getUrl();
        if (requestData.getRequestParams().Count > 0)
        {
            ur += "?";
            foreach (KeyValuePair<string, string> kvp in requestData.getRequestParams())
            {
                ur += kvp.Key + "=" + kvp.Value + "&";
            }
        }

        if (ur.EndsWith("&"))
        {
            ur = ur.Substring(0, ur.Length - 1);
        }

        return ur;
    }

}
