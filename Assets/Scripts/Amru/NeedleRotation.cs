

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NeedleRotation : MonoBehaviour
{
    public GameObject needle;  // Assign the needle GameObject in the Inspector
    private Quaternion initialRotation = Quaternion.Euler(0, 297.899994f, 0);
    private Quaternion enterRotation = Quaternion.Euler(4.26886828e-07f, 297.899994f, 194.999969f);

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        RotateNeedle(enterRotation);
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        RotateNeedle(initialRotation);
    }

    private void RotateNeedle(Quaternion targetRotation)
    {
        if (needle != null)
        {
            needle.transform.rotation = targetRotation;
        }
    }
}
