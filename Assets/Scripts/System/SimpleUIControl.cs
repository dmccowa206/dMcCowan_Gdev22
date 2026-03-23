using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class SimpleUIControl : MonoBehaviour
{
    [SerializeField] XRButtonInteractable startButton;
    [SerializeField] GameObject keyIndicatorLight;
    [SerializeField] string[] msgStrings;
    [SerializeField] TMP_Text[] msgTexts;
    void Start()
    {
        if (startButton != null)
        {
            startButton.selectEntered.AddListener(StartButtonPressed);
            if (keyIndicatorLight != null)
            {
                keyIndicatorLight.SetActive(true);
            }
        }
    }

    private void StartButtonPressed(SelectEnterEventArgs arg0)
    {
        SetText(msgStrings[1]);
    }

    public void SetText(string msg)
    {
        for (int i=0; i < msgTexts.Length; i++)
        {
            msgTexts[i].text = msg;
        }
    }
}
