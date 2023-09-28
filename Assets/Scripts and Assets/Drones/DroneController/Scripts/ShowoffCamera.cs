using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowoffCamera : MonoBehaviour {

	[SerializeField] private Transform _transformToShow;
	[SerializeField] private Vector3 _positionInfront;
	[SerializeField] private float _movementSpeed;

	float Pitch;
	float Roll;
	float Yaw;
	float Throttle;

	private void Start()
	{		
		StartCoroutine(MoveCameraAround());
	}
	
	void Update()
	{
		transform.LookAt(_transformToShow);

		Throttle = Input.GetAxis("Left_Y");
		Yaw = -Input.GetAxis("Left_X");
		Pitch = -Input.GetAxis("Right_Y");
		Roll = Input.GetAxis("Right_X");

		if(Throttle != 0 || Yaw != 0 || Pitch != 0 || Roll != 0)
		{
			Destroy(gameObject);
		}
	}

	IEnumerator MoveCameraAround()
	{
		Vector3 camPos = _transformToShow.forward * _positionInfront.z + _transformToShow.up * _positionInfront.y + _transformToShow.right * _positionInfront.x;
		transform.position = camPos;

		yield return new WaitForSeconds(0.25f);

		Vector3 currentPos = transform.position;
		Vector3 wantedPos = currentPos + (_transformToShow.right * -_positionInfront.x*2);

		float timer = 0;
		while (timer <= 1)
		{
			timer += Time.deltaTime * _movementSpeed;
			yield return null;
			transform.position = Vector3.Lerp(currentPos, wantedPos, timer);
		}

		StartCoroutine(ZoomCameraInside());
	}

	IEnumerator ZoomCameraInside()
	{
		Vector3 currentPos = transform.position;
		Vector3 wantedPos = _transformToShow.position;

		float timer = 0;
		while (timer <= 0.9f)
		{
			timer += Time.deltaTime * _movementSpeed * 3;
			transform.position = Vector3.Lerp(currentPos, wantedPos, timer);
			yield return null;
		}

		Destroy(gameObject);
	}

}
