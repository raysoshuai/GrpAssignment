using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VinylSocketHandler : MonoBehaviour
{
    public VinylPlaybackHandler vinylPlaybackHandler;
    public int vinylTrackIndex;  // Assign the appropriate track index for this vinyl record in the Inspector
    public XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing on this GameObject.");
            return;
        }

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (vinylPlaybackHandler != null)
        {
            vinylPlaybackHandler.SetVinylTrack(vinylTrackIndex);
            vinylPlaybackHandler.StartPlayback();
        }
        else
        {
            Debug.LogError("VinylPlaybackHandler is not assigned.");
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (vinylPlaybackHandler != null)
        {
            vinylPlaybackHandler.StopPlayback();
        }
        else
        {
            Debug.LogError("VinylPlaybackHandler is not assigned.");
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }
}
