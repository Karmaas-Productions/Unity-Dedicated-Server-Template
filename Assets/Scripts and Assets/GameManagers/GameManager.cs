using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.Multiplay;

public class PlayerSpawnManager : NetworkBehaviour
{
    public GameObject playerPrefab;

    public Transform initialSpawnPoint;

    private async void Start()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
#if DEDICATED_SERVER
            await MultiplayService.Instance.UnreadyServerAsync();
#endif
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            GameObject player = Instantiate(playerPrefab, initialSpawnPoint.position, initialSpawnPoint.rotation);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

            Debug.Log("Spawned Player");
        }
    }
} 