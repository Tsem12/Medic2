using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openurl : MonoBehaviour
{
    public void OpenUrl(string UrlName)
    {
        Application.OpenURL(UrlName);
    }
}
