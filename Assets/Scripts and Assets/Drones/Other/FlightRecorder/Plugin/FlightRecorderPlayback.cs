using UnityEngine;
namespace FlightRecorderPlugin {

	interface IFlightRecorder<T>
	{
		void PrepareDroneForPlaybackFlight(T obj);
	}

	public class FlightRecorderPlayback : MonoBehaviour, IFlightRecorder<DroneController.Physics.DroneMovementScript>
	{

		[HideInInspector] public bool Playbacking;
		DroneMovement droneMovement;
		[HideInInspector]public bool LeftHanded;
		[HideInInspector] public float Roll, Pitch, Yaw, Throttle;

		public virtual void Awake()
		{
			droneMovement = GetComponent<DroneMovement>();
		}

		public virtual void Start()
		{
			if(Playbacking)
				PrepareDroneForPlaybackFlight(droneMovement);
		}

		/// <summary>
		/// Starts playing recored flight.
		/// </summary>
		/// <param name="_callback">On finish playing recorded flight.</param>
		public void StartPlayback(System.Action _callback)
		{
			if (dataFromSaveFile.Length != 0)
			{
				FlightRecorder.Instance.StartPlayingPlayback(dataFromSaveFile, this, () =>
				{
					_callback();
					//on flight playback finished...
					//Debug.Log("Playback finished");
				});
			}
			else
			{
				Debug.LogError("•Playback data not loaded.", gameObject);
			}
		}

		public virtual void Update()
		{
			//If not recording, we want to store these values
			if (Playbacking == false)
			{
				LeftHanded = droneMovement.LeftHanded;

				if (LeftHanded)
				{
					Roll = Input.GetAxis(droneMovement.Left_Analog_X);
					Pitch = Input.GetAxis(droneMovement.Left_Analog_Y);
					Yaw = Input.GetAxis(droneMovement.Right_Analog_X);
					Throttle = -Input.GetAxis(droneMovement.Right_Analog_Y);
				}
				else
				{
					Roll = Input.GetAxis(droneMovement.Right_Analog_X);
					Pitch = -Input.GetAxis(droneMovement.Right_Analog_Y);
					Yaw = Input.GetAxis(droneMovement.Left_Analog_X);
					Throttle = Input.GetAxis(droneMovement.Left_Analog_Y);
				}
			}
			else
			{
				//playbacking is filling these info because drone is on custom input then...
				droneMovement.CustomFeed_throttle = Throttle;
				droneMovement.CustomFeed_yaw = Yaw;
				droneMovement.CustomFeed_pitch = Pitch;
				droneMovement.CustomFeed_roll = Roll;
			}						
		}


		public string SaveFileName;
		[HideInInspector] public DataReadingStructure[] dataFromSaveFile;
		void DecodeSavedFile(string flightPath)
		{
			if (!FlightRecorder.Instance) //if load runtime
			{
				dataFromSaveFile = FindObjectOfType<FlightRecorder>().ReadRecodedFile(flightPath);
			}
			else //if load from editor
			{
				dataFromSaveFile = FlightRecorder.Instance.ReadRecodedFile(flightPath);
			}
		}

		public void LoadFlight(string flightPath)
		{
			string[] flightPathSplit = flightPath.Split('\\');
			SaveFileName = flightPathSplit[flightPathSplit.Length - 1];
			DecodeSavedFile(flightPath);
			Playbacking = (SaveFileName.Length > 0) ? true : false;
		}

		public void UnloadFlight()
		{
			SaveFileName = string.Empty;
			dataFromSaveFile = new DataReadingStructure[0];
			Playbacking = false;
		}

		public void PrepareDroneForPlaybackFlight(DroneController.Physics.DroneMovementScript obj)
		{
			//Debug.Log("Preparing my drone");
			obj.OurDrone.isKinematic = true;
			obj.OurDrone.useGravity = false;
			obj.customFeed = true;
			obj.inputEditorSelection = 0;
			obj.FlightRecorderOverride = true;
		}
	}
}