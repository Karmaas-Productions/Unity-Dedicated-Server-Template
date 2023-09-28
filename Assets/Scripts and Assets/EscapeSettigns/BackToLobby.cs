using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackToLobby : MonoBehaviour
{
    public void LobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
