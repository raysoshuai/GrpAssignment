using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomPokeDetector : MonoBehaviour
{
    [Tooltip("Actions to check")]
    public InputAction leftTriggerAction = null;
    public InputAction rightTriggerAction = null;

    [Tooltip("Interactable object to detect hover events")]
    public XRSimpleInteractable interactable;

    // When the button is pressed
    public UnityEvent OnPress = new UnityEvent();
    public UnityEvent OnHoverAndPress = new UnityEvent();
    public UnityEvent OnHoverExitOrReleaseOrOnHoverAndRelease = new UnityEvent(); // Consolidated event

    private bool isHovering = false;
    private bool isTriggerPressed = false;

    private void Awake()
    {
        Debug.Log("Awake: Initializing actions.");
        if (leftTriggerAction != null)
        {
            leftTriggerAction.started += OnTriggerPressed;
            leftTriggerAction.canceled += OnTriggerReleased;
        }
        else
        {
            Debug.LogError("Left trigger action not assigned.");
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.started += OnTriggerPressed;
            rightTriggerAction.canceled += OnTriggerReleased;
        }
        else
        {
            Debug.LogError("Right trigger action not assigned.");
        }
    }

    private void OnDestroy()
    {
        if (leftTriggerAction != null)
        {
            leftTriggerAction.started -= OnTriggerPressed;
            leftTriggerAction.canceled -= OnTriggerReleased;
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.started -= OnTriggerPressed;
            rightTriggerAction.canceled -= OnTriggerReleased;
        }
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable: Enabling actions.");
        if (leftTriggerAction != null)
        {
            leftTriggerAction.Enable();
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.Enable();
        }
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable: Disabling actions.");
        if (leftTriggerAction != null)
        {
            leftTriggerAction.Disable();
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.Disable();
        }
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log("OnTriggerPressed: Button pressed.");
        float triggerValue = context.action.ReadValue<float>();
        if (triggerValue > 0.0f)
        {
            isTriggerPressed = true;
            OnPress.Invoke();
            Debug.Log("Trigger Pressed");

            if (isHovering)
            {
                OnHoverAndPress.Invoke();
                Debug.Log("Trigger Pressed and Hovering");
            }
        }
    }

    private void OnTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("OnTriggerReleased: Button released.");
        isTriggerPressed = false;

        // Invoke consolidated event on release
        if (isHovering)
        {
            OnHoverExitOrReleaseOrOnHoverAndRelease.Invoke();
            Debug.Log("Trigger Released and Hovering");
        }
        else
        {
            OnHoverExitOrReleaseOrOnHoverAndRelease.Invoke();
            Debug.Log("Trigger Released and Not Hovering");
        }
    }

    private void Start()
    {
        Debug.Log("Start: Setting up interactable.");
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHoverEnter);
            interactable.hoverExited.AddListener(OnHoverExit);
        }
        else
        {
            Debug.LogError("XRSimpleInteractable object not assigned.");
        }
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        isHovering = true;
        Debug.Log("Hover Entered");

        if (isTriggerPressed)
        {
            OnHoverAndPress.Invoke();
            Debug.Log("Trigger Pressed and Hover Entered");
        }
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        isHovering = false;
        Debug.Log("Hover Exited");

        // Invoke consolidated event on hover exit
        OnHoverExitOrReleaseOrOnHoverAndRelease.Invoke();
        Debug.Log("Hover Exited");
    }

    private void Update()
    {
        if (leftTriggerAction != null && leftTriggerAction.triggered)
        {
            Debug.Log("Left trigger action detected in Update.");
        }

        if (rightTriggerAction != null && rightTriggerAction.triggered)
        {
            Debug.Log("Right trigger action detected in Update.");
        }
    }
}
