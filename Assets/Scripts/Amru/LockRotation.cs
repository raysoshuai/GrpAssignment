using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class LockRotation : MonoBehaviour
{
    [SerializeField]
    private HingeJoint hingeJoint;
    [SerializeField]
    private XRGrabInteractable grabInteractable;

    [SerializeField]
    private Rigidbody rb;

    private float lastAngle = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); 

        if (hingeJoint == null)
            hingeJoint = GetComponent<HingeJoint>();
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        SetupHingeJointLimits();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void SetupHingeJointLimits()
    {
        if (hingeJoint != null)
        {
            JointLimits jointLimits = hingeJoint.limits;
            jointLimits.min = 0;
            jointLimits.max = 360; 
            hingeJoint.limits = jointLimits;
            hingeJoint.useLimits = true;
        }
        else
        {
            Debug.LogError("HingeJoint component is missing on this GameObject.");
        }
    }

    void FixedUpdate()
    {
        float currentAngle = hingeJoint.angle;
        if (currentAngle < lastAngle) // Prevents anticlockwise movement
        {
            JointLimits jointLimits = hingeJoint.limits;
            jointLimits.min = currentAngle;
            hingeJoint.limits = jointLimits;
        }
        lastAngle = currentAngle;
    }



    private void OnSelectEntered(SelectEnterEventArgs arg)
    {
        // Reduce angular drag when grabbed for smoother interaction
        rb.angularDrag = CalculateAngularDrag();
    }

    private void OnSelectExited(SelectExitEventArgs arg)
    {
        // Reset velocities and apply higher angular drag to stabilize the object when released
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.angularDrag = 10f; // Higher drag to quickly stabilize the object

        // Lock the hinge joint at the last angle using a spring
        hingeJoint.useSpring = true;
        JointSpring jointSpring = hingeJoint.spring;
        jointSpring.targetPosition = lastAngle;  // Lock to the last recorded angle
        jointSpring.spring = 1000f;  // Adjust stiffness 
        jointSpring.damper = 50f;  // Adjust damping 
        hingeJoint.spring = jointSpring;
    }


    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    float CalculateAngularDrag()
    {
        // No angular drag
        return 0f;
    }
}