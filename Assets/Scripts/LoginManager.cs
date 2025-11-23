using UnityEngine;
using PlayFab;
using PlayFab.ClientModels; 
using System;

public class LoginManager
{
    private ILogin loginMethod;

    public void SetLoginMethod(ILogin loginMethod)
    {
        this.loginMethod = loginMethod;
    }

    public void Login(Action<LoginResult> OnSucess, Action<PlayFabError> OnFail)
    {
        if (loginMethod != null)
        {
            loginMethod.Login(OnSucess, OnFail);
        }
        else
        {
            {
                Debug.LogError("No login method found in LoginManager.");
            }
        }
    }
}
