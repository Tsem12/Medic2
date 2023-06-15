using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

[CreateAssetMenu(fileName = "Achievement", menuName = "ScriptableObjects/Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField] LevelDataObject levelData;
    public string Token;
    public string Error;

    public void Init()
    {
        PlayGamesPlatform.Activate();
        LoginGooglePlayGames();
    }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                });
            }
            else
            {
                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
            }
        });
    }


    public void Check()
    {
        
    }

    public void FinishLevel()
    {
        switch (levelData.currentSceneIndex)
        {
            case 0:
                Social.ReportProgress("CgklzPjh_7UCEAIQAA", 100.0f, null);
                break;
            case 1:
                Social.ReportProgress("CgklzPjh_7UCEAIQAQ", 100.0f, null);
                break;
            case 2:
                Social.ReportProgress("CgklzPjh_7UCEAIQAg", 100.0f, null);
                break;
            case 3:
                Social.ReportProgress("CgklzPjh_7UCEAIQAw", 100.0f, null);
                break;
        }
    }
}
