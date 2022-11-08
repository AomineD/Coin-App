using OneSignalSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static ApiManager;

public abstract class AuthManager : MonoBehaviour
{
    /* KEYs for PlayerPrefs sessions */
    private const string key_session = "is_logged";
    private const string key_user = "usn_";
    private const string key_pass = "pssw_";
    // Single logged user instance
    public static User userLogged;
    internal ApiManager apiManager;
    bool isLoggedIn;

    [Header("UI")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button buttonLogin;
    public GameObject loadingBtnAnim;
    public TMP_Text errorHandling;
    [Header("Event")]
    public UnityEvent onLogged;

    private void Awake()
    {
        apiManager = FindObjectOfType<ApiManager>();
    }

    private void OnEnable()
    {
      passwordInput.text = "";
      usernameInput.text = "";
      OnChildEnable();
        setButtonState(false);
    }

    IEnumerator setStateWithDelay(bool stateLoading, bool isLogged = false)
    {
        setButtonStateDelayed(stateLoading);
        yield return new WaitForSeconds(0.5f);
        if (isLogged)
        {
            onLogged.Invoke();
        }
    }

    public abstract void OnChildEnable();

    protected void onResponse(Response<LoginResponse> response)
    {
        
        if(response.responseStatus() != ResponseStatus.OK)
        {
            showError(response.GetResponseMessage());
            if (isLoggedIn)
            {
                PlayerPrefs.DeleteAll();
            }
            return;
        }

        if(response.GetValue().status != "success")
        {
            showError(response.GetValue().message);
            if (isLoggedIn)
            {
                PlayerPrefs.DeleteAll();
            }
            return;
        }

        userLogged = response.GetValue().data;


        // set session
        if (!isLoggedIn)
        {
            PlayerPrefs.SetInt(key_session, 1);
            PlayerPrefs.SetString(key_user, userLogged.username);
            PlayerPrefs.SetString(key_pass, passwordInput.text);
            setId();
        }
        setButtonState(false, true);
    }

    /*
     * Set onesignal external id for automated notifications
     */
    protected async void setId()
    {
        var result = await OneSignal.Default.SetExternalUserId(userLogged.id + "234567");
        if (result)
        {
            Debug.Log("success");
        }
    }

    /*
     * 
     */
   

    /*
     * Set button state to loading or idle
     */
    protected void setButtonState(bool loading, bool isLogged = false)
    {
        StartCoroutine(setStateWithDelay(loading, isLogged));
    }

    private void setButtonStateDelayed(bool loading)
    {
        buttonLogin.getButtonText().enabled = !loading;
        loadingBtnAnim.SetActive(loading);
        buttonLogin.interactable = !loading;
        usernameInput.interactable = !loading;
        passwordInput.interactable = !loading;
    }

    /*
     * Show text error and hide within 3 seconds
     */
    protected void showError(string txt)
    {
        errorHandling.text = txt;
        errorHandling.gameObject.SetActive(true);
        Invoke("hideError", 3f);
        setButtonState(false);
    }

    protected void hideError()
    {
        errorHandling.gameObject.SetActive(false);
    }

    /*
     * Auto login if any session exist
     */
    public void autoLogin()
    {
        isLoggedIn = PlayerPrefs.GetInt(key_session, 0) == 1;
        if (isLoggedIn)
        {
            string username = PlayerPrefs.GetString(key_user, "");
            string password = PlayerPrefs.GetString(key_pass, "");
            setButtonState(true);
            apiManager.login(username, password, (response) => onResponse(response));
        }
    }
}
