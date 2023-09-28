using LootLocker.Requests;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseUpgradeTemplate : MonoBehaviour
{
    public string upgradeKey;

    private string currency = "Krunk";
    private float currentMoney;

    public string levelNumber;

    public bool isThrusterItem;
    public bool isSensorItem;
    public bool isPropelleritem;
    public bool isChassisItem;
    public bool isWeaponitem;
    public bool isAbilityItem;

    public GameObject buyButton;
    public GameObject equiptButton;

    private string equiptedThrusterKey = "equiptedthruster";
    private string equiptedSensorKey = "equiptedsensor";
    private string equiptedPropellerKey = "equiptedpropeller";
    private string equiptedChassisKey = "equiptedchassis";
    private string equiptedWeaponKey = "equiptedweapon";
    private string equiptedAbilityKey = "equiptedability";

    public GameObject couldNotAfford;
    public GameObject doesNotOwn;

    public float price;

    private IEnumerator StartFunctionEveryTwoSeconds()
    {
        while (true)
        {
            // Call your function here
            CheckForOwnerShip();

            // Wait for 2 seconds before running the function again
            yield return new WaitForSeconds(2f);
        }
    }

    private void Awake()
    {
        StartCoroutine(StartFunctionEveryTwoSeconds());

        CreateKeys();
        CheckForOwnerShip();

        buyButton.SetActive(false);
        equiptButton.SetActive(false);

        Button buyButtonSet = buyButton.GetComponent<Button>();

        buyButtonSet.onClick.AddListener(BuyUpgrade);

        Button equiptButtonSet = equiptButton.GetComponent<Button>();

        equiptButtonSet.onClick.AddListener(EquiptUpgrade);
    }

    public void BuyUpgrade()
    {
        LootLockerSDKManager.GetSingleKeyPersistentStorage(currency, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with key" + currency + "with value: " + response.payload.value);

                    currentMoney = float.Parse(response.payload.value);

                    currentMoney -= price;

                    if (float.Parse(response.payload.value) >= price)
                    {
                        LootLockerSDKManager.UpdateOrCreateKeyValue(upgradeKey, "1", (getPersistentStoragResponse) =>
                        {
                            if (getPersistentStoragResponse.success)
                            {
                                Debug.Log("Successfully purchased weapon" + upgradeKey);

                                LootLockerSDKManager.UpdateOrCreateKeyValue(currency, currentMoney.ToString(), (getPersistentStoragResponse) =>
                                {
                                    if (getPersistentStoragResponse.success)
                                    {
                                        Debug.Log("Successfully subtracted money " + currentMoney);
                                    }
                                    else
                                    {
                                        Debug.Log("Error updating player storage from key" + currency);
                                    }
                                });
                            }
                            else
                            {
                                Debug.Log("Error updating player storage from key" + upgradeKey);
                            }
                        });
                    } else
                    {
                        Debug.Log("Could not afford " + upgradeKey);

                        couldNotAfford.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("Item with key " + currency + " does not exist");
                }
            }
            else
            {
                Debug.Log("Error getting player storage for " + upgradeKey);
            }
        });
    }

    public void EquiptUpgrade()
    {
        LootLockerSDKManager.GetSingleKeyPersistentStorage(upgradeKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);

                    if (response.payload.value == "0")
                    {
                        Debug.Log("Does not own " + upgradeKey);

                        doesNotOwn.SetActive(true);
                    } else
                    {
                        if (isThrusterItem == true)
                        {
                            LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedThrusterKey, levelNumber, (getPersistentStoragResponse) =>
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

                        if (isSensorItem == true)
                        {
                            LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedSensorKey, levelNumber, (getPersistentStoragResponse) =>
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

                        if (isPropelleritem == true)
                        {
                            LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedPropellerKey, levelNumber, (getPersistentStoragResponse) =>
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

                        if (isChassisItem == true)
                        {
                            LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedChassisKey, levelNumber, (getPersistentStoragResponse) =>
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

                        if (isWeaponitem == true)
                        {
                            LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedWeaponKey, levelNumber, (getPersistentStoragResponse) =>
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

                        if (isAbilityItem == true)
                        {
                            LootLockerSDKManager.UpdateOrCreateKeyValue(equiptedAbilityKey, levelNumber, (getPersistentStoragResponse) =>
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
                }
                else
                {
                    Debug.Log("Item with key " + upgradeKey + " does not exist");
                }
            }
            else
            {
                Debug.Log("Error getting player storage for key" + upgradeKey);
            }
        });
    }

    public void CheckForOwnerShip()
    {
        LootLockerSDKManager.GetSingleKeyPersistentStorage(upgradeKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with " + upgradeKey + " with value: " + response.payload.value);

                    if (response.payload.value == "0")
                    {
                        Debug.Log("Does not own " + upgradeKey);

                        buyButton.SetActive(true);
                    }
                    else
                    {
                        equiptButton.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("Item with key " + upgradeKey + " does not exist");

                    CreateKeys();
                }
            }
            else
            {
                Debug.Log("Error getting player storage for key" + upgradeKey);
            }
        });
    }

    public void CreateKeys()
    {
        LootLockerSDKManager.GetSingleKeyPersistentStorage(upgradeKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with " + upgradeKey + " with value: " + response.payload.value);

                    Debug.Log("help");
                }
                else
                {
                    Debug.Log("Item with key " + upgradeKey + " does not exist");

                    LootLockerSDKManager.UpdateOrCreateKeyValue(upgradeKey, "0", (getPersistentStoragResponse) =>
                    {
                        if (getPersistentStoragResponse.success)
                        {
                            Debug.Log("Successfully created key " + upgradeKey);
                        }
                        else
                        {
                            Debug.Log("Error updating player storage from key" + upgradeKey);
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