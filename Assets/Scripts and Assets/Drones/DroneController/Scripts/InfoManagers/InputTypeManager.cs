using UnityEngine;
using DroneController.Physics;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InputTypeManager : MonoBehaviour {

	[TextArea(3, 10)]
	public string description = "READ TOOLTIPS. \n Hover your mouse over variable to read more details on them.";

	[Tooltip("Drag drone from scene into this field.")]
	public DroneMovementScript dms;

	[Tooltip("CanvasUI Text element that will display weather we use joystick or keyboard. Pressing AlphaNumeric number 0 will change runtime from joystick to keyboard" +
		"and vice versa.")]
	public Text controller;

	/// <summary>
	/// Our methods...
	/// If ControllerType Canvas is not assigned, don't execute.
	/// </summary>
	private void Update()
	{
		if (!controller) return;

		DrawControllerText();
	}

	/// <summary>
	/// Changing between joystick and keyboard by pressing alpha numeric key '0'
	/// Updating controller CanvasUI Text with corresponding input type
	/// </summary>
	void DrawControllerText()
	{
		if (dms.customFeed)
			SetTextAndFont("CustomFeed", 60);
		else if (dms.joystick_turned_on == true && dms.customFeed == false)
			SetTextAndFont("Joystick", 60);
		else if (dms.joystick_turned_on == false && dms.customFeed == false)
			SetTextAndFont("Keyboard", 60);

		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			dms.customFeed = false;
			if (dms.joystick_turned_on)
			{
				dms.joystick_turned_on = false;
				dms.inputEditorSelection = 1;
			}
			else
			{
				dms.joystick_turned_on = true;
				dms.inputEditorSelection = 2;
			}
		}		
	}

	/// <summary>
	/// Updating CanvasUI components in this method, easier like tthis, rather than writing this in each line...
	/// </summary>
	/// <param name="m_text">Wanted profile type</param>
	/// <param name="m_fontSize">Changing font when chaging text because they don't have the same letter count so it all fits nicely</param>
	void SetTextAndFont(string m_text, int m_fontSize)
	{
		controller.fontSize = m_fontSize;
		controller.text = m_text;
	}

}
