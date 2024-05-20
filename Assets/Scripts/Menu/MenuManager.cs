using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public Transform head;
    public float spawnDistance = 2;
    public GameObject menu;
    private bool isMenuVisible = false;  // Boolean to track the visibility of the menu

    void Start()
    {
        // Ensure the menu is initially hidden
        menu.SetActive(false);
    }

    // Public method to toggle the menu visibility based on the current state
    public void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;  // Toggle the visibility state
        menu.SetActive(isMenuVisible);

        if (isMenuVisible)
        {
            PositionAndRotateMenu();
        }
    }

    void PositionAndRotateMenu()
    {
        // Calculate the forward direction, flattened on the y-axis
        Vector3 forwardDirection = new Vector3(head.forward.x, 0, head.forward.z).normalized;

        // Position the menu in front of the head at the specified spawn distance
        menu.transform.position = head.position + forwardDirection * spawnDistance;

        // Adjust the height of the menu to be at the eye level
        menu.transform.position = new Vector3(menu.transform.position.x, head.position.y, menu.transform.position.z);

        // Make the menu face the player
        // Face directly opposite the head's forward direction
        menu.transform.LookAt(head);
        menu.transform.rotation = Quaternion.LookRotation(menu.transform.position - head.position);
    }
}
