using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthLogin : AuthManager
{
    void Start()
    {
     autoLogin();
    }

    public void login()
    {
        setButtonState(true);
        string username = usernameInput.text;
        string password = passwordInput.text;
        if (username == "" || password == "")
        {
            showError("Username or password must not be empty");
            return;
        }

        apiManager.login(username, password, (response) => onResponse(response));
    }

    public override void OnChildEnable()
    {
      
    }
}
