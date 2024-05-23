using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class NeedleRotation : MonoBehaviour
{
    public GameObject needle;
    public AudioClip sfxClip; 
    private AudioSource audioSource;
    private Vector3 initialRotation = new Vector3(0, 297.899994f, 0);
    private Vector3 enterRotation = new Vector3(0, 297.899994f, 197.499985f);
    public float rotationSpeed = 394.99997f; // Speed of rotation in degrees per second

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(RotateNeedleToTargetWithSFX(enterRotation));
        }
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(RotateNeedleToTarget(initialRotation));
        }
    }

    private IEnumerator RotateNeedleToTarget(Vector3 targetRotation)
    {
        Vector3 currentRotation = needle.transform.eulerAngles;
        float angleDifference = Mathf.Abs(targetRotation.z - currentRotation.z);

        while (angleDifference > 0.01f)
        {
            float step = rotationSpeed * Time.deltaTime;
            currentRotation = Vector3.MoveTowards(currentRotation, targetRotation, step);
            needle.transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z);
            angleDifference = Mathf.Abs(targetRotation.z - currentRotation.z);
            yield return null;
        }

        needle.transform.eulerAngles = targetRotation; // Ensuring the final rotation is set
    }

    private IEnumerator RotateNeedleToTargetWithSFX(Vector3 targetRotation)
    {
        yield return RotateNeedleToTarget(targetRotation);
        PlaySFX();
    }

    private void PlaySFX()
    {
        if (sfxClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(sfxClip);
        }
    }
}
