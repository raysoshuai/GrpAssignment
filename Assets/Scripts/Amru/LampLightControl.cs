using UnityEngine;

public class LampLightControl : MonoBehaviour
{
    public Light lampLight;
    public Material surfaceMaterial;
    private bool isLampOn = false; // Boolean to track the state of the lamp

    void Awake()
    {
        // Ensure the lamp light is assigned
        if (lampLight == null)
        {
            lampLight = GetComponentInChildren<Light>();
            if (lampLight == null)
            {
                Debug.LogError("Lamp light not assigned or found in children of " + gameObject.name);
            }
        }

        // Initialize the lamp state
        SetLightAndColor(isLampOn);
    }

    // Public method to toggle the lamp based on the current state
    public void ToggleLamp()
    {
        isLampOn = !isLampOn; // Toggle the lamp state
        SetLightAndColor(isLampOn);
    }

    // Helper function to set light intensity and material color based on lamp state
    private void SetLightAndColor(bool lampOn)
    {
        float intensity = lampOn ? 0.5f : 0f;
        Color baseColor = lampOn ? Color.yellow : Color.white;

        // Set the specified intensity to the lamp light
        if (lampLight != null)
        {
            lampLight.intensity = intensity; // Set light intensity
            lampLight.color = Color.yellow; // Ensure the light color is yellow when on
        }
        else
        {
            Debug.LogError("Lamp light is not assigned or is missing in " + gameObject.name);
        }

        // Change the material base color based on the lamp state
        if (surfaceMaterial != null)
        {
            surfaceMaterial.SetColor("_BaseColor", baseColor);
        }
        else
        {
            Debug.LogError("Surface material not assigned on " + gameObject.name);
        }
    }
}
