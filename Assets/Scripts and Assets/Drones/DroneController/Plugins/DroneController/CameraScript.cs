using UnityEngine;
using DroneController.Physics;
using System.Collections;
namespace DroneController
{
    namespace CameraMovement
    {
        public class CameraScript : MonoBehaviour
        {

			#region PUBLIC VARIABLES - EDITED THROUGH CUSTOM INSPECTOR EDITOR

			[HideInInspector] public int inputEditorFPS;

            [HideInInspector] public bool FPS;
            [HideInInspector] public Vector3 positionInsideDrone;
            [HideInInspector] public Vector3 rotationInsideDrone;
            [HideInInspector] public float fpsFieldOfView = 90;

            public GameObject ourDrone; //our drone game object

            [Header("Position of the camera behind the drone.")]
            [HideInInspector] public Vector3 positionBehindDrone = new Vector3(0, 2, -4);

            [Tooltip("How fast the camera will follow drone position. (The lower the value the faster it will follow)")]
            [Range(0.0f, 0.1f)]
            [HideInInspector] public float cameraFollowPositionTime = 0.1f;

            [Tooltip("Value where if the camera/drone is moving upwards will raise the camera view upward to get a better look at what is above, same goes when going downwards.")]
            [HideInInspector] public float extraTilt = 10;
            [Tooltip("Parts of drone we wish to see in the third person.")]
            [HideInInspector] public float tpsFieldOfView = 60;

            [Header("Mouse movement variables")]
            [Tooltip("Allows to freely view around the drone with your mouse and not depending on drone look rotation.")]
            [HideInInspector] public bool freeMouseMovement = false;
            [Tooltip("Value that will determine how fast your free look mouse will behave.")]
            [HideInInspector] public float mouseSensitvity = 100;
            [Tooltip("Value that will follow the camera view behind the mouse movement.(The lower the value, the faster it will follow mouse movement)")]
            [HideInInspector] public float mouseFollowTime = 0.2f;

            #endregion

            #region PRIVATE VARIABLES

            private Vector3 velocitiCameraFollow;

            private float cameraYVelocity;
            private float previousFramePos;

            private float currentXPos, currentYPos;
            private float xVelocity, yVelocity;

            private float mouseXwanted, mouseYwanted;

            private float zScrollAmountSensitivity = 1, yScrollAmountSensitivity = -0.5f;
            private float zScrollValue, yScrollValue;

            #endregion

			#region MONO BEHAVIOUR METHODS

			public virtual void Start()
			{
				StartCoroutine(KeepTryingToFindOurDrone());
			}

			public virtual void Awake()
			{

			}

			#endregion

			#region PRIVATE METHODS

			//only when FPS is toggled ON
			void FPSCameraPositioning()
            {
                if (transform.parent == null)
				{
					transform.SetParent(ourDrone.GetComponent<DroneMovementScript>().droneObject.transform);
				}

				transform.localPosition = positionInsideDrone;
                transform.localEulerAngles = rotationInsideDrone;
            }

            //only when FPS is toggled OFF (Third person view)
            void TPSCameraPositioning()
            {
                if (transform.parent != null)
				{
					transform.SetParent(null);
				}

				FollowDroneMethod();
            }

            void FollowDroneMethod()
            {
                if (ourDrone)
				{
					transform.position = Vector3.SmoothDamp(transform.position, ourDrone.transform.position + (positionBehindDrone + new Vector3(0, yScrollValue, zScrollValue)), ref velocitiCameraFollow, cameraFollowPositionTime);
					transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
				}
			}
            
            void TiltCameraUpDown()
            {
                cameraYVelocity = Mathf.Lerp(cameraYVelocity, (transform.position.y - previousFramePos) * -extraTilt, Time.deltaTime * 10);
                previousFramePos = transform.position.y;
            }

            void FreeMouseMovementView()
            {
                if (freeMouseMovement == true)
                {
                    mouseXwanted -= Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitvity;
                    mouseYwanted += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitvity;

                    currentXPos = Mathf.SmoothDamp(currentXPos, mouseXwanted, ref xVelocity, mouseFollowTime);
                    currentYPos = Mathf.SmoothDamp(currentYPos, mouseYwanted, ref yVelocity, mouseFollowTime);

                    transform.rotation = Quaternion.Euler(new Vector3(14, 0, 0)) *
                        Quaternion.Euler(currentXPos, currentYPos, 0);
                }
                else
                {
                    if (ourDrone)
                        transform.rotation = Quaternion.Euler(new Vector3(14 + cameraYVelocity, 0, 0));
                }
            }

            #endregion

            #region PUBLIC METHODS

            /// <summary>
            /// Checking which values for camera positioning should we use.
            /// If we are picking the drone, use third person view, and after we select it will switch to first person view
            /// </summary>
            public void FPVTPSCamera()
            {
                if (FPS == true)
                {
                    FPSCameraPositioning();
                }
                else
                {
                    TPSCameraPositioning();
                }
            }

            /// <summary>
            /// Handles scrolling of mouse wheel to zoomin the camera
            /// </summary>
            public void ScrollMath()
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                {
                    zScrollValue += Input.GetAxis("Mouse ScrollWheel") * zScrollAmountSensitivity;
                    yScrollValue += Input.GetAxis("Mouse ScrollWheel") * yScrollAmountSensitivity;
                }
            }

            #endregion

            #region PRIVATE Coroutine METHODS

            /// <summary>
            /// Keep trying to find our drone.
            /// This is needed if you want to connect your drones to network, so first you have your camera, and after you create your drone, you want it to be found by the camera.
            /// So this checks every frame if there is a new drone created.
            /// </summary>
            IEnumerator KeepTryingToFindOurDrone()
            {
                while (ourDrone == null)
                {
                    try
                    {
						//Find drone that is not flying on a recorded path
						DroneMovement[] drones = FindObjectsOfType<DroneMovement>();
						foreach(DroneMovement dm in drones)
						{
							if(dm.FlightRecorderOverride == false)
							{
								ourDrone = dm.gameObject;
							}
						}
						if (!ourDrone) ourDrone = drones[Random.Range(0, drones.Length - 1)].gameObject;
					}
					catch (System.Exception e)
                    {
                        print("Are you supposed to have only one drone on the scene? <color=red>I can't find it!</color> -> " + e);
                    }
					yield return null;
				}
			}

            #endregion

        }
    }
}
