using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReticleScaler : MonoBehaviour
{
    public XRInteractorLineVisual lineVisual;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float speed = 2.0f;

    private Transform reticleTransform;

    void Start()
    {
        if (lineVisual != null && lineVisual.reticle != null)
        {
            reticleTransform = lineVisual.reticle.transform;
        }
        else
        {
            Debug.LogError("Line Visual or Reticle is not assigned.");
        }
    }

    void Update()
    {
        if (reticleTransform != null)
        {
            float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * speed, 1));
            reticleTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
