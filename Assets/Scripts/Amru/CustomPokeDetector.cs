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

    private void Awake()
    {
        Debug.Log("Awake: Initializing actions.");
        if (leftTriggerAction != null)
        {
            leftTriggerAction.started += Pressed;
        }
        else
        {
            Debug.LogError("Left trigger action not assigned.");
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.started += Pressed;
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
            leftTriggerAction.started -= Pressed;
        }

        if (rightTriggerAction != null)
        {
            rightTriggerAction.started -= Pressed;
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

    private void Pressed(InputAction.CallbackContext context)
    {
        Debug.Log("Pressed: Button pressed.");
        float triggerValue = context.action.ReadValue<float>();
        if (triggerValue > 0.0f)
        {
            OnPress.Invoke();
            Debug.Log("Trigger Pressed");

            if (isHovering)
            {
                OnHoverAndPress.Invoke();
                Debug.Log("Trigger Pressed and Hovering");
            }
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
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        isHovering = false;
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
