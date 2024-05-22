using UnityEngine;

public class RotationToggleSwitch : MonoBehaviour
{
    [Tooltip("Rotation angle when toggle is on")]
    public float onRotationY = -21.28f;

    [Tooltip("Rotation angle when toggle is off")]
    public float offRotationY = 0.0f;

    private bool isToggleOn = false;

    // Method to toggle the rotation
    public void ToggleRotation()
    {
        isToggleOn = !isToggleOn;

        if (isToggleOn)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, onRotationY, transform.rotation.eulerAngles.z);
            Debug.Log("Toggled to On Rotation: " + onRotationY);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, offRotationY, transform.rotation.eulerAngles.z);
            Debug.Log("Toggled to Off Rotation: " + offRotationY);
        }
    }
}
