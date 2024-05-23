using UnityEngine;

public class FlashLightControl : MonoBehaviour
{
    public Light spotLight; 
    public Material emissiveMaterial; 
    private bool isFlashOn = false; // Boolean to track the state of the flashlight

    void Awake()
    {
        // Ensure the spotlight is assigned
        if (spotLight == null)
        {
            spotLight = GetComponentInChildren<Light>();
            if (spotLight == null)
            {
                Debug.LogError("Spotlight not assigned or found in children of " + gameObject.name);
            }
        }

        // Initialize the flashlight state
        SetLightAndEmission(isFlashOn);
    }

    // Public method to toggle the flashlight based on the current state
    public void ToggleFlashlight()
    {
        isFlashOn = !isFlashOn; // Toggle the flashlight state
        SetLightAndEmission(isFlashOn);
    }

    // Helper function to set light intensity and material emission based on flashlight state
    private void SetLightAndEmission(bool emissionOn)
    {
        float intensity = emissionOn ? 1f : 0f;

        // Set the specified intensity to the spotlight
        if (spotLight != null)
        {
            spotLight.intensity = intensity; // Set light intensity
        }
        else
        {
            Debug.LogError("Spotlight is not assigned or is missing in " + gameObject.name);
        }

        // Toggle emission on or off based on the flashlight state
        if (emissiveMaterial != null)
        {
            if (emissionOn)
            {
                emissiveMaterial.EnableKeyword("_EMISSION");
            }
            else
            {
                emissiveMaterial.DisableKeyword("_EMISSION");
            }
        }
        else
        {
            Debug.LogError("Emissive material not assigned on " + gameObject.name);
        }
    }
}
