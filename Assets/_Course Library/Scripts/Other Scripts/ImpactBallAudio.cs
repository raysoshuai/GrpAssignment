using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ImpactBallAudio : MonoBehaviour
{
    public AudioClip impact;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter()
    {
        audioSource.PlayOneShot(impact, 0.7F);
    }
}
