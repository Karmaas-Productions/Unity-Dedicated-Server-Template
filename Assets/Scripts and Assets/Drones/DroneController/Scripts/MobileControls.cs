using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MobileControls : MonoBehaviour
{
    public GameObject androidIOSContainer; // Container for buttons on Android and iOS

    private void Awake()
    {

        // Show/hide buttons based on platform
#if UNITY_ANDROID || UNITY_IOS
        androidIOSContainer.SetActive(true);
#else
        androidIOSContainer.SetActive(false);
#endif
    }
}