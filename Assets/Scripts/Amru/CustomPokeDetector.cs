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

    private bool isHovering = false;
    private bool isTriggerPressed = false;
    private bool wasHoverAndPressInvoked = false;

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

    private void Update()
    {
        // This method can be removed since we handle trigger detection in the callbacks
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log("OnTriggerPressed: Button pressed.");
        isTriggerPressed = true;
        wasHoverAndPressInvoked = false; // Reset the flag on new trigger press
        OnPress.Invoke();

        CheckHoverAndPress();
    }

    private void OnTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("OnTriggerReleased: Button released.");
        isTriggerPressed = false;
        wasHoverAndPressInvoked = false;
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        isHovering = true;
        Debug.Log("Hover Entered: isHovering set to true");
        CheckHoverAndPress();
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        isHovering = false;
        wasHoverAndPressInvoked = false; // Reset the flag on hover exit
        Debug.Log("Hover Exited: isHovering set to false");
    }

    private void CheckHoverAndPress()
    {
        if (isHovering && isTriggerPressed && !wasHoverAndPressInvoked)
        {
            OnHoverAndPress.Invoke();
            wasHoverAndPressInvoked = true;
            Debug.Log("CheckHoverAndPress: OnHoverAndPress Invoked");
        }
    }
}
