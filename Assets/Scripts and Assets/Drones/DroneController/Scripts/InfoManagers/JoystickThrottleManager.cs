using UnityEngine;
using UnityEngine.UI;
using DroneController.Physics;

public class JoystickThrottleManager : MonoBehaviour {

	[TextArea(3, 10)]
	public string description = "READ TOOLTIPS. \n Hover your mouse over variable to read more details on them.";

	[Space(20)]
	[Tooltip("Drone from the scene, drag into this property")]
	public DroneMovementScript dms;

	[Space(20)]
	[Tooltip("Image that will represent motor throttle current/max.")]
	public Image motorThrottleImage;
	[Tooltip("Image that will represent joystick Y axis input. from 0 up to 1")]
	public Image joystickThrottleImage;

	[Space(20)]
	[Tooltip("Image that will represent motor pitch current/max.")]
	public Image motorPitchImage_positive;
	[Tooltip("Image that will represent motor pitch current/max.")]
	public Image motorPitchImage_negative;
	[Tooltip("Image that will represent joystick Y axis input. from -1 up to 1")]
	public Image joystickPitchImage_positive;
	[Tooltip("Image that will represent joystick Y axis input. from -1 up to 1")]
	public Image joystickPitchImage_negative;

	[Space(20)]
	[Tooltip("Image that will represent motor roll current/max.")]
	public Image motorRollImage_positive;
	[Tooltip("Image that will represent motor roll current/max.")]
	public Image motorRollImage_negative;
	[Tooltip("Image that will represent joystick X axis input. from -1 up to 1")]
	public Image joystickRollImage_positive;
	[Tooltip("Image that will represent joystick X axis input. from -1 up to 1")]
	public Image joystickRollImage_negative;

	[Space(20)]
	[Tooltip("Image that will represent motor roll current/max.")]
	public Image motorRotateImage_positive;
	[Tooltip("Image that will represent motor roll current/max.")]
	public Image motorRotateImage_negative;
	[Tooltip("Image that will represent joystick X axis input. from -1 up to 1")]
	public Image joystickRotateImage_positive;
	[Tooltip("Image that will represent joystick X axis input. from -1 up to 1")]
	public Image joystickRotateImage_negative;

	[Space(20)]
	[Tooltip("Max height of the CanvasUI Image that will scale, on my set up its the same as the canvas image i made in the editor.")]
	public float maxHeight;

	/// <summary>
	/// Reading throttle joystick axis (which one you assigned to be the trottle) and scaling that CanvasUI image to represent amount from 0 up to 1.
	/// Reading current throttle from the code, and showing throttle thats beeing applied.
	/// </summary>
	void Update()
	{
		joystickThrottleImage.rectTransform.sizeDelta = new Vector2(joystickThrottleImage.rectTransform.sizeDelta.x, dms.Vertical_I * maxHeight);
		motorThrottleImage.rectTransform.sizeDelta = new Vector2(joystickThrottleImage.rectTransform.sizeDelta.x, dms.currentThrottle * maxHeight);

		joystickPitchImage_positive.rectTransform.sizeDelta = new Vector2(joystickPitchImage_positive.rectTransform.sizeDelta.x, dms.Vertical_W * maxHeight / 2);
		motorPitchImage_positive.rectTransform.sizeDelta = new Vector2(motorPitchImage_positive.rectTransform.sizeDelta.x, dms.currentPitchThrottle * maxHeight / 2);
		joystickPitchImage_negative.rectTransform.sizeDelta = new Vector2(joystickPitchImage_negative.rectTransform.sizeDelta.x, -dms.Vertical_W * maxHeight / 2);
		motorPitchImage_negative.rectTransform.sizeDelta = new Vector2(motorPitchImage_negative.rectTransform.sizeDelta.x, -dms.currentPitchThrottle * maxHeight / 2);

		joystickRollImage_positive.rectTransform.sizeDelta = new Vector2(joystickRollImage_positive.rectTransform.sizeDelta.x, dms.Horizontal_D * maxHeight / 2);
		motorRollImage_positive.rectTransform.sizeDelta = new Vector2(motorRollImage_positive.rectTransform.sizeDelta.x, dms.currentRollThrottle * maxHeight / 2);
		joystickRollImage_negative.rectTransform.sizeDelta = new Vector2(joystickRollImage_negative.rectTransform.sizeDelta.x, -dms.Horizontal_D * maxHeight / 2);
		motorRollImage_negative.rectTransform.sizeDelta = new Vector2(motorRollImage_negative.rectTransform.sizeDelta.x, -dms.currentRollThrottle * maxHeight / 2);

		joystickRotateImage_positive.rectTransform.sizeDelta = new Vector2(joystickRotateImage_positive.rectTransform.sizeDelta.x, dms.Horizontal_L * maxHeight / 2);
		motorRotateImage_positive.rectTransform.sizeDelta = new Vector2(motorRotateImage_positive.rectTransform.sizeDelta.x, dms.currentYawThrottle * maxHeight / 2);
		joystickRotateImage_negative.rectTransform.sizeDelta = new Vector2(joystickRotateImage_negative.rectTransform.sizeDelta.x, -dms.Horizontal_L * maxHeight / 2);
		motorRotateImage_negative.rectTransform.sizeDelta = new Vector2(motorRotateImage_negative.rectTransform.sizeDelta.x, -dms.currentYawThrottle * maxHeight / 2);
	}

}
