using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour {

	[Tooltip("Drago drone into this field.")]
	public Rigidbody trackingObject;

	[Tooltip("Speed in km/s. CanvasUI Text that will display drone velocity.")]
	public Text droneVelocity;

	/// <summary>
	/// Just reading drone rigidbody component velocity, and turning m/s to km/h.
	/// </summary>
	void LateUpdate()
	{
		droneVelocity.text = (trackingObject.velocity.magnitude * 3.6f).ToString("f2");
	}

}
