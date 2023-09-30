using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LootLockerAuthentication : MonoBehaviour
{   
    public TMP_InputField logInEmailInputField;
    public TMP_InputField logInPasswordInputField;

    public TMP_InputField signUpEmailInputField;
    public TMP_InputField signUpPasswordInputField;

    public TMP_InputField playerNameInputField;

    public TMP_InputField resetEmailInputField;

    public GameObject Authentication;
    public GameObject LogInPage;
    public GameObject SignUpPage;
    public GameObject ResetPasswordPage;
    public GameObject VerifyPage;
    public GameObject OperationFailed;
    public GameObject LogInButton;
    public GameObject SignUpButton;
    public GameObject Wait;
    public GameObject setPlayerName;

    public GameObject cameraTransitionObject;

    public GameObject signUpPage;
    public GameObject logInPage;

    public GameObject signUpButton;
    public GameObject logInButton;

    public GameObject mainMenu;

    bool rememberMe = true;

    private string logInEmail = "Enter your email...";
    private string logInPassword = "Enter your password...";

    private string signUpEmail = "Enter your email...";
    private string signUpPassword = "Enter your password...";

    private string playerName = "Enter your desired player name...";

    private string resetEmail = "Enter your email...";

    private bool canStart = true;

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Start()
    {

#if DEDICATED_SERVER
        Authentication.SetActive(false);

        canStart = false;
#endif

        if (canStart == true)
        {
            StartFunctions();
        }
    }

    public void StartFunctions()
    {
        Debug.developerConsoleVisible = true;

        // Load saved email and password from PlayerPrefs
        if (PlayerPrefs.HasKey("SavedEmail") && PlayerPrefs.HasKey("SavedPassword"))
        {
            logInEmail = PlayerPrefs.GetString("SavedEmail");
            logInPassword = PlayerPrefs.GetString("SavedPassword");
        }

        // Set the input fields' text based on the saved data
        if (logInEmailInputField != null)
        {
            logInEmailInputField.text = logInEmail;
        }

        if (logInPasswordInputField != null)
        {
            logInPasswordInputField.text = logInPassword;
        }

        LogIn();
    }

    private void UpdateEmailFromInputField()
    {
        if (logInEmailInputField != null)
        {
            logInEmail = logInEmailInputField.text;
        }

        if (signUpEmailInputField != null)
        {
            signUpEmail = signUpEmailInputField.text;
        }

        if (resetEmailInputField != null)
        {
            resetEmail = resetEmailInputField.text;
        }
    }

    private void UpdatePasswordFromInputField()
    {
        if (logInPasswordInputField != null)
        {
            logInPassword = logInPasswordInputField.text;
        }

        if (signUpPassword != null)
        {
            signUpPassword = signUpPasswordInputField.text;
        }

        if (signUpPassword.Length < 8)
        {
            if (signUpButton != null)
            {
                signUpButton.SetActive(false);
            }
        }
        else
        {
            if (signUpButton != null)
            {
                signUpButton.SetActive(true);
            }
        }
    }

    private void SetPlayerNameFromInputField()
    {
        if (playerNameInputField != null)
        {
            playerName = playerNameInputField.text;
        }
    }

    public void Update()
    {
        UpdateEmailFromInputField();
        UpdatePasswordFromInputField();
        SetPlayerNameFromInputField();
    }

    public void SignUp()
    {
        LootLockerSDKManager.WhiteLabelSignUp(signUpEmail, signUpPassword, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Error while creating user");

                OperationFailed.SetActive(true);

                return;
            }

            Debug.Log("User created successfully");

            VerifyPage.SetActive(true);
            SignUpPage.SetActive(false);

            OperationFailed.SetActive(false);

        });
    }

    public void LogIn()
    {
        LootLockerSDKManager.WhiteLabelLoginAndStartSession(logInEmail, logInPassword, rememberMe, response =>
        {
            if (!response.success)
            {
                if (!response.LoginResponse.success)
                {
                    Debug.Log("error while logging in");

                    OperationFailed.SetActive(true);

                    LogInButton.SetActive(true);
                    SignUpButton.SetActive(true);

                    signUpPage.SetActive(false);
                    logInPage.SetActive(false);

                    Wait.SetActive(false);      
                }
                else if (!response.SessionResponse.success)
                {
                    Debug.Log("error while starting session");

                    OperationFailed.SetActive(true);

                    LogInButton.SetActive(true);
                    SignUpButton.SetActive(true);

                    signUpPage.SetActive(false);
                    logInPage.SetActive(false);

                    Wait.SetActive(false);
                }
                return;

                Authentication.SetActive(false);

                CallMoveCameraToTarget1();
            }

            LootLockerSDKManager.CheckWhiteLabelSession(response =>
            {
                if (response)
                {
                    LootLockerSDKManager.StartWhiteLabelSession((response) =>
                    {
                        if (!response.success)
                        {
                            Debug.Log("error starting LootLocker session");

                            OperationFailed.SetActive(true);

                            return;
                        }

                        Debug.Log("session started successfully");

                        LootLockerSDKManager.GetPlayerName((response) =>
                        {
                            if (response.success)
                            {
                                Debug.Log("Successfully retrieved player name: " + response.name);

                                if (response.name == "")
                                {
                                    setPlayerName.SetActive(true);
                                } else
                                {
                                    Authentication.SetActive(false);

                                    CallMoveCameraToTarget1();

                                    PlayerPrefs.SetString("SavedEmail", logInEmail);
                                    PlayerPrefs.SetString("SavedPassword", logInPassword);
                                    PlayerPrefs.Save();

                                    mainMenu.SetActive(true);
                                }
                            }
                            else
                            {
                                Debug.Log("Error getting player name");
                            }
                        });
                    });
                }
                else
                {
                    Debug.Log("session is NOT valid, we should show the login form");

                    LogInButton.SetActive(true);
                    SignUpButton.SetActive(true);
                    Wait.SetActive(false);
                }
            });

        }, rememberMe);
    }

    public void ResetPassword()
    {
        LootLockerSDKManager.WhiteLabelRequestPassword(resetEmail, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error requesting password reset");

                OperationFailed.SetActive(true);

                return;
            }

            Debug.Log("requested password reset successfully");

            LogInButton.SetActive(true);
            SignUpButton.SetActive(true);
            ResetPasswordPage.SetActive(false);

        });
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");

                Authentication.SetActive(false);

                CallMoveCameraToTarget1();

                PlayerPrefs.SetString("SavedEmail", logInEmail);
                PlayerPrefs.SetString("SavedPassword", logInPassword);
                PlayerPrefs.Save();

                mainMenu.SetActive(true);
            }
            else
            {
                Debug.Log("Error setting player name");
            }
        });
    }

    public void CallMoveCameraToTarget1()
    {
        if (cameraTransitionObject != null)
        {
            CameraTransition cameraTransition = cameraTransitionObject.GetComponent<CameraTransition>();

            if (cameraTransition != null)
            {
                cameraTransition.MoveCameraToTarget1();
            }
            else
            {
                Debug.LogError("CameraTransition component not found on the specified GameObject.");
            }
        }
        else
        {
            Debug.LogError("CameraTransition GameObject is not assigned.");
        }
    }
}
