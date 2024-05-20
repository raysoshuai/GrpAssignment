using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSwitch : MonoBehaviour
{
    [SerializeField] private GameObject fanSwitch;
    [SerializeField] private GameObject fanBlades;
    [SerializeField] private float maxSpeed = 1000f;
    [SerializeField] private float acceleration = 150f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private bool isFanOn = false;
    private float finalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFanOn && currentSpeed < maxSpeed)
            currentSpeed += acceleration * Time.deltaTime;
        else if (!isFanOn && currentSpeed >= 0)
            currentSpeed -= acceleration * Time.deltaTime;

        if (currentSpeed > 0)
        {
            finalSpeed += currentSpeed * Time.deltaTime;

            fanBlades.transform.localRotation = Quaternion.Euler(new Vector3(0, finalSpeed, 0));
        }
    }

    public void ToggleFan()
    {
        if (isFanOn)
        {
            fanSwitch.transform.rotation = Quaternion.Euler(0, 0, 180f);
            isFanOn = false;
        }
        else
        {
            fanSwitch.transform.rotation = Quaternion.Euler(0, 0, 0);
            isFanOn = true;
        }
    }
}
