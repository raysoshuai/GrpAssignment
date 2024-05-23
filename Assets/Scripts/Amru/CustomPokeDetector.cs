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

    private bool isLeftHandHovering = false;
    private bool isRightHandHovering = false;
    private bool isLeftTriggerPressed = false;
    private bool isRightTriggerPressed = false;
    private bool wasHoverAndPressInvoked = false;

    private void Awake()
    {
        Debug.Log("Awake: Initializing actions.");
        if (leftTriggerAction != null)
        {
            leftTriggerAction.started += OnLeftTriggerPressed;
            leftTriggerAction.canceled += OnLeftTriggerReleased;
        }
        else
        {
            Debug.LogError("Left trigger action not assigned.");
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.started += OnRightTriggerPressed;
            rightTriggerAction.canceled += OnRightTriggerReleased;
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
            leftTriggerAction.started -= OnLeftTriggerPressed;
            leftTriggerAction.canceled -= OnLeftTriggerReleased;
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.started -= OnRightTriggerPressed;
            rightTriggerAction.canceled -= OnRightTriggerReleased;
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

    private void OnLeftTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log("OnLeftTriggerPressed: Left trigger pressed.");
        isLeftTriggerPressed = true;
        wasHoverAndPressInvoked = false; // Reset the flag on new trigger press
        OnPress.Invoke();

        CheckHoverAndPress();
    }

    private void OnLeftTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("OnLeftTriggerReleased: Left trigger released.");
        isLeftTriggerPressed = false;
        wasHoverAndPressInvoked = false;
    }

    private void OnRightTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log("OnRightTriggerPressed: Right trigger pressed.");
        isRightTriggerPressed = true;
        wasHoverAndPressInvoked = false; // Reset the flag on new trigger press
        OnPress.Invoke();

        CheckHoverAndPress();
    }

    private void OnRightTriggerReleased(InputAction.CallbackContext context)
    {
        Debug.Log("OnRightTriggerReleased: Right trigger released.");
        isRightTriggerPressed = false;
        wasHoverAndPressInvoked = false;
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            isLeftHandHovering = true;
            Debug.Log("Hover Entered: Left hand hovering");
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            isRightHandHovering = true;
            Debug.Log("Hover Entered: Right hand hovering");
        }
        CheckHoverAndPress();
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            isLeftHandHovering = false;
            Debug.Log("Hover Exited: Left hand no longer hovering");
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            isRightHandHovering = false;
            Debug.Log("Hover Exited: Right hand no longer hovering");
        }
        wasHoverAndPressInvoked = false; // Reset the flag on hover exit
    }

    private void CheckHoverAndPress()
    {
        if ((isLeftHandHovering && isLeftTriggerPressed) || (isRightHandHovering && isRightTriggerPressed) && !wasHoverAndPressInvoked)
        {
            OnHoverAndPress.Invoke();
            wasHoverAndPressInvoked = true;
            Debug.Log("CheckHoverAndPress: OnHoverAndPress Invoked");
        }
    }
}
