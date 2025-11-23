using UnityEngine;
using PlayFab.ClientModels;
using System;
using PlayFab;

public interface ILogin
{
    public void Login(Action<LoginResult> OnSucess, Action<PlayFabError> OnFail);
    
    
}
