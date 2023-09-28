using UnityEngine;
using DroneController.Physics;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProfileModManager : MonoBehaviour {

	[TextArea(3, 10)]
	public string description = "READ TOOLTIPS. \n Hover your mouse over variable to read more details on them.";

	[Tooltip("Drag drone from scene into this field.")]
	public DroneMovementScript dms;

	[Tooltip("CanvasUI Text element that will display profile types, such as 'begginer', 'intermediate', 'advanced'." +
	" Press AlphaNumeric keys 1,2 or 3 to change profile whilst runtime.")]
	public Text profileType;

	/// <summary>
	/// Our methods...
	/// If DroneMovementScript or ProfileType Canvas are not assigned, don't execute.
	/// </summary>
	private void Update()
	{
		if (!dms || !profileType) return;

		DrawProfileText();
	}

	/// <summary>
	/// Hardcoded.
	/// Setting CanvasUI Text for profile type.
	/// By pressing alpha number keys 1,2 or 3 switching between profiles and updating values on drone it self...
	/// </summary>
	void DrawProfileText()
	{
		if (dms._profileIndex == 0)
		{
			SetTextAndFont("Advanced", 60);
		}
		else if (dms._profileIndex == 1)
		{
			SetTextAndFont("Intermediate", 60);
		}
		else if (dms._profileIndex == 2)
		{
			SetTextAndFont("Beginner", 60);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetTextAndFont("Advanced", 60);
			dms._profileIndex = 0;
			dms.UpdateValuesFromEditor();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetTextAndFont("Intermediate", 60);
			dms._profileIndex = 1;
			dms.UpdateValuesFromEditor();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SetTextAndFont("Beginner", 60);
			dms._profileIndex = 2;
			dms.UpdateValuesFromEditor();
		}
	}

	/// <summary>
	/// Updating CanvasUI components in this method, easier like tthis, rather than writing this in each line...
	/// </summary>
	/// <param name="m_text">Wanted profile type</param>
	/// <param name="m_fontSize">Changing font when chaging text because they don't have the same letter count so it all fits nicely</param>
	void SetTextAndFont(string m_text, int m_fontSize)
	{
		profileType.fontSize = m_fontSize;
		profileType.text = m_text;
	}

}
