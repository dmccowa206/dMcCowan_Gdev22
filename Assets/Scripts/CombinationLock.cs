using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class CombinationLock : MonoBehaviour
{
    [SerializeField] TMP_Text userInputTxt;
    [SerializeField] XRButtonInteractable[] comboBtns;
    void Start()
    {
        userInputTxt.text = "";
        for (int i = 0; i < comboBtns.Length; i++)
        {
            comboBtns[i].selectEntered.AddListener(OnComboButtonPressed);
        }
    }

    private void OnComboButtonPressed(SelectEnterEventArgs arg0)
    {
        for (int i = 0; i < comboBtns.Length; i++)
        {
            if (arg0.interactableObject.transform.name == comboBtns[i].transform.name)
            {
                userInputTxt.text = i.ToString();
            }
            else
            {
                comboBtns[i].ResetColor();
            }
        }
    }
}
