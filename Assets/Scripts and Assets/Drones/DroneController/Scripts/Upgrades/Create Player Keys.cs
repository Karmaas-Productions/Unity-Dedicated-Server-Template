using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CreatePlayerKeys : MonoBehaviour
{
    public string equiptedThrusterKey;
    public string equiptedSensorKey;
    public string equiptedPropellerKey;
    public string equiptedChassisKey;
    public string equiptedWeaponKey;
    public string equiptedAbilityKey;

    void Awake()
    {
        LootLockerSDKManager.GetSingleKeyPersistentStorage(equiptedThrusterKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);
                }
                else
                {
                    LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedThrusterKey, "0", (getPersistentStoragResponse) =>
                    {
                        if (getPersistentStoragResponse.success)
                        {
                            Debug.Log("Successfully updated player storage");
                        }
                        else
                        {
                            Debug.Log("Error updating player storage");
                        }
                    });
                }
            }
            else
            {
                Debug.Log("Error getting player storage");
            }
        });

        LootLockerSDKManager.GetSingleKeyPersistentStorage(equiptedSensorKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);
                }
                else
                {
                    LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedSensorKey, "0", (getPersistentStoragResponse) =>
                    {
                        if (getPersistentStoragResponse.success)
                        {
                            Debug.Log("Successfully updated player storage");
                        }
                        else
                        {
                            Debug.Log("Error updating player storage");
                        }
                    });
                }
            }
            else
            {
                Debug.Log("Error getting player storage");
            }
        });

        LootLockerSDKManager.GetSingleKeyPersistentStorage(equiptedPropellerKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);
                }
                else
                {
                    LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedPropellerKey, "0", (getPersistentStoragResponse) =>
                    {
                        if (getPersistentStoragResponse.success)
                        {
                            Debug.Log("Successfully updated player storage");
                        }
                        else
                        {
                            Debug.Log("Error updating player storage");
                        }
                    });
                }
            }
            else
            {
                Debug.Log("Error getting player storage");
            }
        });

        LootLockerSDKManager.GetSingleKeyPersistentStorage(equiptedChassisKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);
                }
                else
                {
                    LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedChassisKey, "0", (getPersistentStoragResponse) =>
                    {
                        if (getPersistentStoragResponse.success)
                        {
                            Debug.Log("Successfully updated player storage");
                        }
                        else
                        {
                            Debug.Log("Error updating player storage");
                        }
                    });
                }
            }
            else
            {
                Debug.Log("Error getting player storage");
            }
        });

        LootLockerSDKManager.GetSingleKeyPersistentStorage(equiptedWeaponKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);
                }
                else
                {
                    LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedWeaponKey, "0", (getPersistentStoragResponse) =>
                    {
                        if (getPersistentStoragResponse.success)
                        {
                            Debug.Log("Successfully updated player storage");
                        }
                        else
                        {
                            Debug.Log("Error updating player storage");
                        }
                    });
                }
            }
            else
            {
                Debug.Log("Error getting player storage");
            }
        });

        LootLockerSDKManager.GetSingleKeyPersistentStorage(equiptedAbilityKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);
                }
                else
                {
                    LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedAbilityKey, "0", (getPersistentStoragResponse) =>
                    {
                        if (getPersistentStoragResponse.success)
                        {
                            Debug.Log("Successfully updated player storage");
                        }
                        else
                        {
                            Debug.Log("Error updating player storage");
                        }
                    });
                }
            }
            else
            {
                Debug.Log("Error getting player storage");
            }
        });
    }
}
