//flower disappear
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class flower01 : MonoBehaviour
{
    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Destroy(gameObject); // Make the flower disappear
    }

    private void OnDestroy()
    {
        // Cleanup to avoid memory leaks
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnSelectEntered);
    }
}
