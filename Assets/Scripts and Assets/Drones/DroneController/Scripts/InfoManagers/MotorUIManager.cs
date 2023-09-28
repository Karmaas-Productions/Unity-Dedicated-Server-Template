using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DroneController.Physics;

public class MotorUIManager : MonoBehaviour {

	[SerializeField] private DroneMovement _droneMovement;
	[SerializeField] private Vector3[] _localMotorForce;
	[SerializeField] private UnityEngine.UI.Image[] _motorImages;

	private float maxThrottleForce;
	private float proppelerJoystickPercentage;


	// Use this for initialization
	void Start () {
		_localMotorForce = new Vector3[4];
		maxThrottleForce = _droneMovement.maxThrottleForce;
		proppelerJoystickPercentage = _droneMovement.MaxPercentageOfForceDrop;
	}

	private void LateUpdate()
	{
		for(int i = 0; i < _localMotorForce.Length; i++)
		{
			_localMotorForce[i] = _droneMovement.transform.InverseTransformDirection(_droneMovement.ProppelerForces[i]);
			_motorImages[i].fillAmount = /*Mathf.Lerp(*/
										 //_motorImages[i].fillAmount,
					(_localMotorForce[i].y + Random.Range(-0.1f, 0.1f) + _droneMovement.ProppelerForceBasedOnJoystickInput[i]) / (maxThrottleForce + (proppelerJoystickPercentage / 100.0f * maxThrottleForce));
			//	Time.deltaTime * 10
			//);
			_motorImages[i].fillAmount = /*Mathf.Lerp(*/
										 //_motorImages[i].fillAmount,
			Mathf.Clamp(_motorImages[i].fillAmount, 0.025f,1.0f);
		}
	}
}
