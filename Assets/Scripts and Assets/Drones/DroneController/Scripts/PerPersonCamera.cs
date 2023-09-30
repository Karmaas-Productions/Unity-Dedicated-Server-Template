using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PerPersonCamera : NetworkBehaviour
{
    public GameObject cameraHolder;

    public GameObject droneObject;   

    public override void OnNetworkSpawn()
    {
        cameraHolder.SetActive(IsOwner);
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            DisableDroneMovementScript();
        }
    }

    public void DisableDroneMovementScript()
    {
        if (droneObject != null)
        {
            DroneMovement script = droneObject.GetComponent<DroneMovement>();

            if (script != null)
            {
                script.enabled = false;
                Debug.Log("DroneMovement script disabled on " + droneObject.name);
            }
            else
            {
                Debug.LogWarning("DroneMovement script not found on " + droneObject.name);
            }
        }
        else
        {
            Debug.LogError("Drone GameObject is not assigned!");
        }
    }

}