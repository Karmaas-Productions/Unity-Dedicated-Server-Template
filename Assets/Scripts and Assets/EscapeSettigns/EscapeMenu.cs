using UnityEngine;

public class EscapeMenuController : MonoBehaviour
{
    public GameObject escapeMenu;

    private bool isEscapeMenuActive;
    private bool wasCursorVisible;

    private void Start()
    {
        // Initially, the escape menu is inactive
        isEscapeMenuActive = false;
        escapeMenu.SetActive(false);
    }

    private void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the escape menu on or off
            isEscapeMenuActive = !isEscapeMenuActive;

            // Set the active state of the escape menu GameObject based on the toggle
            escapeMenu.SetActive(isEscapeMenuActive);
        }
    }
}
