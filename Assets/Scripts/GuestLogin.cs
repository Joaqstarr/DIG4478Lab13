using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class GuestLogin : ILogin
{
    private string deviceId;

    public GuestLogin(string deviceId)
    {
        this.deviceId = deviceId;
    }

    public void Login(Action<LoginResult> OnSucess, Action<PlayFabError> OnFail)
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = this.deviceId,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucess, OnFail);
    }
}
