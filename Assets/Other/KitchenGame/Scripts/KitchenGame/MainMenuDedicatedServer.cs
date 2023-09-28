using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDedicatedServer : MonoBehaviour {


    private void Update() {
#if DEDICATED_SERVER
        Debug.Log("DEDICATED_SERVER");
        Loader.Load(Loader.Scene.LobbyScene);
#endif
    }

}