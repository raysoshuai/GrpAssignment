using UnityEngine;

public class ToggleRotation : MonoBehaviour
{
    public Transform targetTransform;

    public bool isRotated = false;
    public float offRotation = 113f;
    public float onRotation = 67f;

    void Start()
    {
        // Set the initial rotation based on the initial state of isRotated
        UpdateRotation();
    }

    public void ToggleRotationState()
    {
        isRotated = !isRotated; // Invert the boolean state
        UpdateRotation(); // Update the rotation based on the new state
    }

    private void UpdateRotation()
    {
        // Set rotation based on the current state
        targetTransform.localEulerAngles = new Vector3(isRotated ? onRotation : offRotation, 0f, 0f);
    }
}
