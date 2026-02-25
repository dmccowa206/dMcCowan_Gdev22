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
    [SerializeField] TMP_Text infoText;
    private const string startString = "Enter 3 Digit Combo";
    private const string resetString = "Enter 3 Digits to reset combo";
    [SerializeField] Image lockedPanel;
    [SerializeField] Color unlockedColor, lockedColor;
    [SerializeField] TMP_Text lockedText;
    private const string unlockText = "Unlocked";
    private const string lockText = "locked";
    [SerializeField] bool isLocked, isResettable;
    private bool resetCombo;
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
        if (resetCombo)
        {
            resetCombo = false;
            LockCombo();
            return;
        }
        int matches = 0;
        for (int i = 0; i < comboValues.Length; i++)
        {
            if (comboValues[i] == inputValues[i])
            {
                matches++;
            }
        }
        if (matches == maxBtnPresses)
        {
            UnlockCombo();
        }
        else
        {
            ResetUserValues();
        }
    }
    private void UnlockCombo()
    {
        isLocked = false;
        lockedPanel.color = unlockedColor;
        lockedText.text = unlockText;
        if (isResettable)
        {
            ResetCombo();
        }
    }
    private void LockCombo()
    {
        isLocked = true;
        lockedPanel.color = lockedColor;
        lockedText.text = lockText;
        infoText.text = startString;
        for (int i = 0; i < maxBtnPresses; i++)
        {
            comboValues[i] = inputValues[i];
        }
        ResetUserValues();
    }
    private void ResetCombo()
    {
        infoText.text = resetString;
        ResetUserValues();
        resetCombo = true;
    }
    private void ResetUserValues()
    {
        inputValues = new int[comboValues.Length];
        userInputTxt.text = "";
        btnPresses = 0;
    }
}
