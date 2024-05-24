using UnityEngine;

public class FanSwitch : MonoBehaviour
{
    public GameObject fanBlades;
    public float maxSpeed = 1000f;
    public float acceleration = 150f;
    public float currentSpeed = 0f;
    public bool isFanOn = false;
    private float finalSpeed;

    private AudioSource fanAudioSource;

    void Start()
    {
        fanAudioSource = fanBlades.GetComponent<AudioSource>();
        if (fanAudioSource == null)
        {
            Debug.LogError("AudioSource component missing from fanBlades.");
        }
    }

    void Update()
    {
        if (isFanOn && currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (!isFanOn && currentSpeed > 0)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }

        if (currentSpeed > 0)
        {
            finalSpeed += currentSpeed * Time.deltaTime;
            fanBlades.transform.localRotation = Quaternion.Euler(new Vector3(0, finalSpeed, 0));

            if (fanAudioSource != null)
            {
                fanAudioSource.enabled = true;
                fanAudioSource.volume = currentSpeed / maxSpeed;
            }
        }
        else
        {
            if (fanAudioSource != null)
            {
                fanAudioSource.enabled = false;
            }
        }
    }

    public void ToggleFan()
    {
        isFanOn = !isFanOn;
    }
}
