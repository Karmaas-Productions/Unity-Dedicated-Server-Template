using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PerPersonCamera : NetworkBehaviour
{
    public GameObject cameraHolder;

    public override void OnNetworkSpawn()
    {
        cameraHolder.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }

}
