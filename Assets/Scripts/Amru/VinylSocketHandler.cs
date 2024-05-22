using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VinylSocketHandler : MonoBehaviour
{
    public VinylPlaybackHandler vinylPlaybackHandler;
    public int vinylTrackIndex;  // Assign the appropriate track index for this vinyl record in the Inspector
    public XRSocketInteractor socketInteractor;

    private void Awake()
    {
        if (socketInteractor == null)
        {
            socketInteractor = GetComponent<XRSocketInteractor>();
        }

        if (socketInteractor == null)
        {
            Debug.LogError("XRSocketInteractor component is missing on this GameObject.");
            return;
        }

        socketInteractor.selectEntered.AddListener(OnSelectEntered);
        socketInteractor.selectExited.AddListener(OnSelectExited);
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

    private void OnDestroy()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
            socketInteractor.selectExited.RemoveListener(OnSelectExited);
        }
    }
}
