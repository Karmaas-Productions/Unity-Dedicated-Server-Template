using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DroneController.Physics;
using UnityEngine.UI;

public class JoystickMovementDisplayManager : MonoBehaviour {

	[Tooltip("Drone from the scene, drag into this property")]
	[SerializeField] public DroneMovementScript dms;

	[Space(5)]
	[SerializeField] public Vector2 Raw_Input_Left_Analog;
	[SerializeField] public Vector2 Raw_Input_Right_Analog;

	[Space(5)]
	[SerializeField] public RectTransform Left_Analog_Background;
	[SerializeField] public RectTransform Right_Analog_Background;

	[Space(5)]
	[SerializeField] public RectTransform Left_Analog_Dot;
	[SerializeField] public RectTransform Right_Analog_Dot;

	[Space(5)]
	[SerializeField] public GameObject Joystick_Axis_Info_left;
	[SerializeField] public GameObject Joystick_Axis_Info_right;

	private void LateUpdate()
	{
		if (dms.LeftHanded)
		{
			Joystick_Axis_Info_left.SetActive(true);
			Joystick_Axis_Info_right.SetActive(false);

			Raw_Input_Left_Analog.x = dms.CustomFeed_roll;
			Raw_Input_Left_Analog.y = dms.CustomFeed_pitch;
			Raw_Input_Right_Analog.x = dms.CustomFeed_yaw;
			Raw_Input_Right_Analog.y = dms.CustomFeed_throttle;
		}
		else
		{
			Joystick_Axis_Info_right.SetActive(true);
			Joystick_Axis_Info_left.SetActive(false);

			Raw_Input_Left_Analog.x = dms.CustomFeed_yaw;
			Raw_Input_Left_Analog.y = dms.CustomFeed_throttle;
			Raw_Input_Right_Analog.x = dms.CustomFeed_roll;
			Raw_Input_Right_Analog.y = dms.CustomFeed_pitch;
		}
		

		Left_Analog_Dot.anchoredPosition = new Vector2(
			Raw_Input_Left_Analog.x * Left_Analog_Background.rect.width / 2,
			Raw_Input_Left_Analog.y * Left_Analog_Background.rect.height / 2
			);

		Right_Analog_Dot.anchoredPosition = new Vector2(
			Raw_Input_Right_Analog.x * Right_Analog_Background.rect.width / 2,
			Raw_Input_Right_Analog.y * Right_Analog_Background.rect.height / 2
			);
	}

}
