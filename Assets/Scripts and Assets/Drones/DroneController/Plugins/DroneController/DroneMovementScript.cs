using UnityEngine;
using System.Collections;
using DroneController.CameraMovement;
using DroneController.Profiles;
using UnityEditor;

namespace DroneController
{
    namespace Profiles
    {
        [System.Serializable]
        public class Profile
        {
            public float maxSpeed;

            public float maxThrottleForce;
            public float maxPitchForce;
            public float maxRollForce;
            public float maxRotateForce;

            public bool angleLocked;
            public float angleLimit;

            public float dronePitchAmplifier;
            public float soundMultiplier;
            public float angularSoundMultiplier;
            public float staticSoundMultiplier;
            public float staticSoundStartPos;

            public float minDrag;
            public float maxDrag;
            public AnimationCurve speedValueCurve;

            public AnimationCurve inputThrottleCurve;
            public AnimationCurve inputPitchThrottleCurve;
            public AnimationCurve inputYawThrottleCurve;
            public AnimationCurve inputRollThrottleCurve;

            public float maxAngularDrag;
            public float minAngularDrag;
            public float angularDragZeroingTime;

            public float rotationSlowDown;
        }
    }
    namespace Physics
    {
        public enum JoystickDrivingAxis { none, pitch, roll, throttle, yaw }
        public class DroneMovementScript : MonoBehaviour
        {
            #region PUBLIC GETTERS

            public Vector3[] ProppelerForces
            {
                get
                {
                    return proppelerForces;
                }
            }

            public float[] ProppelerForceBasedOnJoystickInput
            {
                get
                {
                    return proppelerForceBasedOnJoystickInput;
                }
            }

            public int MaxPercentageOfForceDrop
            {
                get
                {
                    return maxPercentageOfForceDrop;
                }
            }

            public bool FlightRecorderOverride
            {
                get
                {
                    return flightRecorderOverride;
                }
                set
                {
                    flightRecorderOverride = value;
                }
            }

            public string Left_Analog_X
            {
                get
                {
                    return left_analog_x;
                }
            }

            public string Right_Analog_X
            {
                get
                {
                    return right_analog_x;
                }
            }

            public string Left_Analog_Y
            {
                get
                {
                    return left_analog_y;
                }
            }

            public string Right_Analog_Y
            {
                get
                {
                    return right_analog_y;
                }
            }

            public bool LeftHanded
            {
                get
                {
                    return leftHanded;
                }
                set
                {
                    leftHanded = value;
                }
            }

            public int HandedInput
            {
                get
                {
                    return handedInput;
                }
                set
                {
                    handedInput = value;
                }
            }

            public Rigidbody OurDrone
            {
                get
                {
                    return ourDrone;
                }
            }

            #endregion

            #region PUBLIC VARIABLES - EDITED THROUGH CUSTOM INSPECTOR

            //Force variables
            [HideInInspector] public Transform[] proppelers;

            [HideInInspector] public float maxSpeed;

            [HideInInspector] public float maxAngularDrag;
            [HideInInspector] public float minAngularDrag;
            [HideInInspector] public float angularDragZeroingTime;

            [HideInInspector] public float maxThrottleForce;
            [HideInInspector] public float maxRollForce;
            [HideInInspector] public float maxPitchForce;
            [HideInInspector] public float maxRotateForce;

            [HideInInspector] public bool angleLocked;
            [HideInInspector] public float angleLimit;

            [HideInInspector] public float minDrag;
            [HideInInspector] public float maxDrag;
            [HideInInspector] public AnimationCurve dragValueCurve;

            [HideInInspector] public float currentThrottle;
            [HideInInspector] public AnimationCurve inputThrottleCurve;

            [HideInInspector] public float currentYawThrottle;
            [HideInInspector] public AnimationCurve inputYawThrottleCurve;

            [HideInInspector] public float currentPitchThrottle;
            [HideInInspector] public AnimationCurve inputPitchThrottleCurve;

            [HideInInspector] public float currentRollThrottle;
            [HideInInspector] public AnimationCurve inputRollThrottleCurve;

            [HideInInspector] public int _profileIndex; //index where we select profiles in custom editor
            [HideInInspector] public Profile[] profiles = new Profile[3]; // index = mode    ->      0 = advanced, 1 = intermediate, 2 = begginer

            [HideInInspector] public int inputEditorSelection; //keyboard or joystick  index for custom editor

            [Tooltip("Check this if you want to use your joystick. (DON'T FORGET TO ADJUST THE INPUT SETTINGS!!)")]
            [HideInInspector] public bool joystick_turned_on = false;
            [HideInInspector] public CameraScript mainCamera;

            [Tooltip("Center Part of actual drone hierarchy.")]
            [HideInInspector] public Transform droneObject;

            [Tooltip("Just a reading of current velocity")]
            [HideInInspector] public float velocity; //check for speed
            [HideInInspector] public float angularVelocity; //check for speed

            [HideInInspector] public float dronePitchAmplifier = 1;
            [HideInInspector] public float soundMultiplier;

            [HideInInspector] public float staticSoundMultiplier;
            [HideInInspector] public float staticSoundStartPos;//position to start from, or the new '0' so its always there as a noise...

            [HideInInspector] public float angularSoundMultiplier;

            [HideInInspector] public float rotationSlowDown = 8;



            //ON DRAW GIZMO SETTINGS, color, radius and center of mass
            [HideInInspector] public Vector3 centerOfMass;
            [HideInInspector] public float centerOfMassRadiusGizmo = 0.1f;
            [HideInInspector] public Color centerOfMassGizmoColor = Color.green;

            #endregion

            #region PUBLIC VARIABLES - INPUT SETTINGS

            [Header("JOYSTICK AXIS INPUT")]
            [HideInInspector] public bool leftHanded = false;
            [HideInInspector] public int handedInput;
            [HideInInspector] public string left_analog_x = "Horizontal";
            [HideInInspector] public string left_analog_y = "Vertical";
            [HideInInspector] public string right_analog_x = "Horizontal_Right";
            [HideInInspector] public string right_analog_y = "Horizontal_UpDown";
            [HideInInspector] public KeyCode downButton = KeyCode.JoystickButton13;
            [HideInInspector] public KeyCode upButton = KeyCode.JoystickButton14;
            [HideInInspector] public JoystickDrivingAxis left_analog_y_movement = JoystickDrivingAxis.pitch;
            [HideInInspector] public JoystickDrivingAxis left_analog_x_movement = JoystickDrivingAxis.roll;
            [HideInInspector] public JoystickDrivingAxis right_analog_y_movement = JoystickDrivingAxis.throttle;
            [HideInInspector] public JoystickDrivingAxis right_analog_x_movement = JoystickDrivingAxis.yaw;

            [Header("INPUT TRANSLATED FOR KEYBOARD CONTROLS")]
            [HideInInspector] public bool W;
            [HideInInspector] public bool S;
            [HideInInspector] public bool A;
            [HideInInspector] public bool D;
            [HideInInspector] public bool I;
            [HideInInspector] public bool K;
            [HideInInspector] public bool J;
            [HideInInspector] public bool L;

            [Header("Keyboard Inputs")]
            [HideInInspector] public KeyCode forward = KeyCode.W;
            [HideInInspector] public KeyCode backward = KeyCode.S;
            [HideInInspector] public KeyCode rightward = KeyCode.D;
            [HideInInspector] public KeyCode leftward = KeyCode.A;
            [HideInInspector] public KeyCode upward = KeyCode.I;
            [HideInInspector] public KeyCode downward = KeyCode.K;
            [HideInInspector] public KeyCode rotateRightward = KeyCode.L;
            [HideInInspector] public KeyCode rotateLeftward = KeyCode.J;

            /// <summary>
            /// Custom Feed Input Variables
            /// </summary>
            [HideInInspector] public float CustomFeed_pitch;
            [HideInInspector] public float CustomFeed_roll;
            [HideInInspector] public float CustomFeed_yaw;
            [HideInInspector] public float CustomFeed_throttle;
            [HideInInspector] public bool customFeed; // do we use custom input for the inputs?


            #endregion

            #region PRIVATE VARIABLES

            //Final forces that are applied to the drone
            private Vector3 throttleForce;
            private Vector3 yawThrottleForce;
            private Vector3 pitchThrottleForce;
            private Vector3 rollThrottleForce;
            private Vector3 rollForce;
            private float[] proppelerSpeedPercentage = new float[4];

            Rigidbody ourDrone;
            AudioSource droneSound;
            AudioSource droneStaticSound;
            AudioSource droneAngularSound;

            private Vector3 velocityToSmoothDampToZero;

            [HideInInspector] public float Vertical_W = 0;
            private float Vertical_S = 0;
            private float Horizontal_A = 0;
            [HideInInspector] public float Horizontal_D = 0;
            [HideInInspector] public float Vertical_I = 0;
            private float Vertical_K = 0;
            private float Horizontal_J = 0;
            [HideInInspector] public float Horizontal_L = 0;

            private Vector3[] proppelerForces; //forces calculated for the thurst on the proppelers
            private float[] proppelerForceBasedOnJoystickInput; //forces calculated on the proppelers based on jyostick input, for ex. if you full roll to the right, only left proppelers should produce force, thus powering up left proppelers to the current force
            private int maxPercentageOfForceDrop = 30; //percentage of how much is yaw/throttle/roll dispalyed on the UI graph

            private Vector3 OurDroneTransformUp;

            private bool flightRecorderOverride;
            #endregion

            #region Threaded Methods

            float fixedDeltaTime;
            float deltaTime;

            public float throttlespeed;
            public float pitchyawrollspeed;

            public void ResetPitch()
            {
                CustomFeed_pitch = 0;
                Vertical_W = 0;
                Vertical_S = 0;

                Cursor.visible = false;
            }

            public void ResetYaw()
            {
                CustomFeed_yaw = 0;
                Horizontal_A = 0;
                Horizontal_D = 0;

                Cursor.visible = false;
            }

            public void ResetRoll()
            {
                CustomFeed_roll = 0;
                Horizontal_J = 0;
                Horizontal_L = 0;

                Cursor.visible = false;
            }

            public void ResetThrottle()
            {
                CustomFeed_throttle = 0;
                Vertical_I = 0;
                Vertical_S = 0;

                Cursor.visible = false;
            }

            public void MobileForward()
            {
                CustomFeed_pitch = pitchyawrollspeed;
                Vertical_W = pitchyawrollspeed;

                Cursor.visible = false;
            }

            public void MobileBackward()
            {
                CustomFeed_pitch = pitchyawrollspeed;
                Vertical_S = pitchyawrollspeed;

                Cursor.visible = false;
            }

            public void MobileYawLeft()
            {
                CustomFeed_yaw = pitchyawrollspeed;
                Horizontal_A = pitchyawrollspeed;

                Cursor.visible = false;
            }

            public void MobileYawRight()
            {
                CustomFeed_yaw = pitchyawrollspeed;
                Horizontal_D = pitchyawrollspeed;

                Cursor.visible = false;
            }

            public void MobileThrottleUp()
            {
                CustomFeed_throttle = throttlespeed;
                Vertical_I = throttlespeed;

                Cursor.visible = false;
            }

            public void MobileRollLeft()
            {
                CustomFeed_roll = pitchyawrollspeed;
                Horizontal_J = pitchyawrollspeed;

                Cursor.visible = false;
            }

            public void MobileRollRight()
            {
                CustomFeed_roll = pitchyawrollspeed;
                Horizontal_L = pitchyawrollspeed;

                Cursor.visible = false;
            }



            IEnumerator MotorsForceLogic()
            {
                while (true)
                {
                    float decreaseEffectOfYaw = 0.5f;
                    proppelerForceBasedOnJoystickInput[0] = maxPercentageOfForceDrop / 100.0f * maxThrottleForce * (Horizontal_D - Vertical_W + (Horizontal_L * decreaseEffectOfYaw));
                    proppelerForceBasedOnJoystickInput[3] = maxPercentageOfForceDrop / 100.0f * maxThrottleForce * (Horizontal_D + Vertical_W - (Horizontal_L * decreaseEffectOfYaw));
                    proppelerForceBasedOnJoystickInput[1] = maxPercentageOfForceDrop / 100.0f * maxThrottleForce * (-Horizontal_D - Vertical_W - (Horizontal_L * decreaseEffectOfYaw));
                    proppelerForceBasedOnJoystickInput[2] = maxPercentageOfForceDrop / 100.0f * maxThrottleForce * (-Horizontal_D + Vertical_W + (Horizontal_L * decreaseEffectOfYaw));

                    Vector3[] finalProppelerForce = new Vector3[4]; // <- this is currently unused but will be nice to have in a sooner update
                    for (int i = 0; i < proppelerForces.Length; i++)
                    {
                        //if (proppelerForceBasedOnJoystickInput[i] < 0) proppelerForceBasedOnJoystickInput[i] = 0; //dont take force from our main thrusts
                        proppelerForces[i] = throttleForce * proppelerSpeedPercentage[i];
                        finalProppelerForce[i] = proppelerForces[i] + (OurDroneTransformUp * proppelerForceBasedOnJoystickInput[i]);
                    }

                    for (int i = 0; i < proppelerSpeedPercentage.Length; i++)
                    {
                        proppelerSpeedPercentage[i] = Mathf.Lerp(proppelerSpeedPercentage[i], 1, fixedDeltaTime * 10);
                    }

                    yield return new WaitForEndOfFrame();
                }
            }

            IEnumerator DragManagerCalculation()
            {
                while (true)
                {
                    float evaluatedValue = dragValueCurve.Evaluate(velocity / maxSpeed);
                    currentDrag = maxDrag * evaluatedValue;
                    currentDrag = Mathf.Clamp(currentDrag, minDrag, maxDrag);
                    yield return new WaitForEndOfFrame();
                }

            }

            IEnumerator RotationCalculation()
            {
                while (true)
                {
                    if (joystick_turned_on == true || (joystick_turned_on == false && customFeed == true))
                    {
                        //not need for both since  L/J,W/S and A/D go to the same values, -1 -> 1				
                        currentYawThrottle = inputYawThrottleCurve.Evaluate((Horizontal_L >= 0) ? Horizontal_L : -Horizontal_L);
                        currentYawThrottle *= Horizontal_L >= 0 ? 1 : -1;
                        yawThrottleForce = maxRotateForce * ourDroneTransformUp * currentYawThrottle;

                        currentPitchThrottle = inputPitchThrottleCurve.Evaluate(Vertical_W >= 0 ? Vertical_W : -Vertical_W);
                        currentPitchThrottle *= Vertical_W >= 0 ? 1 : -1;
                        pitchThrottleForce = maxPitchForce * ourDroneTransformRight * currentPitchThrottle;

                        currentRollThrottle = inputRollThrottleCurve.Evaluate(Horizontal_D > 0 ? Horizontal_D : -Horizontal_D);
                        currentRollThrottle *= Horizontal_D >= 0 ? 1 : -1;
                        rollThrottleForce = maxRollForce * -ourDroneTransformForward * currentRollThrottle;

                        if (angleLocked)
                        {
                            if (ourDroneRotation.x > angleLimit && ourDroneRotation.x < 180)
                            {
                                if (currentPitchThrottle > 0)
                                {
                                    pitchThrottleForce = Vector3.zero;
                                }
                            }
                            else if (ourDroneRotation.x < 360 - angleLimit && ourDroneRotation.x > 180)
                            {
                                if (currentPitchThrottle < 0)
                                {
                                    pitchThrottleForce = Vector3.zero;
                                }
                            }

                            if (ourDroneRotation.z > angleLimit && ourDroneRotation.z < 180)
                            {
                                if (currentRollThrottle < 0)
                                {
                                    rollThrottleForce = Vector3.zero;
                                }
                            }
                            else if (ourDroneRotation.z < 360 - angleLimit && ourDroneRotation.z > 180)
                            {
                                if (currentRollThrottle > 0)
                                {
                                    rollThrottleForce = Vector3.zero;
                                }
                            }
                        }

                        //rollForce = Horizontal_D * maxRollForce * -ourDroneTransformForward + Vertical_W * maxPitchForce * ourDroneTransformRight + Horizontal_L * maxRotateForce * ourDroneTransformUp;
                        rollForce = rollThrottleForce + pitchThrottleForce + yawThrottleForce;
                    }
                    else if (joystick_turned_on == false)
                    {
                        Vector3 pitching_forward = Vertical_W * maxRollForce * ourDroneTransformRight;
                        Vector3 pitching_backward = Vertical_S * maxRollForce * ourDroneTransformRight;
                        Vector3 rolling_right = Horizontal_D * maxRollForce * -ourDroneTransformForward;
                        Vector3 rolling_left = Horizontal_A * maxRollForce * -ourDroneTransformForward;
                        Vector3 yawing_right = Horizontal_L * maxRotateForce * -ourDroneTransformUp;
                        Vector3 yawing_left = Horizontal_J * maxRotateForce * -ourDroneTransformUp;

                        rollForce = rolling_right + rolling_left + pitching_forward + pitching_backward + yawing_left + yawing_right;
                    }

                    yield return new WaitForEndOfFrame();
                }
            }

            #endregion

            #region Mono Behaviour METHODS

            public virtual void Awake()
            {
                ourDrone = GetComponent<Rigidbody>();
                StartCoroutine(FindMainCamera());
                FindDroneSoundComponent();

                //setting intial values for collision percentages ... to 1
                for (int i = 0; i < proppelerSpeedPercentage.Length; i++) proppelerSpeedPercentage[i] = 1;

                proppelerForces = new Vector3[4];
                proppelerForceBasedOnJoystickInput = new float[4];
            }

            private void OnEnable()
            {
                StartCoroutine(MotorsForceLogic());
                StartCoroutine(DragManagerCalculation());
                StartCoroutine(RotationCalculation());
            }

            private void OnDisable()
            {
                StopAllCoroutines();
            }

            public virtual void FixedUpdate()
            {
                fixedDeltaTime = Time.fixedDeltaTime;

                GetVelocity(); //just reading velocity
                MovementUpDown(); //hovering up and down
                PitchingRollingYawing(); //method name says it all... 
            }

            public virtual void Update()
            {
                deltaTime = Time.deltaTime;

                SettingCenterOffMass(); //updating our drone center of mass
                DragManager(); //making our drone not get infite speed
                DroneSound(); //sound producing stuff
                SettingControllerToInputSettings(); //sensitivity settings for joystick,keyboard,mobile (depending on which is turned on)
                CameraCorrectPickAndTranslatingInputToWSAD(); //setting input for keys, translating joystick, mobile inputs as WSAD (depending on which is turned on)
            }

            private void OnDrawGizmos()
            {
                Gizmos.color = centerOfMassGizmoColor;
                Gizmos.DrawSphere(transform.position + transform.forward * centerOfMass.z + transform.right * centerOfMass.x + transform.up * centerOfMass.y, centerOfMassRadiusGizmo);

                //runs in gamemode
                if (ourDrone)
                {
                    float rayLength = 0.25f;

                    //Representation of forces that are applied to the drone seen in the scene view.
                    Gizmos.color = (currentPitchThrottle > 0) ? Color.red : Color.green;
                    Gizmos.DrawRay(ourDrone.transform.position, pitchThrottleForce * rayLength);

                    Gizmos.color = (currentRollThrottle > 0) ? Color.red : Color.green;
                    Gizmos.DrawRay(ourDrone.transform.position, rollThrottleForce * rayLength);

                    Gizmos.color = (currentYawThrottle > 0) ? Color.red : Color.green;
                    Gizmos.DrawRay(ourDrone.transform.position, yawThrottleForce * rayLength);

                    Gizmos.color = (currentThrottle > 0) ? Color.red : Color.green;
                    Gizmos.DrawRay(ourDrone.transform.position, throttleForce * rayLength);

                    for (int i = 0; i < proppelerForces.Length; i++)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawRay(proppelers[i].position, proppelerForces[i] * rayLength);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawRay(proppelers[i].position + (ourDrone.transform.forward * 0.01f) + (proppelerForces[i] * rayLength), ourDrone.transform.up * proppelerForceBasedOnJoystickInput[i] * rayLength);
                    }
                }
            }

            #endregion

            #region PRIVATE METHODS

            /// <summary>
            /// Tries to find drone sound component upon script awake
            /// </summary>
            void FindDroneSoundComponent()
            {
                try
                {
                    if (gameObject.transform.Find("drone_sound").GetComponent<AudioSource>())
                    {
                        droneSound = gameObject.transform.Find("drone_sound").GetComponent<AudioSource>();
                    }
                    else
                    {
                        print("Found drone_sound but it has no AudioSource component.");
                    }
                }
                catch (System.Exception ex)
                {
                    print("No Sound Child GameObject ->" + ex.StackTrace.ToString());
                }

                try
                {
                    if (gameObject.transform.Find("drone_angularSound").GetComponent<AudioSource>())
                    {
                        droneAngularSound = gameObject.transform.Find("drone_angularSound").GetComponent<AudioSource>();
                    }
                    else
                    {
                        print("Found drone_sound but it has no AudioSource component.");
                    }
                }
                catch (System.Exception ex)
                {
                    print("No Sound Child GameObject ->" + ex.StackTrace.ToString());
                }

                try
                {
                    if (gameObject.transform.Find("drone_staticSound").GetComponent<AudioSource>())
                    {
                        droneStaticSound = gameObject.transform.Find("drone_staticSound").GetComponent<AudioSource>();
                    }
                    else
                    {
                        print("Found drone_sound but it has no AudioSource component.");
                    }
                }
                catch (System.Exception ex)
                {
                    print("No Sound Child GameObject ->" + ex.StackTrace.ToString());
                }
            }

            /// <summary>
            /// Translating Left Y Analog to WSAD, IJKL inputs
            /// </summary>
            void Left_Analog_Y_Translation()
            {
                if (left_analog_y_movement == JoystickDrivingAxis.pitch)
                {
                    W = (Input.GetAxisRaw(left_analog_y) > 0) ? true : false;
                    S = (Input.GetAxisRaw(left_analog_y) < 0) ? true : false;
                }
                else if (left_analog_y_movement == JoystickDrivingAxis.roll)
                {
                    D = (Input.GetAxisRaw(left_analog_y) > 0) ? true : false;
                    A = (Input.GetAxisRaw(left_analog_y) < 0) ? true : false;
                }
                else if (left_analog_y_movement == JoystickDrivingAxis.throttle)
                {
                    I = (Input.GetAxisRaw(left_analog_y) > 0) ? true : false;
                    K = (Input.GetAxisRaw(left_analog_y) < 0) ? true : false;
                }
                else if (left_analog_y_movement == JoystickDrivingAxis.yaw)
                {
                    J = (-Input.GetAxisRaw(left_analog_y) > 0) ? true : false;
                    L = (-Input.GetAxisRaw(left_analog_y) < 0) ? true : false;
                }
            }

            /// <summary>
            /// Translating Left X Analog to WSAD, IJKL inputs
            /// </summary>
            void Left_Analog_X_Translation()
            {
                if (left_analog_x_movement == JoystickDrivingAxis.pitch)
                {
                    W = (Input.GetAxisRaw(left_analog_x) > 0) ? true : false;
                    S = (Input.GetAxisRaw(left_analog_x) < 0) ? true : false;
                }
                else if (left_analog_x_movement == JoystickDrivingAxis.roll)
                {
                    D = (Input.GetAxisRaw(left_analog_x) > 0) ? true : false;
                    A = (Input.GetAxisRaw(left_analog_x) < 0) ? true : false;
                }
                else if (left_analog_x_movement == JoystickDrivingAxis.throttle)
                {
                    I = (Input.GetAxisRaw(left_analog_x) > 0) ? true : false;
                    K = (Input.GetAxisRaw(left_analog_x) < 0) ? true : false;
                }
                else if (left_analog_x_movement == JoystickDrivingAxis.yaw)
                {
                    J = (-Input.GetAxisRaw(left_analog_x) > 0) ? true : false;
                    L = (-Input.GetAxisRaw(left_analog_x) < 0) ? true : false;
                }
            }

            /// <summary>
            /// Translating Right Y Analog to WSAD, IJKL inputs
            /// </summary>
            void Right_Analog_Y_Translation()
            {
                if (right_analog_y_movement == JoystickDrivingAxis.pitch)
                {
                    W = (-Input.GetAxisRaw(right_analog_y) > 0) ? true : false;
                    S = (-Input.GetAxisRaw(right_analog_y) < 0) ? true : false;
                }
                else if (right_analog_y_movement == JoystickDrivingAxis.roll)
                {
                    D = (Input.GetAxisRaw(right_analog_y) > 0) ? true : false;
                    A = (Input.GetAxisRaw(right_analog_y) < 0) ? true : false;
                }
                else if (right_analog_y_movement == JoystickDrivingAxis.throttle)
                {
                    I = (-Input.GetAxisRaw(right_analog_y) > 0) ? true : false;
                    K = (-Input.GetAxisRaw(right_analog_y) < 0) ? true : false;
                }
                else if (right_analog_y_movement == JoystickDrivingAxis.yaw)
                {
                    J = (-Input.GetAxisRaw(right_analog_y) > 0) ? true : false;
                    L = (-Input.GetAxisRaw(right_analog_y) < 0) ? true : false;
                }
            }

            /// <summary>
            /// Translating Right X Analog to WSAD, IJKL inputs
            /// </summary>
            void Right_Analog_X_Translation()
            {
                if (right_analog_x_movement == JoystickDrivingAxis.pitch)
                {
                    W = (-Input.GetAxisRaw(right_analog_x) > 0) ? true : false;
                    S = (-Input.GetAxisRaw(right_analog_x) < 0) ? true : false;
                }
                else if (right_analog_x_movement == JoystickDrivingAxis.roll)
                {
                    D = (Input.GetAxisRaw(right_analog_x) > 0) ? true : false;
                    A = (Input.GetAxisRaw(right_analog_x) < 0) ? true : false;
                }
                else if (right_analog_x_movement == JoystickDrivingAxis.throttle)
                {
                    I = (Input.GetAxisRaw(right_analog_x) > 0) ? true : false;
                    K = (Input.GetAxisRaw(right_analog_x) < 0) ? true : false;
                }
                else if (right_analog_x_movement == JoystickDrivingAxis.yaw)
                {
                    J = (-Input.GetAxisRaw(right_analog_x) > 0) ? true : false;
                    L = (-Input.GetAxisRaw(right_analog_x) < 0) ? true : false;
                }
            }

            /// <summary>
            /// Faking smooth press on a mobile key or keyboard, faking a joystick sstick moving up...
            /// </summary>
            void Input_Mobile_Sensitvity_Calculation()
            {
                if (W == true)
                    Vertical_W = Mathf.LerpUnclamped(Vertical_W, 1, Time.deltaTime * 10);
                else Vertical_W = Mathf.LerpUnclamped(Vertical_W, 0, Time.deltaTime * 10);

                if (S == true)
                    Vertical_S = Mathf.LerpUnclamped(Vertical_S, -1, Time.deltaTime * 10);
                else Vertical_S = Mathf.LerpUnclamped(Vertical_S, 0, Time.deltaTime * 10);

                if (A == true)
                    Horizontal_A = Mathf.LerpUnclamped(Horizontal_A, -1, Time.deltaTime * 10);
                else Horizontal_A = Mathf.LerpUnclamped(Horizontal_A, 0, Time.deltaTime * 10);

                if (D == true)
                    Horizontal_D = Mathf.LerpUnclamped(Horizontal_D, 1, Time.deltaTime * 10);
                else Horizontal_D = Mathf.LerpUnclamped(Horizontal_D, 0, Time.deltaTime * 10);

                if (I == true)
                    Vertical_I = Mathf.LerpUnclamped(Vertical_I, 1, Time.deltaTime * 10);
                else Vertical_I = Mathf.LerpUnclamped(Vertical_I, 0, Time.deltaTime * 10);

                if (K == true)
                    Vertical_K = Mathf.LerpUnclamped(Vertical_K, -1, Time.deltaTime * 10);
                else Vertical_K = Mathf.LerpUnclamped(Vertical_K, 0, Time.deltaTime * 10);

                if (J == true)
                    Horizontal_J = Mathf.LerpUnclamped(Horizontal_J, 1, Time.deltaTime * 10);
                else Horizontal_J = Mathf.LerpUnclamped(Horizontal_J, 0, Time.deltaTime * 10);

                if (L == true)
                    Horizontal_L = Mathf.LerpUnclamped(Horizontal_L, -1, Time.deltaTime * 10);
                else Horizontal_L = Mathf.LerpUnclamped(Horizontal_L, 0, Time.deltaTime * 10);

                Debug.Log(Vertical_W + " " + Vertical_I);
            }

            /// <summary>
            /// Since we can change keys and bind joystick axis to any action, this method reads the input axis in original way so it knows what to do with it.
            /// Translating joystick input
            /// </summary>
            void Joystick_Input_Sensitivity_Calculation() //getting the input from joystick and determening where to move
            {
                Left_Analog_Y_Movement();
                Left_Analog_X_Movement();
                Right_Analog_Y_Movement();
                Right_Analog_X_Movement();
            }

            //reads the inputs of the wanted axis to behave and moves it in that direction
            void Left_Analog_Y_Movement()
            {
                if (left_analog_y_movement == JoystickDrivingAxis.pitch)
                {
                    Vertical_W = Input.GetAxis(left_analog_y);
                    Vertical_S = Input.GetAxis(left_analog_y);

                    CustomFeed_pitch = Vertical_W;
                }
                else if (left_analog_y_movement == JoystickDrivingAxis.roll)
                {
                    Horizontal_D = Input.GetAxis(left_analog_y);
                    Horizontal_A = Input.GetAxis(left_analog_y);

                    CustomFeed_roll = Horizontal_D;
                }
                else if (left_analog_y_movement == JoystickDrivingAxis.throttle)
                {
                    Vertical_I = Input.GetAxis(left_analog_y);
                    Vertical_K = Input.GetAxis(left_analog_y);

                    CustomFeed_throttle = Vertical_I;
                }
                else if (left_analog_y_movement == JoystickDrivingAxis.yaw)
                {
                    Horizontal_J = Input.GetAxis(left_analog_y);
                    Horizontal_L = Input.GetAxis(left_analog_y);

                    CustomFeed_yaw = Horizontal_J;
                }
            }
            void Left_Analog_X_Movement()
            {
                if (left_analog_x_movement == JoystickDrivingAxis.pitch)
                {
                    Vertical_W = Input.GetAxis(left_analog_x);
                    Vertical_S = Input.GetAxis(left_analog_x);

                    CustomFeed_pitch = Vertical_W;
                }
                else if (left_analog_x_movement == JoystickDrivingAxis.roll)
                {
                    Horizontal_D = Input.GetAxis(left_analog_x);
                    Horizontal_A = Input.GetAxis(left_analog_x);

                    CustomFeed_roll = Horizontal_D;
                }
                else if (left_analog_x_movement == JoystickDrivingAxis.throttle)
                {
                    Vertical_I = Input.GetAxis(left_analog_x);
                    Vertical_K = Input.GetAxis(left_analog_x);

                    CustomFeed_throttle = Vertical_I;
                }
                else if (left_analog_x_movement == JoystickDrivingAxis.yaw)
                {
                    Horizontal_J = Input.GetAxis(left_analog_x);
                    Horizontal_L = Input.GetAxis(left_analog_x);

                    CustomFeed_yaw = Horizontal_J;
                }
            }
            void Right_Analog_Y_Movement()
            {
                if (right_analog_y_movement == JoystickDrivingAxis.pitch)
                {
                    Vertical_W = -Input.GetAxis(right_analog_y);
                    Vertical_S = -Input.GetAxis(right_analog_y);

                    CustomFeed_pitch = Vertical_W;
                }
                else if (right_analog_y_movement == JoystickDrivingAxis.roll)
                {
                    Horizontal_D = Input.GetAxis(right_analog_y);
                    Horizontal_A = Input.GetAxis(right_analog_y);

                    CustomFeed_roll = Horizontal_D;
                }
                else if (right_analog_y_movement == JoystickDrivingAxis.throttle)
                {
                    Vertical_I = -Input.GetAxis(right_analog_y);
                    Vertical_K = -Input.GetAxis(right_analog_y);

                    CustomFeed_throttle = Vertical_I;
                }
                else if (right_analog_y_movement == JoystickDrivingAxis.yaw)
                {
                    Horizontal_J = Input.GetAxis(right_analog_y);
                    Horizontal_L = Input.GetAxis(right_analog_y);

                    CustomFeed_yaw = Horizontal_J;
                }
            }
            void Right_Analog_X_Movement()
            {
                if (right_analog_x_movement == JoystickDrivingAxis.pitch)
                {
                    Vertical_W = -Input.GetAxis(right_analog_x);
                    Vertical_S = -Input.GetAxis(right_analog_x);

                    CustomFeed_pitch = Vertical_W;
                }
                else if (right_analog_x_movement == JoystickDrivingAxis.roll)
                {
                    Horizontal_D = Input.GetAxis(right_analog_x);
                    Horizontal_A = Input.GetAxis(right_analog_x);

                    CustomFeed_roll = Horizontal_D;
                }
                else if (right_analog_x_movement == JoystickDrivingAxis.throttle)
                {
                    Vertical_I = Input.GetAxis(right_analog_x);
                    Vertical_K = Input.GetAxis(right_analog_x);

                    CustomFeed_throttle = Vertical_I;
                }
                else if (right_analog_x_movement == JoystickDrivingAxis.yaw)
                {
                    Horizontal_J = Input.GetAxis(right_analog_x);
                    Horizontal_L = Input.GetAxis(right_analog_x);

                    CustomFeed_yaw = Horizontal_J;
                }
            }

            #endregion

            #region PUBLIC METHODS

            /// <summary>
            /// changing drag based on speed of our drone
            /// </summary>
            float currentDrag;
            public void DragManager()
            {
                //CALCULATED IN A THREAD FROM NOW ON...
                /*
				float evaluatedValue = dragValueCurve.Evaluate(velocity / maxSpeed);
				currentDrag = maxDrag * evaluatedValue;
				currentDrag = Mathf.Clamp(currentDrag, minDrag, maxDrag);
				*/
                ourDrone.drag = currentDrag; //calculated in a thread
            }

            /// <summary>
            /// Just getting velocity from our rigidbody
            /// </summary>
            public void GetVelocity()
            {
                velocity = ourDrone.velocity.magnitude * 3.6f;
                angularVelocity = ourDrone.angularVelocity.magnitude;
            }

            /// <summary>
            /// 
            /// </summary>
            public void SettingCenterOffMass()
            {
                ourDrone.centerOfMass = centerOfMass;
            }

            /// <summary>
            /// Determining waether we are using joystick, mobile or a keyboard
            /// and does some calculations
            /// </summary>
            public void SettingControllerToInputSettings()
            {
                if (customFeed)
                {
                    CustomInputFeed();
                    return;
                }

                if (joystick_turned_on == false)
                    Input_Mobile_Sensitvity_Calculation();//returns lineary pressed WSAD keys for PC and mobile, joystick has sensitvity builtin by default
                else
                    Joystick_Input_Sensitivity_Calculation();
            }

            private void CustomInputFeed()
            {
                //PITCHING
                Vertical_W = CustomFeed_pitch;
                Vertical_S = CustomFeed_pitch;

                if (Vertical_W > 0) W = true;
                else W = false;
                if (Mathf.Abs(Vertical_S) > 0) S = true;
                else S = false;

                //ROLLING
                Horizontal_A = CustomFeed_roll;
                Horizontal_D = CustomFeed_roll;

                if (Mathf.Abs(Horizontal_A) > 0) A = true;
                else A = false;
                if (Horizontal_D > 0) D = true;
                else D = false;

                //THORTTLING
                Vertical_I = CustomFeed_throttle;
                Vertical_K = CustomFeed_throttle;
                if (Vertical_I > 0) I = true;
                else I = false;
                if (Mathf.Abs(Vertical_K) > 0) K = true;
                else K = false;

                //YAWING
                Horizontal_J = -CustomFeed_yaw;
                Horizontal_L = -CustomFeed_yaw;
                if (Mathf.Abs(Horizontal_J) > 0) J = true;
                else J = false;
                if (Horizontal_L > 0) L = true;
                else L = false;
            }

            /// <summary>
            /// Translating all the input weather its joystick, keyboard or mobile to WSAD at the end
            /// so it works cross platforms and if we add a new controller typed
            /// </summary>
            public void CameraCorrectPickAndTranslatingInputToWSAD()
            {
                /*
                 * If we picked the drone we wish to control and if its the same one as this one
                 * control only this drone, else remain uncontrolled
                 */
                if (customFeed == true || ourDrone.transform != transform)
                    return;

                if (joystick_turned_on == false)
                {
                    W = (Input.GetKey(forward)) ? true : false;
                    S = (Input.GetKey(backward)) ? true : false;
                    A = (Input.GetKey(leftward)) ? true : false;
                    D = (Input.GetKey(rightward)) ? true : false;

                    I = (Input.GetKey(upward)) ? true : false;
                    J = (Input.GetKey(rotateLeftward)) ? true : false;
                    K = (Input.GetKey(downward)) ? true : false;
                    L = (Input.GetKey(rotateRightward)) ? true : false;

                    return;
                }

                Left_Analog_Y_Translation();
                Left_Analog_X_Translation();
                Right_Analog_Y_Translation();
                Right_Analog_X_Translation();
            }

            /// <summary>
            /// Drone Sound Managing
            /// </summary>
            public void DroneSound()
            {
                if (droneSound)
                {
                    droneSound.pitch = 1 + (((Mathf.Abs(Vertical_I) + Mathf.Abs(Vertical_K) + Mathf.Abs(Horizontal_J) + Mathf.Abs(Horizontal_L))) * dronePitchAmplifier);
                    droneSound.volume = (Mathf.Abs(Vertical_I) + Mathf.Abs(Vertical_K) + Mathf.Abs(Horizontal_J) + Mathf.Abs(Horizontal_L)) * soundMultiplier;
                }
                if (droneAngularSound)
                {
                    droneAngularSound.volume = (Mathf.Abs(Vertical_W) + Mathf.Abs(Vertical_S) + Mathf.Abs(Horizontal_A) + Mathf.Abs(Horizontal_D)) * angularSoundMultiplier;
                }
                if (droneStaticSound)
                {
                    droneStaticSound.pitch = staticSoundStartPos + (angularVelocity * staticSoundMultiplier);
                }
            }

            /// <summary>
            /// when your drone flips to put it back in the position to fly...
            /// </summary>
            public void FlipDrone()
            {
                ourDrone.transform.eulerAngles = new Vector3(ourDrone.transform.eulerAngles.x, ourDrone.transform.eulerAngles.y + 180, 0);
            }

            /// <summary>
            /// Handling up down movement and applying needed force.
            /// </summary>
            public void MovementUpDown()
            {
                if (Vertical_I >= 0)
                {
                    currentThrottle = inputThrottleCurve.Evaluate(Vertical_I);
                    throttleForce = currentThrottle * maxThrottleForce * ourDrone.transform.up;
                }

                OurDroneTransformUp = ourDrone.transform.up;

                if (flightRecorderOverride) return; //if we are not playbacking our flight then use physics as per usual...

                for (int i = 0; i < proppelers.Length; i++)
                {
                    //This is waiting another update for better and more realistic movement
                    //if(ourDrone.transform.InverseTransformDirection(finalProppelerForce[i]).y > 0)
                    //	ourDrone.AddForceAtPosition(finalProppelerForce[i], proppelers[i].transform.position, ForceMode.Force);
                    ourDrone.AddForceAtPosition(proppelerForces[i], proppelers[i].transform.position, ForceMode.Force);
                }
            }

            /// <summary>
            /// Motions where you rotate your drone, pitching, rolling and yawing.
            /// </summary>
            Vector3 ourDroneTransformRight;
            Vector3 ourDroneTransformForward;
            Vector3 ourDroneTransformUp;
            Vector3 ourDroneRotation; // za thread
            public void PitchingRollingYawing()
            {

                //need to get data here to sync with the thread
                ourDroneTransformForward = ourDrone.transform.forward;
                ourDroneTransformRight = ourDrone.transform.right;
                ourDroneTransformUp = ourDrone.transform.up;
                ourDroneRotation = ourDrone.transform.rotation.eulerAngles;


                if (rollForce.magnitude > 0f)
                {
                    ourDrone.angularDrag = maxAngularDrag;
                }
                else
                {
                    ourDrone.angularDrag = Mathf.Lerp(ourDrone.angularDrag, minAngularDrag, Time.deltaTime * angularDragZeroingTime);
                }

                if (flightRecorderOverride) return; //if we are not playbacking our flight then use physics as per usual...

                ourDrone.angularVelocity = Vector3.Lerp(ourDrone.angularVelocity, Vector3.zero, Time.deltaTime * rotationSlowDown);
                ourDrone.angularVelocity += rollForce;
            }

            /// <summary>
            /// Updating values from editor script
            /// </summary>
            public void UpdateValuesFromEditor()
            {
                maxSpeed = profiles[_profileIndex].maxSpeed;

                maxThrottleForce = profiles[_profileIndex].maxThrottleForce;
                maxRollForce = profiles[_profileIndex].maxRollForce;
                maxPitchForce = profiles[_profileIndex].maxPitchForce;
                maxRotateForce = profiles[_profileIndex].maxRotateForce;

                angleLocked = profiles[_profileIndex].angleLocked;
                angleLimit = profiles[_profileIndex].angleLimit;

                dronePitchAmplifier = profiles[_profileIndex].dronePitchAmplifier;
                soundMultiplier = profiles[_profileIndex].soundMultiplier;
                angularSoundMultiplier = profiles[_profileIndex].angularSoundMultiplier;
                staticSoundMultiplier = profiles[_profileIndex].staticSoundMultiplier;
                staticSoundStartPos = profiles[_profileIndex].staticSoundStartPos;

                minDrag = profiles[_profileIndex].minDrag;
                maxDrag = profiles[_profileIndex].maxDrag;
                dragValueCurve = profiles[_profileIndex].speedValueCurve;

                inputThrottleCurve = profiles[_profileIndex].inputThrottleCurve;
                inputYawThrottleCurve = profiles[_profileIndex].inputYawThrottleCurve;
                inputPitchThrottleCurve = profiles[_profileIndex].inputPitchThrottleCurve;
                inputRollThrottleCurve = profiles[_profileIndex].inputRollThrottleCurve;

                maxAngularDrag = profiles[_profileIndex].maxAngularDrag;
                minAngularDrag = profiles[_profileIndex].minAngularDrag;
                angularDragZeroingTime = profiles[_profileIndex].angularDragZeroingTime;

                rotationSlowDown = profiles[_profileIndex].rotationSlowDown;
            }

            /// <summary>
            /// Will lower the power rate of this proppeler
            /// </summary>
            /// <param name="_proppeler"></param>
            public void SlowdownThisProppelerSpeed(Transform _proppeler, int _thatproppelerIndex)
            {
                proppelerSpeedPercentage[_thatproppelerIndex] = 0.4f;
            }

            #endregion

            #region PRIVATE Coroutine METHODS

            /// <summary>
            /// Keeps trying to find camera until we find it.
            /// </summary>
            IEnumerator FindMainCamera()
            {
                while (!mainCamera)
                {
                    try
                    {
                        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
                    }
                    catch (System.Exception e)
                    {
                        print("<color=red>Missing main camera! check the tags!</color> -> " + e);
                    }
                    yield return new WaitForEndOfFrame();
                }
            }

            #endregion

        }
    }
}