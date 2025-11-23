using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayfabLoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    
    private LoginManager loginManager;

    private string savedUsernameKey = "SavedUsername";

    private string username;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loginManager = new LoginManager();
    }

    public void LoginButtonClicked()
    {
        this.username = username;
        loginManager.SetLoginMethod(new GuestLogin(inputField.text));
        loginManager.Login(OnLoginSuccess, OnLoginFail);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login success!");
    }

    private void OnLoginFail(PlayFabError error)
    {
        Debug.Log("Login failed: " + error.ErrorMessage);
    }
}
