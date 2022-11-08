using OneSignalSDK;
using System;
using UnityEngine;
using UnityEngine.Events;

// I am using my own Network manager created in C# to manage HTTP request
public class ApiManager : NetworkManager
{
    // Using PHP CdeIgniter backend made by me
    private const string baseUrl = "https://api-coin.uheist.com/api/";
    
    void Start()
    {
        // Initialize Onesignal
        OneSignal.Default.Initialize("965d6723-86eb-4619-bc31-06e36033d27a");
    }

    public void login(string user, string pass, UnityAction<Response<LoginResponse>> listener)
    {
        RequestData data = RequestData.POSTSimple(baseUrl+"login");
        data.setRequestParams("user", user, "password", pass);
        setRequestData(data);
        load(listener);
    }

    public void signup(string user, string pass, UnityAction<Response<LoginResponse>> listener)
    {
        RequestData data = RequestData.POSTSimple(baseUrl + "signup");
        data.setRequestParams("user", user, "password", pass);
        setRequestData(data);
        load(listener);
    }

    public void getCoins(User user, UnityAction<Response<CoinResponse>> listener)
    {
        RequestData data = RequestData.POSTSimple(baseUrl + "getCoins");
        data.setRequestParams("idUser", user.id.ToString());
        setRequestData(data);
        load(listener);
    }

    [Serializable]
    public class User
    {
        public long id;
        public string username;
        public long coins;
        public string last_claim;

        public DateTime getTimeForClaim()
        {
            DateTime parsedDateTime = DateTime.Parse(last_claim);
            parsedDateTime = parsedDateTime.AddMinutes(3); // Add 3 minutes delay to claim again
            return parsedDateTime;
        }
       
    }

    [Serializable]
    public class CoinInfo
    {
        public long coins;
        public string last_claim;

        public DateTime getTimeForClaim()
        {
            DateTime parsedDateTime = DateTime.Parse(last_claim);
            parsedDateTime = parsedDateTime.AddMinutes(3); // Add 3 minutes delay to claim again
            return parsedDateTime;
        }

    }

    [Serializable]
    public class LoginResponse : Response
    {
        public User data;
    }

    [Serializable]
    public class CoinResponse : Response
    {
        public CoinInfo data;
    }


    public class Response
    {
        public string status;
        public string message;
    }

}
