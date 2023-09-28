using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using DroneController.Physics;

[CustomEditor(typeof(DroneMovement))]
[ExecuteInEditMode]
public class DroneMovementEditor : Editor {

	bool demo;

	public override void OnInspectorGUI()
    {
		demo = false;
		DrawDefaultInspector();
        var myScript = target as DroneMovementScript;

		if (myScript.FlightRecorderOverride) GUI.backgroundColor = Color.red;

        EditorGUILayout.HelpBox("Hover NAME OF VARIABLES for properties to find out more about them. If you're not sure what they are used for feel free to contact me via e-mail or watch the youtube tutorials i prepared first.", MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawGameObjectPocketsAndVelocityReadings(myScript);

		EditorGUILayout.Space();

		DrawProepelerPockets(myScript);

		EditorGUILayout.Space();

		DrawCenterOfMassPockets(myScript);

		EditorGUILayout.Space();

		DrawCustomProfilesWindow(myScript);

        EditorGUILayout.Space();

        DrawCustomInputTypeTabbedStyle(myScript);
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

    void DrawGameObjectPocketsAndVelocityReadings(DroneMovementScript myScript)
    {
        myScript.droneObject = (Transform)EditorGUILayout.ObjectField(new GUIContent("Drone Object", "Object part that is used to tilt. Ff you're not sure what to put here, check out already made prefabs or checkout my youtube tutorials for Drone Controller (located in the documentation)"), myScript.droneObject, typeof(Transform), true);
        GUI.enabled = false;
		EditorGUILayout.FloatField(new GUIContent("Velocity", "Getting our drone velocity."), myScript.velocity);
		EditorGUILayout.FloatField(new GUIContent("Angular Velocity", "Getting our drone angular velocity."), myScript.angularVelocity);
		GUI.enabled = true;
    }

	void DrawProepelerPockets(DroneMovementScript myScript)
	{
		if (myScript.proppelers.Length == 0) myScript.proppelers = new Transform[4];
		myScript.proppelers[0] = (Transform)EditorGUILayout.ObjectField(new GUIContent("Propeler[0]", "todo..."), myScript.proppelers[0], typeof(Transform), true);
		myScript.proppelers[1] = (Transform)EditorGUILayout.ObjectField(new GUIContent("Propeler[1]", "todo..."), myScript.proppelers[1], typeof(Transform), true);
		myScript.proppelers[2] = (Transform)EditorGUILayout.ObjectField(new GUIContent("Propeler[2]", "todo..."), myScript.proppelers[2], typeof(Transform), true);
		myScript.proppelers[3] = (Transform)EditorGUILayout.ObjectField(new GUIContent("Propeler[3]", "todo..."), myScript.proppelers[3], typeof(Transform), true);
	}

	void DrawCenterOfMassPockets(DroneMovementScript myScript)
	{
		myScript.centerOfMass = EditorGUILayout.Vector3Field(new GUIContent("Center of Mass", "todo..."), myScript.centerOfMass);
		myScript.centerOfMassRadiusGizmo = EditorGUILayout.Slider(new GUIContent("Gizmo radius", "todo..."), myScript.centerOfMassRadiusGizmo, 0.0f, 1.0f);
		myScript.centerOfMassGizmoColor = EditorGUILayout.ColorField(new GUIContent("Gizmo color", "todo..."), myScript.centerOfMassGizmoColor);
	}

	void DrawCustomProfilesWindow(DroneMovementScript myScript)
    {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("Use this tab bar to customize flying settings. \n Edit to your own preferences. \n Selected one is applied to the drone and will drive with those settings.", EditorStyles.helpBox);

		if (demo)
		{
			GUI.enabled = false;
			myScript._profileIndex = 1;
		}
		else
		{
			GUI.enabled = true;
		}

		myScript._profileIndex = GUILayout.Toolbar(myScript._profileIndex, new string[] { "Advanced", "Intermediate", "Begginer" });
		switch (myScript._profileIndex)
        {
            default:
				EditorGUILayout.LabelField("Velocity Limiter", EditorStyles.toolbarButton);
				EditorGUILayout.Space();
				myScript.profiles[myScript._profileIndex].maxSpeed = EditorGUILayout.FloatField(new GUIContent("Max Speed (km/h)", "todo..."), myScript.profiles[myScript._profileIndex].maxSpeed);

				EditorGUILayout.Space();
				EditorGUILayout.Space();

				myScript.profiles[myScript._profileIndex].minDrag = EditorGUILayout.Slider(new GUIContent("Minimum Drag Value", "todo..."), myScript.profiles[myScript._profileIndex].minDrag, 0.0f, 0.05f);
				myScript.profiles[myScript._profileIndex].maxDrag = EditorGUILayout.Slider(new GUIContent("Maximum Drag Value", "todo..."), myScript.profiles[myScript._profileIndex].maxDrag, 0.05f, 4.0f);
				myScript.profiles[myScript._profileIndex].speedValueCurve = EditorGUILayout.CurveField(new GUIContent("Drag/Speed Value Curve", "todo..."), myScript.profiles[myScript._profileIndex].speedValueCurve);
				EditorGUILayout.Space();



				EditorGUILayout.LabelField("Angulr Drag Settings", EditorStyles.toolbarButton);
				EditorGUILayout.Space();
				myScript.profiles[myScript._profileIndex].maxAngularDrag = EditorGUILayout.FloatField(new GUIContent("Max Angular Drag", "todo..."), myScript.profiles[myScript._profileIndex].maxAngularDrag);
				myScript.profiles[myScript._profileIndex].minAngularDrag = EditorGUILayout.FloatField(new GUIContent("Min Angular Drag", "todo..."), myScript.profiles[myScript._profileIndex].minAngularDrag);
				myScript.profiles[myScript._profileIndex].angularDragZeroingTime = EditorGUILayout.FloatField(new GUIContent("Zeroing Time", "todo..."), myScript.profiles[myScript._profileIndex].angularDragZeroingTime);

				EditorGUILayout.Space();
				EditorGUILayout.Space();
				


				EditorGUILayout.LabelField("Feature Speeds", EditorStyles.toolbarButton);
				EditorGUILayout.Space();

				if (!demo) GUI.enabled = false;
				EditorGUILayout.Slider(new GUIContent("Throttle", "todo..."), myScript.currentThrottle, 0.0f, 1.0f);
				if (!demo) GUI.enabled = true;
				myScript.profiles[myScript._profileIndex].inputThrottleCurve = EditorGUILayout.CurveField(new GUIContent("Throttle Curve", "todo..."), myScript.profiles[myScript._profileIndex].inputThrottleCurve);
				if (!demo) GUI.enabled = false;
				EditorGUILayout.Slider(new GUIContent("Pitch", "todo..."), myScript.currentPitchThrottle, -1.0f, 1.0f);
				if (!demo) GUI.enabled = true;
				myScript.profiles[myScript._profileIndex].inputPitchThrottleCurve = EditorGUILayout.CurveField(new GUIContent("Pitch Curve", "todo..."), myScript.profiles[myScript._profileIndex].inputPitchThrottleCurve);
				if (!demo) GUI.enabled = false;
				EditorGUILayout.Slider(new GUIContent("Roll", "todo..."), myScript.currentRollThrottle, -1.0f, 1.0f);
				if (!demo) GUI.enabled = true;
				myScript.profiles[myScript._profileIndex].inputRollThrottleCurve = EditorGUILayout.CurveField(new GUIContent("Roll Curve", "todo..."), myScript.profiles[myScript._profileIndex].inputRollThrottleCurve);
				if (!demo) GUI.enabled = false;
				EditorGUILayout.Slider(new GUIContent("Yaw", "todo..."), myScript.currentYawThrottle, -1.0f, 1.0f);
				if (!demo) GUI.enabled = true;
				myScript.profiles[myScript._profileIndex].inputYawThrottleCurve = EditorGUILayout.CurveField(new GUIContent("Yaw Curve", "todo..."), myScript.profiles[myScript._profileIndex].inputYawThrottleCurve);

				EditorGUILayout.Space();
				EditorGUILayout.Space();

				myScript.profiles[myScript._profileIndex].maxThrottleForce = EditorGUILayout.FloatField(new GUIContent("Max Throttle Force", "todo..."), myScript.profiles[myScript._profileIndex].maxThrottleForce);
				myScript.profiles[myScript._profileIndex].maxPitchForce = EditorGUILayout.FloatField(new GUIContent("Max Pitch Force", "todo......"), myScript.profiles[myScript._profileIndex].maxPitchForce);
				myScript.profiles[myScript._profileIndex].maxRollForce = EditorGUILayout.FloatField(new GUIContent("Max Roll Force", "todo......"), myScript.profiles[myScript._profileIndex].maxRollForce);
				myScript.profiles[myScript._profileIndex].maxRotateForce = EditorGUILayout.FloatField(new GUIContent("Max Yaw Force", "todo......"), myScript.profiles[myScript._profileIndex].maxRotateForce);

				EditorGUILayout.Space();

				myScript.profiles[myScript._profileIndex].rotationSlowDown = EditorGUILayout.Slider(new GUIContent("Rotation Slowdown", "todo......"), myScript.profiles[myScript._profileIndex].rotationSlowDown, 0.0f, 15.0f);


				EditorGUILayout.Space();
				EditorGUILayout.BeginVertical("Box");
				myScript.profiles[myScript._profileIndex].angleLocked = EditorGUILayout.Toggle(new GUIContent("Angle Lock", "todo......"), myScript.profiles[myScript._profileIndex].angleLocked);
				myScript.profiles[myScript._profileIndex].angleLimit = EditorGUILayout.FloatField(new GUIContent("Max Angle Tilt", "todo......"), myScript.profiles[myScript._profileIndex].angleLimit);
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();

				EditorGUILayout.LabelField("Drone Sound Amplifier", EditorStyles.toolbarButton);
				EditorGUILayout.BeginVertical("Box");

				EditorGUILayout.HelpBox("Main brush sound, pitch and volume amplifiers", MessageType.Info);
				myScript.profiles[myScript._profileIndex].dronePitchAmplifier = EditorGUILayout.Slider(new GUIContent("Drone Pitch Change", "tooltiptodo..."), myScript.profiles[myScript._profileIndex].dronePitchAmplifier, 0.0f, 2.0f);
				myScript.profiles[myScript._profileIndex].soundMultiplier = EditorGUILayout.Slider(new GUIContent("Drone Movement Sound Change", "tooltiptodo..."), myScript.profiles[myScript._profileIndex].soundMultiplier, 0.0f, 2.0f);

				EditorGUILayout.HelpBox("Angular sound, volume amplifier", MessageType.Info);
				myScript.profiles[myScript._profileIndex].angularSoundMultiplier = EditorGUILayout.Slider(new GUIContent("Drone Angular Volume Change", "tooltiptodo..."), myScript.profiles[myScript._profileIndex].angularSoundMultiplier, 0.0f, 2.0f);

				EditorGUILayout.HelpBox("Static sound, volume amplifier", MessageType.Info);
				myScript.profiles[myScript._profileIndex].staticSoundMultiplier = EditorGUILayout.Slider(new GUIContent("Static Volume Change", "tooltiptodo..."), myScript.profiles[myScript._profileIndex].staticSoundMultiplier, 0.0f, 2.0f);
				myScript.profiles[myScript._profileIndex].staticSoundStartPos = EditorGUILayout.Slider(new GUIContent("Static Staring Volume", "tooltiptodo..."), myScript.profiles[myScript._profileIndex].staticSoundStartPos, 0.0f, 2.0f);

				EditorGUILayout.EndVertical();


				//applying presets
				myScript.UpdateValuesFromEditor();
				//myScript.maxSpeed = myScript.profiles[myScript._profileIndex].maxSpeed;

				//myScript.maxThrottleForce = myScript.profiles[myScript._profileIndex].maxThrottleForce;
				//myScript.maxRollForce = myScript.profiles[myScript._profileIndex].maxRollForce;
				//myScript.maxPitchForce = myScript.profiles[myScript._profileIndex].maxPitchForce;
				//myScript.maxRotateForce = myScript.profiles[myScript._profileIndex].maxRotateForce;

				//myScript.angleLocked = myScript.profiles[myScript._profileIndex].angleLocked;
				//myScript.angleLimit = myScript.profiles[myScript._profileIndex].angleLimit;

				//myScript.dronePitchAmplifier = myScript.profiles[myScript._profileIndex].dronePitchAmplifier;
				//myScript.soundMultiplier = myScript.profiles[myScript._profileIndex].soundMultiplier;
				//myScript.angularSoundMultiplier = myScript.profiles[myScript._profileIndex].angularSoundMultiplier;
				//myScript.staticSoundMultiplier = myScript.profiles[myScript._profileIndex].staticSoundMultiplier;
				//myScript.staticSoundStartPos = myScript.profiles[myScript._profileIndex].staticSoundStartPos;

				break;
        }
		EditorGUILayout.EndVertical();
 		if(demo) GUI.enabled = true;
   }

	void DrawCustomInputTypeTabbedStyle(DroneMovementScript myScript)
	{
		//dodaj info tekst koji opisuje svaki%%%%%%%%%%%%%%%%%%%%%%%%%%%
		EditorGUILayout.BeginVertical("Box");

		if (demo)
		{
			GUI.enabled = false;
			myScript.inputEditorSelection = 0;
		}

		myScript.inputEditorSelection = GUILayout.Toolbar(myScript.inputEditorSelection, new string[] { "Custom", "Keyboard", "Joystick" });
		
		switch (myScript.inputEditorSelection)
        {
			case 0:
				myScript.customFeed = true;
				myScript.joystick_turned_on = false;

				EditorGUILayout.Space();

				EditorGUILayout.HelpBox("Hover on variable names to see their original name for custom code access.\nUse this if you want to feed your own movement values.\nIf you're not sure how to access these values, check the tutorial. Link from tutorial playlist is in the doc files.", MessageType.Info);

				EditorGUILayout.Space();
				myScript.CustomFeed_pitch = EditorGUILayout.Slider(new GUIContent("Pitch", "todo..."), myScript.CustomFeed_pitch, -1.0f, 1.0f);
				myScript.CustomFeed_roll = EditorGUILayout.Slider(new GUIContent("Roll", "todo..."), myScript.CustomFeed_roll, -1.0f, 1.0f);
				myScript.CustomFeed_yaw = EditorGUILayout.Slider(new GUIContent("Yaw", "todo..."), myScript.CustomFeed_yaw, -1.0f, 1.0f);
				myScript.CustomFeed_throttle = EditorGUILayout.Slider(new GUIContent("Throttle", "todo..."), myScript.CustomFeed_throttle, 0f, 1.0f);
				EditorGUILayout.Space();

				break;
			case 1:
                myScript.joystick_turned_on = false;
				myScript.customFeed = false;

                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Setup wanted keyboard input to move your drone.", MessageType.Info);
                EditorGUILayout.Space();

                myScript.forward = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Forward", "Key press to move forward"), myScript.forward);
                myScript.backward = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Backward", "Key press to move backward"), myScript.backward);
                myScript.rightward = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Rightward", "Key press to move right (not rotating)"), myScript.rightward);
                myScript.leftward = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Leftward", "Key press to move left (not rotating)"), myScript.leftward);
                myScript.upward = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Upward", "Key press to 'Wingardium Leviosa' your drone"), myScript.upward);
                myScript.downward = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Downward", "Key press to move your drone down"), myScript.downward);
                myScript.rotateRightward = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Rotate Rightward", "Key press to rotate your drone right"), myScript.rotateRightward);
                myScript.rotateLeftward = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Rotate Leftward", "Key press to rotate your drone left"), myScript.rotateLeftward);

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                break;
            case 2:
                myScript.joystick_turned_on = true;
				myScript.customFeed = false;

                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Setup wanted joystick control input to move your drone.", MessageType.Info);
                EditorGUILayout.Space();

				myScript.HandedInput = GUILayout.Toolbar(myScript.HandedInput, new string[] { "Left handed", "Right handed" });
				if (myScript.HandedInput == 0) myScript.LeftHanded = true; else myScript.LeftHanded = false;

				switch (myScript.LeftHanded)
				{
					case true:

						EditorGUILayout.BeginHorizontal();
						myScript.left_analog_x = EditorGUILayout.TextField(new GUIContent("Left Analog X Axis", "This is usually movement left/right (not rotating)"), myScript.left_analog_x);
						myScript.left_analog_x_movement = (JoystickDrivingAxis)EditorGUILayout.EnumPopup(new GUIContent("", "tooltip"), JoystickDrivingAxis.roll);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						myScript.left_analog_y = EditorGUILayout.TextField(new GUIContent("Left Analog Y Axis", "This is usually movment forward/backward (not going up/down)"), myScript.left_analog_y);
						myScript.left_analog_y_movement = (JoystickDrivingAxis)EditorGUILayout.EnumPopup(new GUIContent("", "tooltip"), JoystickDrivingAxis.pitch);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						myScript.right_analog_x = EditorGUILayout.TextField(new GUIContent("Right Analog X Axis", "This is usually rotating left/right"), myScript.right_analog_x);
						myScript.right_analog_x_movement = (JoystickDrivingAxis)EditorGUILayout.EnumPopup(new GUIContent("", "tooltip"), JoystickDrivingAxis.yaw);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						myScript.right_analog_y = EditorGUILayout.TextField(new GUIContent("Right Analog Y Axis", "This is usually hovering up/down"), myScript.right_analog_y);
						myScript.right_analog_y_movement = (JoystickDrivingAxis)EditorGUILayout.EnumPopup(new GUIContent("", "tooltip"), JoystickDrivingAxis.throttle);
						EditorGUILayout.EndHorizontal();

						break;

					case false:

						EditorGUILayout.BeginHorizontal();
						myScript.left_analog_x = EditorGUILayout.TextField(new GUIContent("Left Analog X Axis", "This is usually movement left/right (not rotating)"), myScript.left_analog_x);
						myScript.left_analog_x_movement = (JoystickDrivingAxis)EditorGUILayout.EnumPopup(new GUIContent("", "tooltip"), JoystickDrivingAxis.yaw);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						myScript.left_analog_y = EditorGUILayout.TextField(new GUIContent("Left Analog Y Axis", "This is usually movment forward/backward (not going up/down)"), myScript.left_analog_y);
						myScript.left_analog_y_movement = (JoystickDrivingAxis)EditorGUILayout.EnumPopup(new GUIContent("", "tooltip"), JoystickDrivingAxis.throttle);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						myScript.right_analog_x = EditorGUILayout.TextField(new GUIContent("Right Analog X Axis", "This is usually rotating left/right"), myScript.right_analog_x);
						myScript.right_analog_x_movement = (JoystickDrivingAxis)EditorGUILayout.EnumPopup(new GUIContent("", "tooltip"), JoystickDrivingAxis.roll);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						myScript.right_analog_y = EditorGUILayout.TextField(new GUIContent("Right Analog Y Axis", "This is usually hovering up/down"), myScript.right_analog_y);
						myScript.right_analog_y_movement = (JoystickDrivingAxis)EditorGUILayout.EnumPopup(new GUIContent("", "tooltip"), JoystickDrivingAxis.pitch);
						EditorGUILayout.EndHorizontal();

						break;
				}
             
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                break;
        }

		EditorGUILayout.EndVertical();
		if(demo) GUI.enabled = true;
	}
}
