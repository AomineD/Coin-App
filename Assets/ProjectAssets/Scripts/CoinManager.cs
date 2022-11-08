using OneSignalSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static ApiManager;

public class CoinManager : MonoBehaviour
{
    [SerializeField]
    private ApiManager apiManager;
    private User userLogged;
    [Header("UI")]
    public TMP_Text currentCoinsText;
    public TMP_Text nameText;
    public Button buttonGetCoin;
    public TMP_Text errorHandlingText;
    public GameObject loadingBtnAnim;
    bool isReady = false;
    [Header("Anim")]
    public Animator animatorCoin;
    [Header("Events")]
    public UnityEvent logOutEvent;

    private void OnEnable()
    {
        userLogged = AuthManager.userLogged;
        nameText.text = "Hello " + userLogged.username + ", welcome back!";
        currentCoinsText.text = "Current coins: " + userLogged.coins;
        isReady = userLogged.isReadyForGetCoins();
        if (isReady)
        {
            isReadyForGetCoins();
        }
    }

    private void Update()
    {
        if (!isReady)
        {
            // check if seconds is 0 to enable button
            float seconds = userLogged.getTimeForClaim().toSecondsFloat();
            if(seconds <= 0)
            {
                isReadyForGetCoins();
                return;
            }
            buttonGetCoin.interactable = false;
            // Set time left in a readable format
            buttonGetCoin.getButtonText().text = seconds.timeLeftText();
        }
    }

    /*
     * Is ready for use GET COINS button!
     */
    private void isReadyForGetCoins()
    {
        buttonGetCoin.getButtonText().text = "GET COINS";
        setButtonState(false);
        isReady = true;
    }

    public void getCoins()
    {
        setButtonState(true);
        apiManager.getCoins(userLogged, (response) => onResponse(response));
    }


    private void onResponse(Response<CoinResponse> response)
    {

        if (response.responseStatus() != ResponseStatus.OK)
        {
            showError(response.GetResponseMessage());
            return;
        }

        if (response.GetValue().status != "success")
        {
            showError(response.GetValue().message);
            return;
        }

        CoinInfo info = response.GetValue().data;

        // Set new data and start cronometer
        userLogged.last_claim = info.last_claim;
        userLogged.coins = info.coins;

        currentCoinsText.text = "Current coins: " + userLogged.coins;
        isReady = false;
        setButtonState(false);
        animatorCoin.Play("new_coin");
    }


    /*
     * Set button state to loading or idle
     */
    void setButtonState(bool loading)
    {
       StartCoroutine(setStateWithDelay(loading));
    }

    /*
    * Show text error and hide within 3 seconds
    */
    void showError(string txt)
    {
        errorHandlingText.text = txt;
        errorHandlingText.gameObject.SetActive(true);
        Invoke("hideError", 3f);
        setButtonState(false);
    }

    void hideError()
    {
        errorHandlingText.gameObject.SetActive(false);
    }

    /*
     * LOG OUT FROM COINGET UI
     */
    public void logout()
    {
        PlayerPrefs.DeleteAll();
        logOutEvent.Invoke();
    }

    /*
     * clear External Id onesignal
     */
    protected async void setId()
    {
        var result = await OneSignal.Default.RemoveExternalUserId();
        if (result)
        {
            Debug.Log("removed external id");
        }
    }


    IEnumerator setStateWithDelay(bool stateLoading)
    {
        yield return new WaitForSeconds(0.5f);
        setButtonStateDelayed(stateLoading);
    }

    private void setButtonStateDelayed(bool loading)
    {
        buttonGetCoin.getButtonText().enabled = !loading;
        loadingBtnAnim.SetActive(loading);
        buttonGetCoin.interactable = !loading;
    }
}
