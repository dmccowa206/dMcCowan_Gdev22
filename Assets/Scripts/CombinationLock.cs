using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;

public class CombinationLock : MonoBehaviour
{
    [SerializeField] TMP_Text userInputTxt;
    [SerializeField] XRButtonInteractable[] comboBtns;
    [SerializeField] Image lockedPanel;
    [SerializeField] Color unlockedColor;
    [SerializeField] TMP_Text lockedText;
    private const string unlockedText = "Unlocked";
    [SerializeField] bool isLocked;
    [SerializeField] int[] comboValues = new int[3];
    [SerializeField] int[] inputValues;
    private int maxBtnPresses, btnPresses;
    void Start()
    {
        maxBtnPresses = comboValues.Length;
        ResetUserValues();
        // inputValues = new int[comboValues.Length];
        // userInputTxt.text = "";
        for (int i = 0; i < comboBtns.Length; i++)
        {
            comboBtns[i].selectEntered.AddListener(OnComboButtonPressed);
        }
    }

    private void OnComboButtonPressed(SelectEnterEventArgs arg0)
    {
        if (btnPresses >= maxBtnPresses)
        {
            //Too many button presses
        }
        else
        {
            for (int i = 0; i < comboBtns.Length; i++)
            {
                if (arg0.interactableObject.transform.name == comboBtns[i].transform.name)
                {
                    userInputTxt.text += i.ToString();
                    inputValues[btnPresses] = i;
                }
                else
                {
                    comboBtns[i].ResetColor();
                }
            }
            btnPresses++;
            if (btnPresses == maxBtnPresses)
            {
                CheckCombo();
            }
        }
    }

    private void CheckCombo()
    {
        int matches = 0;
        for (int i = 0; i < comboValues.Length; i++)
        {
            if (comboValues[i] == inputValues[i])
            {
                matches++;
            }
        }
        if (matches ==maxBtnPresses)
        {
            isLocked = false;
            lockedPanel.color = unlockedColor;
            lockedText.text = unlockedText;
        }
        else
        {
            ResetUserValues();
        }
    }

    private void ResetUserValues()
    {
        inputValues = new int[comboValues.Length];
        userInputTxt.text = "";
        btnPresses = 0;
    }
}
