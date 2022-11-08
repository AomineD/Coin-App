using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using static NetworkUtility;

public class RequestData
{
    private string url = "";
    private RequestType type;
    private string rootArrayName = "";
    internal string rootName {
        get { return rootArrayName; }
    }

   

    private Dictionary<string, string> requestParams = new Dictionary<string, string>();
    private UnityAction<float> progressListener = null;
    
    // EASY AND SIMPLE CONSTRUCTORS

    public static RequestData GETSimple(string url)
    {
        return new RequestData(url, RequestType.GET);
    }

    public static RequestData POSTSimple(string url)
    {
        return new RequestData(url, RequestType.POST);
    }

    public static RequestData POSTNoURL()
    {
        return new RequestData("", RequestType.POST);
    }

    public static RequestData GETNoURL()
    {
        return new RequestData("", RequestType.GET);
    }

    public void setUrl(string url)
    {
        this.url = url;
    }

    // BASIC CONSTRUCTOR
    public RequestData(string url, RequestType type)
    {
        this.url = url;
        this.type = type;
    
    }

    /// <summary>
    /// This method set requestParams in POST or GET request, you can use inline dictionary
    /// </summary>
    public void setRequestParams(Dictionary<string, string> requestParams)
    {
        this.requestParams = requestParams;
    }

    /// <summary>
    /// This method set requestParams in POST or GET request, you can use inline key-value pars
    /// </summary>
    public void setRequestParams(params string[] list)
    {
        this.requestParams = fromListOriented(list);
    }


    /// <summary>
    /// This method set name of root array for List request
    /// </summary>
    public void setRootName(string root)
    {
        rootArrayName = root;
    }

    /// <summary>
    /// This method add requestParams in POST or GET request, you can use inline key-value pars
    /// </summary>
    public void addRequestParams(params string[] list)
    {
        requestParams = fromListOriented(requestParams, list);
    }


    public void setProgressListener(UnityAction<float> progressListener)
    {
        this.progressListener = progressListener;
    }

    public UnityAction<float> getListener()
    {
        return progressListener;
    }


    public Dictionary<string, string> getRequestParams()
    {
        return requestParams;
    }


    public RequestType getType()
    {
        return type;
    }

    public string getUrl()
    {
        if(url == null || url == "")
        {
            throw new ArgumentNullException("Must set URL before load anything");
        }

        return url;
    }

}
