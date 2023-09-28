using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerRPC : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void MyGlobalServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            // Do things for this client
        }
    }

    public override void OnNetworkSpawn()
    {
        MyGlobalServerRpc(); // serverRpcParams will be filled in automatically
    }
}
