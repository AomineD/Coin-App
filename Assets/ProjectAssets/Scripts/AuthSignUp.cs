using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthSignUp : AuthManager
{
    [Header("EXTRA")]
    public TMP_InputField passwordConfirmInput;

    public override void OnChildEnable()
    {
        passwordConfirmInput.text = "";
    }

    public void signUp()
    {
        setButtonState(true);
        string username = usernameInput.text;
        string password = passwordInput.text;
        string confirmPassword = passwordConfirmInput.text;
        if (username == "" || password == "")
        {
            showError("Username or password must not be empty");
            return;
        }

        if(password != confirmPassword)
        {
            showError("Passwords don't match");
            return;
        }

        apiManager.signup(username, password, (response) => onResponse(response));
    }
}
