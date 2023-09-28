using DroneController.Physics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using LootLocker.Requests;
using System;

public class DroneMovement : DroneMovementScript
{
    private bool wKeyPressed = false;
    private bool aKeyPressed = false;
    private bool sKeyPressed = false;
    private bool dKeyPressed = false;
    private bool iKeyPressed = false;
    private bool jKeyPressed = false;
    private bool kKeyPressed = false;
    private bool lKeyPressed = false;

    private bool isCursorVisible = true;

    public GameObject targetObject;

    public override void Update()
    {
        base.Update(); //I would suggest you to put code below this line

        SceneChangeOnClick();
        FlipDroneOnClick();

        CheckSceneName();

        HandleKeyPress(KeyCode.W, ref wKeyPressed, OnWKeyPress, OnWKeyRelease);
        HandleKeyPress(KeyCode.A, ref aKeyPressed, OnAKeyPress, OnAKeyRelease);
        HandleKeyPress(KeyCode.S, ref sKeyPressed, OnSKeyPress, OnSKeyRelease);
        HandleKeyPress(KeyCode.D, ref dKeyPressed, OnDKeyPress, OnDKeyRelease);
        HandleKeyPress(KeyCode.I, ref iKeyPressed, OnIKeyPress, OnIKeyRelease);
        HandleKeyPress(KeyCode.J, ref jKeyPressed, OnJKeyPress, OnJKeyRelease);
        HandleKeyPress(KeyCode.K, ref kKeyPressed, OnKKeyPress, OnKKeyRelease);
        HandleKeyPress(KeyCode.L, ref lKeyPressed, OnLKeyPress, OnLKeyRelease);

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            // Mouse moved, so show the cursor.
            SetCursorVisibility(true);
        }
        else
        {
            // Mouse not moved, so hide the cursor.
            SetCursorVisibility(false);
        }
    }

    public void Start()
    {
        GetKeyValues();

        Cursor.visible = isCursorVisible;
    }

    //#################################################################################################################################################################################
    //My scripts...
    //#################################################################################################################################################################################

    public GameObject camera;

    private void HandleKeyPress(KeyCode key, ref bool keyState, Action onPress, Action onRelease)
    {
        if (Input.GetKey(key))
        {
            if (!keyState)
            {
                onPress?.Invoke();
                keyState = true;
            }
        }
        else
        {
            if (keyState)
            {
                onRelease?.Invoke();
                keyState = false;
            }
        }
    }

    void OnWKeyPress()
    {
        CustomFeed_pitch = 0.65f;

        Cursor.visible = false;
    }

    void OnAKeyPress()
    {
        CustomFeed_yaw = 0.65f;

        Cursor.visible = false;
    }

    void OnSKeyPress()
    {
        CustomFeed_pitch = -0.65f;

        Cursor.visible = false;
    }

    void OnDKeyPress()
    {
        CustomFeed_yaw = -0.65f;

        Cursor.visible = false;
    }

    void OnIKeyPress()
    {
        CustomFeed_throttle = 0.65f;

        Cursor.visible = false;
    }

    void OnJKeyPress()
    {
        CustomFeed_roll = -0.65f;

        Cursor.visible = false;
    }

    void OnKKeyPress()
    {

    }

    void OnLKeyPress()
    {
        CustomFeed_roll = 0.65f;

        Cursor.visible = false;
    }

    void OnWKeyRelease()
    {
        ResetPitch();

        Cursor.visible = false;
    }

    void OnAKeyRelease()
    {
        ResetYaw();

        Cursor.visible = false;
    }

    void OnSKeyRelease()
    {
        ResetPitch();

        Cursor.visible = false;
    }

    void OnDKeyRelease()
    {
        ResetYaw();

        Cursor.visible = false;
    }

    void OnIKeyRelease()
    {
        ResetThrottle();

        Cursor.visible = false;
    }

    void OnJKeyRelease()
    {
        ResetRoll();

        Cursor.visible = false;
    }

    void OnKKeyRelease()
    {

    }

    void OnLKeyRelease()
    {
        ResetRoll();

        Cursor.visible = false;
    }

    private void SetCursorVisibility(bool visible)
    {
        if (visible != isCursorVisible)
        {
            isCursorVisible = visible;
            Cursor.visible = isCursorVisible;
        }
    }

    private string equiptedThrusterKey = "equiptedthruster";
    private string equiptedSensorKey = "equiptedsensor";
    private string equiptedPropellerKey = "equiptedpropeller";
    private string equiptedChassisKey = "equiptedchassis";
    private string equiptedWeaponKey = "equiptedweapon";
    private string equiptedAbilityKey = "equiptedability";

    public float equiptedthruster;
    public float equiptedsensor;
    public float equiptedpropeller;
    private float equiptedchassis;
    private float equiptedweapon;
    private float equiptedability;
    
    private void SceneChangeOnClick()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FlipDroneOnClick()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1)) FlipDrone();
    }

    private void CheckSceneName()
    {
        // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Compare the scene name to "Lobby"
        if (currentSceneName == "LobbyScene")
        {
            // Run your function here (replace with your desired function)
            DisableCameraInLobby();

            DisablePlayerMovement();
        }
    }

    private void DisableCameraInLobby()
    {
        camera.SetActive(false);
    }

    public void DisablePlayerMovement()
    {
        if (targetObject != null)
        {
            // Try to find the DroneMovement script on the target GameObject.
            DroneMovement droneMovementScript = targetObject.GetComponent<DroneMovement>();

            // If the script is found, disable it.
            if (droneMovementScript != null)
            {
                droneMovementScript.enabled = false;
                Debug.Log("DroneMovement script disabled on " + targetObject.name);
            }
            else
            {
                Debug.LogWarning("DroneMovement script not found on " + targetObject.name);
            }
        }
        else
        {
            Debug.LogError("Target Object not assigned. Please assign the GameObject with the DroneMovement script.");
        }
    }

    private void GetKeyValues()
    {
        LootLockerSDKManager.GetSingleKeyPersistentStorage(equiptedThrusterKey, (response) =>
        {
            if (response.success)
            {
                if (response.payload != null)
                {
                    Debug.Log("Successfully retrieved player storage with value: " + response.payload.value);

                    equiptedthruster = float.Parse(response.payload.value);

                    SetThrusterMoevement();
                }
                else
                {
                    Debug.Log("Item with key " + equiptedThrusterKey + " does not exist");
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

                    equiptedsensor = float.Parse(response.payload.value);

                    SetSensorMovement();
                }
                else
                {
                    Debug.Log("Item with key " + equiptedSensorKey + " does not exist");
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

                    equiptedpropeller = float.Parse(response.payload.value);

                    SetPropellerMovement();
                }
                else
                {
                    Debug.Log("Item with key " + equiptedPropellerKey + " does not exist");
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

                    equiptedchassis = float.Parse(response.payload.value);

                    SetChassisMovement();
                }
                else
                {
                    Debug.Log("Item with key " + equiptedChassisKey + " does not exist");
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

                    equiptedweapon = float.Parse(response.payload.value);

                    SetWeaponMovement();
                }
                else
                {
                    Debug.Log("Item with key " + equiptedWeaponKey + " does not exist");
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

                    equiptedability = float.Parse(response.payload.value);

                    SetAbilityMovement();
                }
                else
                {
                    Debug.Log("Item with key " + equiptedAbilityKey + " does not exist");
                }
            }
            else
            {
                Debug.Log("Error getting player storage");
            }
        });
    }

    public void SetThrusterMoevement()
    {
        // Set Thruster Movement

        if (equiptedthruster == 0)
        {
            throttlespeed = 0.6f;
        }

        if (equiptedthruster == 1)
        {
            throttlespeed = 0.65f;
        }

        if (equiptedthruster == 2)
        {
            throttlespeed = 0.7f;
        }

        if (equiptedthruster == 3)
        {
            throttlespeed = 0.75f;
        }

        if (equiptedthruster == 4)
        {
            throttlespeed = 0.8f;
        }

        if (equiptedthruster == 5)
        {
            throttlespeed = 0.9f;
        }

        if (equiptedthruster == 6)
        {
            throttlespeed = 1f;
        }

        if (equiptedthruster == 7)
        {
            throttlespeed = 1.075f;
        }
    }

    public void SetSensorMovement()
    {
        // Set Sensor Movement

        if (equiptedsensor == 0)
        {
            pitchyawrollspeed = 0.6f;
        }

        if (equiptedsensor == 1)
        {
            pitchyawrollspeed = 0.65f;
        }

        if (equiptedsensor == 2)
        {
            pitchyawrollspeed = 0.7f;
        }

        if (equiptedsensor == 3)
        {
            pitchyawrollspeed = 0.75f;
        }

        if (equiptedsensor == 4)
        {
            pitchyawrollspeed = 0.8f;
        }

        if (equiptedsensor == 5)
        {
            pitchyawrollspeed = 0.8f;
        }
    }

    public void SetPropellerMovement()
    {
        // Set Propeller Movement

        if (equiptedpropeller == 0)
        {
            angleLimit = 40f;
        }

        if (equiptedpropeller == 1)
        {
            angleLimit = 35f;
        }

        if (equiptedpropeller == 2)
        {
            angleLimit = 27.5f;
        }

        if (equiptedpropeller == 3)
        {
            angleLimit = 205f;
        }

        if (equiptedpropeller == 4)
        {
            angleLimit = 15f;
        }
    }

    public void SetChassisMovement()
    {
        // Set Chassis Movement

        if (equiptedchassis == 0)
        {
            maxDrag = 4f;
        }

        if (equiptedchassis == 1)
        {
            maxDrag = 3.5f;
        }

        if (equiptedchassis == 2)
        {
            maxDrag = 3f;
        }

        if (equiptedchassis == 3)
        {
            maxDrag = 2.75f;
        }

        if (equiptedchassis == 4)
        {
            maxDrag = 2.25f;
        }

        if (equiptedchassis == 5)
        {
            maxDrag = 1.8f;
        }
    }

    public void SetWeaponMovement()
    {
        // Set Weapon Movement

        if (equiptedweapon == 0)
        {
            // Set Weapon
        }
    }

    public void SetAbilityMovement()
    {
        // Set Ability Movement

        if (equiptedability == 0)
        {
            // Set Ability
        }
    }
}
