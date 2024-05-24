using UnityEngine;

public class PointLampControl : MonoBehaviour
{
    public Light pointLight;
    public Material surfaceMaterial;
    private bool isLampOn = false; // Boolean to track the state of the lamp

    void Awake()
    {
        // Ensure the point light is assigned
        if (pointLight == null)
        {
            pointLight = GetComponentInChildren<Light>();
            if (pointLight == null)
            {
                Debug.LogError("Point light not assigned or found in children of " + gameObject.name);
            }
        }

        // Initialize the lamp state
        SetLightAndEmission(isLampOn);
    }

    // Public method to toggle the lamp based on the current state
    public void ToggleLamp()
    {
        isLampOn = !isLampOn; // Toggle the lamp state
        SetLightAndEmission(isLampOn);
    }

    // Helper function to set light intensity and material emission based on lamp state
    private void SetLightAndEmission(bool lampOn)
    {
        float intensity = lampOn ? 1f : 0f;

        // Set the specified intensity to the point light
        if (pointLight != null)
        {
            pointLight.intensity = intensity; // Set light intensity
        }
        else
        {
            Debug.LogError("Point light is not assigned or is missing in " + gameObject.name);
        }

        // Toggle the emission on the material based on the lamp state
        if (surfaceMaterial != null)
        {
            if (lampOn)
            {
                surfaceMaterial.EnableKeyword("_EMISSION");
                surfaceMaterial.SetColor("_EmissionColor", Color.yellow);
            }
            else
            {
                surfaceMaterial.DisableKeyword("_EMISSION");
            }
        }
        else
        {
            Debug.LogError("Surface material not assigned on " + gameObject.name);
        }
    }
}
