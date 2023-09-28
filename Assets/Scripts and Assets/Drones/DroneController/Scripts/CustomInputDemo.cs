using UnityEngine;

public class CustomInputDemo : MonoBehaviour
{

	public DroneMovement DroneMovement;

	[Space(5)]
	public float Pitch;
	public float Roll;
	public float Yaw;
	public float Throttle;

	void Update()
	{
		Throttle = Input.GetAxis("Left_Y");
		Yaw = -Input.GetAxis("Left_X");
		Pitch = -Input.GetAxis("Right_Y");
		Roll = Input.GetAxis("Right_X");

		DroneMovement.CustomFeed_throttle = Throttle;
		DroneMovement.CustomFeed_yaw = Yaw;
		DroneMovement.CustomFeed_pitch = Pitch;
		DroneMovement.CustomFeed_roll = Roll;
	}

}
