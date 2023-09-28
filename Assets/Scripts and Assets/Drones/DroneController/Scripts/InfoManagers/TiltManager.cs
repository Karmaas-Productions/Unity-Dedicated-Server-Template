using UnityEngine;
using UnityEngine.UI;

public class TiltManager : MonoBehaviour {

	[TextArea(3, 10)]
	public string description = "READ TOOLTIPS. \n Hover your mouse over variable to read more details on them.";

	[Space(20)]
	[Tooltip("Assign the root drone object here.")]
	public Transform trackingObject;

	[Space(20)]
	[Tooltip("Assign CanvasUI Text that will display the amount of front tilt.")]
	public Text frontTilt;
	[Tooltip("Assign CanvasUI Text that will display the amount of side tilt.")]
	public Text sideTilt;

	[Space(20)]
	[Tooltip("Assign CanvasUI Image that will display the amound of front tilt.")]
	public Image frontTilt_visual;
	[Tooltip("Assign CanvasUI Image that will display the amound of side tilt.")]
	public Image sideTilt_visual;

	/// <summary>
	/// Just reading our drone basic transform information, rotation on X axis (side tilt) and Z axis (front tilt).
	/// </summary>
	private void LateUpdate()
	{
		frontTilt.text = trackingObject.eulerAngles.x.ToString("f0");
		sideTilt.text = trackingObject.eulerAngles.z.ToString("f0");

		frontTilt_visual.rectTransform.rotation = Quaternion.Euler(0, 0, -trackingObject.eulerAngles.x);
		sideTilt_visual.rectTransform.rotation = Quaternion.Euler(0, 0, trackingObject.eulerAngles.z);
	}

}
