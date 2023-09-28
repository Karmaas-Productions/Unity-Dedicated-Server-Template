using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

namespace FlightRecorderPlugin
{

	#region Classes and Structures

	[System.Serializable]
	public class DataReadingStructure
	{
		public float Roll;
		public float Pitch;
		public float Yaw;
		public float Throttle;
		public Vector3 Position;
		public Vector3 Rotation;
		public Vector3 Velocity;
		public Vector3 AngularVelocity;
		public float Drag;
		public float AngularDrag;
		public string LeftHanded;

		public DataReadingStructure(float roll, float pitch, float yaw, float throttle, Vector3 position, Vector3 rotation, Vector3 velocity, Vector3 angularVelocity, float drag, float angularDrag, string leftHanded)
		{
			Roll = roll;
			Pitch = pitch;
			Yaw = yaw;
			Throttle = throttle;
			Position = position;
			Rotation = rotation;
			Velocity = velocity;
			AngularVelocity = angularVelocity;
			Drag = drag;
			AngularDrag = angularDrag;
			LeftHanded = leftHanded;
		}
	}

	[System.Serializable]
	public struct LoadFlightStructure
	{
		//public GameObject _buttonPrefab;
		//public Transform _content;

		//Save file fields,rects,buttons
		public RectTransform _saveFileContent;
		public InputField _fileNameInput;
		public Button _saveFileNameButton;
	}

	#endregion

	public class FlightRecorder : MonoBehaviour
	{

		public static FlightRecorder Instance;
		public int FPS;
		public bool Recording
		{
			get
			{
				return _recording;
			}
		}
		public string FolderPath
		{
			set
			{
				_folderPath = value;
			}
			get
			{
				return _folderPath;
			}
		}

		[SerializeField] private bool _recording;
		[SerializeField, TextArea(3, 15)] private string _folderPath;
		[SerializeField] private LoadFlightStructure _loadFlightStructure;

		[Space(10)]
		public FlightRecorderPlayback PlaybackScript;

		public delegate void FlightRecorderEvents();
		public event FlightRecorderEvents OnRecordingStart;
		public event FlightRecorderEvents OnRecordingStop;

		#region MonoBehaviour Methods

		private void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
			}
		}

		private void OnDisable()
		{
			StopRecording();
		}

		private void Update()
		{
			FPS = (int)(1.0f / Time.deltaTime);
		}

		#endregion

		#region Storing and Reading data from and to our .txt file

		/// <summary>
		/// Data that we store every frame, that is parsed to string and stored per line in our .txt file.
		/// This data gets passed to the method that saves our data into the actual .txt file.
		/// </summary>
		List<string> inputData;
		/// <summary>
		/// Recording happens here... depending on the FPS... I hope you have an okay bucket (PC) ;)
		/// </summary>
		/// <returns></returns>
		IEnumerator Coroutine_Recording()
		{
			inputData = new List<string>();
			while (_recording)
			{
				yield return null;
				string decimalPoint = "F5"; //how precise input do you want to capture
				//roll,pitch,yaw,throttle in that particular order as in betaflight
				string posX = PlaybackScript.GetComponent<Rigidbody>().position.x.ToString();
				string posY = PlaybackScript.GetComponent<Rigidbody>().position.y.ToString();
				string posZ = PlaybackScript.GetComponent<Rigidbody>().position.z.ToString();
				string _dataToAdd =
					PlaybackScript.Roll.ToString(decimalPoint) + "#" +
					PlaybackScript.Pitch.ToString(decimalPoint) + "#" +
					PlaybackScript.Yaw.ToString(decimalPoint) + "#" +
					PlaybackScript.Throttle.ToString(decimalPoint) + "#" +
					"(" + posX + "," + posY + "," + posZ + ")" + "#" +
					PlaybackScript.GetComponent<Rigidbody>().rotation.eulerAngles + "#" +
					PlaybackScript.GetComponent<Rigidbody>().velocity + "#" +
					PlaybackScript.GetComponent<Rigidbody>().angularVelocity + "#" +
					PlaybackScript.GetComponent<Rigidbody>().drag + "#" +
					PlaybackScript.GetComponent<Rigidbody>().angularDrag + "#" +
					PlaybackScript.LeftHanded.ToString();
				inputData.Add(_dataToAdd);
			}

			if (OnRecordingStop != null) OnRecordingStop();
		}

		/// <summary>
		/// Reading data from text file
		/// </summary>
		public DataReadingStructure[] ReadRecodedFile(string saveFilePath)
		{
			//if (!File.Exists(_loadedFlight))
			//{
			//	Debug.LogError("•The file you're trying to load does not exstis.");
			//	return null;
			//}

			if (!File.Exists(saveFilePath))
			{
				Debug.LogError("•The file you're trying to load does not exstis.", gameObject);
				return null;
			}

			List<DataReadingStructure> readData = new List<DataReadingStructure>();
			string line;
			//StreamReader stream = new StreamReader(_loadedFlight);
			StreamReader stream = new StreamReader(saveFilePath);
			while ((line = stream.ReadLine()) != null)
			{
				string[] incomingData = line.Split('#');

				string cleanPositionVector = incomingData[4];
				cleanPositionVector = cleanPositionVector.Replace("(", "");
				cleanPositionVector = cleanPositionVector.Replace(")", "");
				string[] vectorSplit = cleanPositionVector.Split(',');
				Vector3 position = new Vector3(
					float.Parse(vectorSplit[0]),
					float.Parse(vectorSplit[1]),
					float.Parse(vectorSplit[2])
					);

				string cleanRotationQuaternion = incomingData[5];
				cleanRotationQuaternion = cleanRotationQuaternion.Replace("(", "");
				cleanRotationQuaternion = cleanRotationQuaternion.Replace(")", "");
				string[] quaternionSplit = cleanRotationQuaternion.Split(',');
				Vector3 rotation = new Vector3(
					float.Parse(quaternionSplit[0]),
					float.Parse(quaternionSplit[1]),
					float.Parse(quaternionSplit[2])
					);

				string cleanVelocityVector3 = incomingData[6];
				cleanVelocityVector3 = cleanVelocityVector3.Replace("(", "");
				cleanVelocityVector3 = cleanVelocityVector3.Replace(")", "");
				string[] velocitySplit = cleanVelocityVector3.Split(',');
				Vector3 velocity = new Vector3(
					float.Parse(velocitySplit[0]),
					float.Parse(velocitySplit[1]),
					float.Parse(velocitySplit[2])
					);

				string cleanAngularVelocityVector3 = incomingData[7];
				cleanAngularVelocityVector3 = cleanAngularVelocityVector3.Replace("(", "");
				cleanAngularVelocityVector3 = cleanAngularVelocityVector3.Replace(")", "");
				string[] angularVelocitySplit = cleanAngularVelocityVector3.Split(',');
				Vector3 angularVelocity = new Vector3(
					float.Parse(angularVelocitySplit[0]),
					float.Parse(angularVelocitySplit[1]),
					float.Parse(angularVelocitySplit[2])
					);

				readData.Add(
					new DataReadingStructure(
						float.Parse(incomingData[0]),
						float.Parse(incomingData[1]),
						float.Parse(incomingData[2]),
						float.Parse(incomingData[3]),
						position,
						rotation,
						velocity,
						angularVelocity,
						float.Parse(incomingData[8]),
						float.Parse(incomingData[9]),
						incomingData[10]
						)
					);
			}
			return readData.ToArray();
		}

		#endregion


		List<Coroutine> C_Playback = new List<Coroutine>();
		public void StartPlayingPlayback(DataReadingStructure[] dataFromSaveFile, FlightRecorderPlayback flightRecorderPlayback, Action callback)
		{
			C_Playback.Add(StartCoroutine(Coroutine_Playback(dataFromSaveFile, flightRecorderPlayback, callback)));
		}

		IEnumerator Coroutine_Playback(DataReadingStructure[] dataFromSaveFile, FlightRecorderPlayback flightRecorderPlayback, Action callback)
		{
			Debug.Log("•Started playing playback");

			//Position on starting od recording point
			flightRecorderPlayback.Pitch = dataFromSaveFile[0].Pitch;
			flightRecorderPlayback.Roll = dataFromSaveFile[0].Roll;
			flightRecorderPlayback.Yaw = dataFromSaveFile[0].Yaw;
			flightRecorderPlayback.Throttle = dataFromSaveFile[0].Throttle;
			flightRecorderPlayback.GetComponent<Rigidbody>().position = dataFromSaveFile[0].Position;
			flightRecorderPlayback.GetComponent<Rigidbody>().rotation = Quaternion.Euler(dataFromSaveFile[0].Rotation);
			flightRecorderPlayback.GetComponent<Rigidbody>().velocity = dataFromSaveFile[0].Velocity;
			flightRecorderPlayback.GetComponent<Rigidbody>().angularVelocity = dataFromSaveFile[0].AngularVelocity;
			flightRecorderPlayback.GetComponent<Rigidbody>().drag = dataFromSaveFile[0].Drag;
			flightRecorderPlayback.GetComponent<Rigidbody>().angularDrag = dataFromSaveFile[0].AngularDrag;
			flightRecorderPlayback.LeftHanded = bool.Parse(dataFromSaveFile[0].LeftHanded);

			//start playing data on our drone
			int i = 0;
			while (i < dataFromSaveFile.Length - 1)
			{
				yield return null;
				flightRecorderPlayback.Pitch = dataFromSaveFile[i].Pitch;
				flightRecorderPlayback.Roll = dataFromSaveFile[i].Roll;
				flightRecorderPlayback.Yaw = dataFromSaveFile[i].Yaw;
				flightRecorderPlayback.Throttle = dataFromSaveFile[i].Throttle;
				flightRecorderPlayback.GetComponent<Rigidbody>().position = Vector3.Lerp(flightRecorderPlayback.GetComponent<Rigidbody>().position, dataFromSaveFile[i].Position, Time.deltaTime * 15);
				flightRecorderPlayback.GetComponent<Rigidbody>().rotation = Quaternion.Lerp(flightRecorderPlayback.GetComponent<Rigidbody>().rotation, Quaternion.Euler(dataFromSaveFile[i].Rotation), Time.deltaTime * 15);
				flightRecorderPlayback.GetComponent<Rigidbody>().velocity = dataFromSaveFile[i].Velocity;
				flightRecorderPlayback.GetComponent<Rigidbody>().angularVelocity = dataFromSaveFile[i].AngularVelocity;
				flightRecorderPlayback.GetComponent<Rigidbody>().drag = dataFromSaveFile[i].Drag;
				flightRecorderPlayback.GetComponent<Rigidbody>().angularDrag = dataFromSaveFile[i].AngularDrag;
				flightRecorderPlayback.LeftHanded = bool.Parse(dataFromSaveFile[0].LeftHanded);
				i++;
			}

			Debug.Log("•Finished playing playback");
			callback.Invoke();
		}

		#region UI Button Methods

		/// <summary>
		/// Just a refference to our coroutine.
		/// </summary>
		Coroutine c_Recording;
		/// <summary>
		/// Starts recording our flight path.
		/// </summary>
		public void StartRecording()
		{
			//prevent someone just calling recording and glitching, we want to start recording only if we we were previously recording
			if (_recording) return;

			Debug.Log("•Started recording");
			if (OnRecordingStart != null) OnRecordingStart(); //delegate call for other actions to link on this
			_recording = true;
			if (c_Recording != null) StopCoroutine(c_Recording);
			c_Recording = StartCoroutine(Coroutine_Recording());

			//remove lsiteners from rename button
			_loadFlightStructure._saveFileNameButton.onClick.RemoveAllListeners();
			//hide change save file name if user clicks star again and didn't rename its file
			if (inputToggled) if (gameObject.activeInHierarchy) StartCoroutine(Coroutine_HideSaveFileInput());
		}

		/// <summary>
		/// Stops recording our flight path.
		/// </summary>
		public void StopRecording()
		{
			//prevent someone just calling stop recording and glitching, we want to stop recording only if we were previously recording
			if (!_recording) return;

			Debug.Log("•Stopped recording");
			_recording = false;
			WriteDataToTextFile(inputData.ToArray());
		}

		//public void LoadFlight()
		//{
		//	//if folder does not exsits, or we don't have refference of it, create it or get the refference
		//	if (!Directory.Exists(_folderPath))
		//	{
		//		CreateDirectoryForRecords();
		//	}

		//	//read files in our folder, returns full paths
		//	string[] recordsList = Directory.GetFiles(_folderPath);

		//	//filter results and get only names
		//	string[] recordsListFiltered = new string[recordsList.Length]; //stored only file name here
		//	for (int i = 0; i < recordsList.Length; i++)
		//	{
		//		string[] splittedString = recordsList[i].Split('\\');
		//		recordsListFiltered[i] = splittedString[splittedString.Length - 1];
		//	}

		//	//Draw buttons to list view
		//	for (int i = 0; i < recordsListFiltered.Length; i++)
		//	{
		//		int iteratorIndex = i; //if we assign 'i' in a lambda function it will pass the refference of 'i' and not a value, we have to make a copy of that and then send as parameter
		//		GameObject tmpGo = Instantiate(_loadFlightStructure._buttonPrefab, _loadFlightStructure._content);
		//		tmpGo.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = recordsListFiltered[i];
		//		UnityEngine.UI.Button button = tmpGo.GetComponent<UnityEngine.UI.Button>();
		//		button.onClick.AddListener(() =>
		//		{
		//			_loadedFlight = _folderPath + "\\" + recordsListFiltered[iteratorIndex];
		//			Debug.Log("•Loaded: " + _loadedFlight);
		//		});
		//	}
		//}

		#endregion

		#region Folder and Files manipulation Methods

		/// <summary>
		/// Creating folder in MyDocuments.
		/// </summary>
		void CreateDirectoryForRecords()
		{
			string myDocumentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string flightRecorderFolderName = "FlightRecorder";
			string finalDirectoryString = myDocumentsLocation + "\\" + flightRecorderFolderName;
			_folderPath = finalDirectoryString;

			if (Directory.Exists(finalDirectoryString))
			{
				Debug.Log("•Folder '" + flightRecorderFolderName + "' <color=red>already exists</color> on location: '" + myDocumentsLocation + "'");
			}
			else
			{
				Directory.CreateDirectory(FolderPath);
				Debug.Log("•'" + flightRecorderFolderName + "' folder created <color=green>successfully</color> on location: '" + myDocumentsLocation + "'");
			}
		}

		/// <summary>
		/// Creating text file after recording stopped.
		/// </summary>
		/// <returns></returns>
		string CreateTextFile()
		{
			string textFileLocation = _folderPath + "\\" + GenerateFileName("");
			StreamWriter sw = File.CreateText(textFileLocation);
			sw.Flush();
			sw.Close();

			return textFileLocation;
		}

		/// <summary>
		/// Writing data to our newly created text file
		/// </summary>
		/// <param name="_data"></param>
		void WriteDataToTextFile(string[] _data)
		{
			CreateDirectoryForRecords();
			string createdTextFileLocaton = CreateTextFile();
			File.WriteAllLines(createdTextFileLocaton, _data);

			if(gameObject.activeInHierarchy) StartCoroutine(Coroutine_ShowSaveFileInput());
			_loadFlightStructure._saveFileNameButton.onClick.AddListener(() =>
			{
				if(_loadFlightStructure._fileNameInput.text.Length != 0)
				{
					string renamedFileFullPathAndName = _folderPath + "\\" + _loadFlightStructure._fileNameInput.text + ".txt";
					if (!File.Exists(renamedFileFullPathAndName))
					{
						File.Move(createdTextFileLocaton, renamedFileFullPathAndName);
					}
					else
					{
						renamedFileFullPathAndName = _folderPath + "\\" + GenerateFileName(_loadFlightStructure._fileNameInput.text) + ".txt";
						File.Move(createdTextFileLocaton, renamedFileFullPathAndName);
					}
					Debug.Log("•Saved flight renamed to: '" + _loadFlightStructure._fileNameInput.text + "'");
				}
				if (gameObject.activeInHierarchy) StartCoroutine(Coroutine_HideSaveFileInput());
			});

			Debug.Log("•Flight saved to: '" + createdTextFileLocaton + "'");
		}

		/// <summary>
		/// USed to generate custom name if current name exists or  user did not input any name for his new save file.
		/// </summary>
		/// <param name="customName"></param>
		/// <returns></returns>
		string GenerateFileName(string customName)
		{
			string dateNow = System.DateTime.Now.ToString("dd mm yyyy hh mm ss");
			string textFileName = "FlightRecord ";
			string fileExtension = ".txt";
			string textFileLocation;

			if(customName.Length == 0) textFileLocation = customName + textFileName + dateNow + fileExtension;
			else textFileLocation = customName + " " + textFileName + dateNow + fileExtension;

			return textFileLocation;
		}

		#endregion

		#region Animation transitions

		bool inputToggled = false;
		IEnumerator Coroutine_ShowSaveFileInput()
		{
			inputToggled = true;
			float timer = 0;
			Vector2 currentPosition = _loadFlightStructure._saveFileContent.anchoredPosition;
			Vector2 wantedPosition = new Vector2(currentPosition.x, currentPosition.y - 45);
			while (timer <= 1)
			{
				timer += Time.deltaTime * 10;
				_loadFlightStructure._saveFileContent.anchoredPosition = Vector2.Lerp(
						currentPosition,
						wantedPosition,
						timer
					);
				yield return null;
			}
		}

		IEnumerator Coroutine_HideSaveFileInput()
		{
			inputToggled = false;
			float timer = 0;
			Vector2 currentPosition = _loadFlightStructure._saveFileContent.anchoredPosition;
			Vector2 wantedPosition = new Vector2(currentPosition.x, currentPosition.y + 45);
			while (timer <= 1)
			{
				timer += Time.deltaTime * 10;
				_loadFlightStructure._saveFileContent.anchoredPosition = Vector2.Lerp(
						currentPosition,
						wantedPosition,
						timer
					);
				yield return null;
			}
		}

		#endregion
	}

}