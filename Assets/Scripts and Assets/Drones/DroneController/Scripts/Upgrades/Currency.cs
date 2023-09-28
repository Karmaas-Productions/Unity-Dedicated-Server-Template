using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    public string currencyKey;

    public TMP_Text currencyText;

    private void Awake()
    {
        StartCoroutine(StartFunctionEveryTwoSeconds());
    }

    private IEnumerator StartFunctionEveryTwoSeconds()
    {
        while (true)
        {
            // Call your function here
            UpdateCurrency();

            // Wait for 2 seconds before running the function again
            yield return new WaitForSeconds(2f);
        }
    }

    public void UpdateCurrency()
    {
        LootLockerSDKManager.GetSingleKeyPersistentStorage(currencyKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);

                    currencyText.text = response.payload.value + " Krunk";
                }
                else
                {
                    LootLockerSDKManager.UpdateOrCreateKeyValue(currencyKey, "0", (getPersistentStoragResponse) =>
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
