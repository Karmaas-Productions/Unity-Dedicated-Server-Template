using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

public class MovementManager : NetworkBehaviour
{
    public GameObject UI;
    public GameObject droneGameObject;

    void Start()
    {
        if (!IsOwner)
        {
            UI.SetActive(false);

            if (droneGameObject != null)
            {
                DroneMovement droneMovementScript = droneGameObject.GetComponent<DroneMovement>();
                if (droneMovementScript != null)
                {
                    droneMovementScript.enabled = false;
                }
            }
        }

        if (droneGameObject != null)
        {
            DroneMovement droneMovement = droneGameObject.GetComponent<DroneMovement>();

            if (droneMovement != null)
            {
                // Enable the DroneMovement script on the target GameObject
                droneMovement.enabled = true;
            }
            else
            {
                Debug.LogError("DroneMovement script not found on the target GameObject.");
            }
        }
        else
        {
            Debug.LogError("Target GameObject is not assigned.");
        }
    }
}
