using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

public class Achievement : ScriptableObject
{
    public void Login()
    {

    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }


    public void Init() // Use at every level load
    {

    }

    public void Check()
    {
        
    }

    public void FinishFirstLevel()
    {
        Social.ReportProgress("Cfjewijawiu_QA", 100.0f, (bool _) => {  });

    }
}
