using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HighlightControl : MonoBehaviour
{
    [SerializeField] XRBaseInteractable interactableObject;
    [SerializeField] Material startMaterial, emissionMaterial;
    [SerializeField] Renderer highlightableObject;
    private void OnEnable()
    {
        if (interactableObject != null)
        {
            interactableObject.selectEntered.AddListener(HighlightObject);
            interactableObject.selectEntered.AddListener(ResetObject);
        }
    }
    private void OnDisable()
    {
        if (interactableObject != null)
        {
            interactableObject.selectEntered.AddListener(HighlightObject);
            interactableObject.selectEntered.AddListener(ResetObject);
        }
    }
    private void HighlightObject(SelectEnterEventArgs arg0)
    {
        if (highlightableObject != null && startMaterial != null)
        {
            highlightableObject.material = startMaterial;
        }
    }
    void ResetObject(SelectEnterEventArgs arg0)
    {
        if (highlightableObject != null && emissionMaterial != null)
        {
            highlightableObject.material = emissionMaterial;
        }
    }
}
