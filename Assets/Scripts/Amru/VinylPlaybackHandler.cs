using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VinylPlaybackHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip vinylTrack; // Changed to a single AudioClip
    public GameObject vinyl;
    public Transform vinylMesh;
    public Transform stickerMesh;
    public XRGrabInteractable handleGrabInteractable;
    public XRSocketInteractor socketInteractor; // Added socket interactor

    private HingeJoint hingeJoint;
    private float totalAngle = 0f; // Track the total accumulated angle
    private bool isPlaying = false;
    private float lastAngle = 0f; // To track the last angle
    private float accumulatedPlaytime = 0f; // Track accumulated playtime
    private float targetPlaytime = 0f; // Target playtime based on rotation
    private bool isVinylOnSocket = false; // Track if the vinyl is on the socket

    void Awake()
    {
        if (handleGrabInteractable == null)
        {
            Debug.LogError("Handle XRGrabInteractable component is not assigned.");
            return;
        }

        handleGrabInteractable.selectEntered.AddListener(OnSelectEntered);
        handleGrabInteractable.selectExited.AddListener(OnSelectExited);

        hingeJoint = handleGrabInteractable.GetComponent<HingeJoint>();
        if (hingeJoint == null)
        {
            Debug.LogError("HingeJoint component is missing on the Handle GameObject.");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component is missing on this GameObject.");
            }
        }

        audioSource.clip = vinylTrack; // Assign the track to the audio source

        if (socketInteractor == null)
        {
            Debug.LogError("Socket Interactor component is not assigned.");
            return;
        }

        socketInteractor.selectEntered.AddListener(OnSocketEntered);
        socketInteractor.selectExited.AddListener(OnSocketExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isVinylOnSocket)
        {
            return;
        }

        lastAngle = hingeJoint.angle; // Set the last angle when the handle is grabbed
        Debug.Log("Handle grabbed, initial angle: " + lastAngle);
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (!isVinylOnSocket)
        {
            return;
        }

        float currentAngle = hingeJoint.angle;
        float deltaAngle = currentAngle - lastAngle;

        // Handle wrapping around the 0-360 angle range
        if (deltaAngle < -180f)
        {
            deltaAngle += 360f;
        }
        else if (deltaAngle > 180f)
        {
            deltaAngle -= 360f;
        }

        // Only process if the rotation is clockwise
        if (deltaAngle > 0f)
        {
            totalAngle += deltaAngle;
            float totalRotations = totalAngle / 360f;
            float rotationPercentage = totalRotations % 1f; // Get the fractional part of the rotations
            float newPlaytime = rotationPercentage * audioSource.clip.length;

            if (isPlaying)
            {
                targetPlaytime += newPlaytime;
            }
            else
            {
                targetPlaytime = accumulatedPlaytime + newPlaytime;
                audioSource.time = accumulatedPlaytime % audioSource.clip.length;
                isPlaying = true;
                audioSource.Play();
            }

            accumulatedPlaytime = targetPlaytime;

            Debug.Log("Handle released, delta angle: " + deltaAngle);
            Debug.Log("Accumulated playtime: " + accumulatedPlaytime);
            Debug.Log("Target playtime: " + targetPlaytime);
        }

        lastAngle = currentAngle;  // Update lastAngle to the current angle
    }

    private void OnSocketEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == vinyl.transform)
        {
            StartPlayback();
        }
    }

    private void OnSocketExited(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform == vinyl.transform)
        {
            StopPlayback();
        }
    }

    void Update()
    {
        if (isVinylOnSocket && isPlaying)
        {
            RotateVinylAndStickerMeshes();

            if (audioSource.time >= targetPlaytime % audioSource.clip.length)
            {
                audioSource.Stop();
                isPlaying = false;
                Debug.Log("Audio stopped, reached target playtime.");
            }
        }
    }

    private void RotateVinylAndStickerMeshes()
    {
        if (!isVinylOnSocket)
        {
            return;
        }

        float rotationSpeed = (720f / audioSource.clip.length) * Time.deltaTime * (isPlaying ? 1f : 0f); // Increased rotation speed
        vinylMesh.Rotate(0f, rotationSpeed, 0f);
        stickerMesh.Rotate(0f, rotationSpeed, 0f);
        Debug.Log("Rotating vinyl and sticker meshes at speed: " + rotationSpeed);
    }

    public void StartPlayback()
    {
        isVinylOnSocket = true;
        totalAngle = 0f; // Reset the total angle when a new track is set
        accumulatedPlaytime = 0f; // Reset accumulated playtime
        targetPlaytime = 0f; // Reset target playtime
        Debug.Log("Vinyl placed on socket.");
    }

    public void StopPlayback()
    {
        audioSource.Stop();
        isPlaying = false;
        isVinylOnSocket = false;
        accumulatedPlaytime = 0f; // Reset accumulated playtime when unsocketed
        totalAngle = 0f; // Reset total angle when unsocketed
        Debug.Log("Playback stopped and reset.");
    }

    void OnDestroy()
    {
        if (handleGrabInteractable != null)
        {
            handleGrabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            handleGrabInteractable.selectExited.RemoveListener(OnSelectExited);
        }

        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSocketEntered);
            socketInteractor.selectExited.RemoveListener(OnSocketExited);
        }
    }
}
