using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularRotationManager : MonoBehaviour {

	[SerializeField] private Rigidbody _drone;
	[SerializeField] private Vector3 _localAngularRotation;
	[SerializeField] private UnityEngine.UI.Text _text;

	private void FixedUpdate()
	{
		_localAngularRotation = _drone.transform.InverseTransformDirection(_drone.angularVelocity * Mathf.Rad2Deg);
		_text.text = _localAngularRotation.ToString("F0");
	}

}
