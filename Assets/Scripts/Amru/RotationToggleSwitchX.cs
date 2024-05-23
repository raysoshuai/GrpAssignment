using UnityEngine;

public class ToggleRotation : MonoBehaviour
{
    public Transform targetTransform; 

    public bool isRotated = false;
    public float offRotation = 113f;
    public float onRotation = 67f;

    public void ToggleRotationState() 
    {
        isRotated = !isRotated; // Invert the boolean state

        // Set rotation based on the toggled state
        targetTransform.localEulerAngles = new Vector3(isRotated ? onRotation : offRotation, 0f, 0f);
    }
}
