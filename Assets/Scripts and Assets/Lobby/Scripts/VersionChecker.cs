#if !UNITY_WEBGL

using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using TMPro;

public class VersionChecker : MonoBehaviour
{
    public string githubVersionURL = "https://github.com/LukeGus/Propulse/blob/main/version.txt";
    public string localVersion;
    private string onlineVersion;

    public GameObject updatePrompt;
    public TMP_Text updateText;
    public GameObject authenticationObject;

    void Start()
    {
        // Start the version checking coroutine.
        StartCoroutine(CheckVersion());
    }

    IEnumerator CheckVersion()
    {
        // Download the online version from GitHub.
        using (WebClient client = new WebClient())
        {
            yield return null;
            onlineVersion = client.DownloadString(githubVersionURL).Trim();
        }

        // Compare local and online versions.
        if (localVersion != onlineVersion)
        { 
            // The versions do not match.
            // Enable the updatePrompt GameObject and set the updateText.
            updatePrompt.SetActive(true);
            updateText.text = "Your Game is Outdated\n\nPlease update your game to " + onlineVersion;

            authenticationObject.SetActive(false);
        }
        else
        {
            // The versions match.
            // Disable the updatePrompt GameObject (assuming it's initially enabled).
            updatePrompt.SetActive(false);
        }
    }
}

#endif