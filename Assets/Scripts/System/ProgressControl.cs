using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System;

public class ProgressControl : MonoBehaviour
{
    public UnityEvent<string> OnStartGame, OnChallengeComplete;
    [Header("Start Button")]
    [SerializeField] XRButtonInteractable startButton;
    [SerializeField] GameObject keyIndicatorLight;
    [Header("Drawer Interactable")]
    [SerializeField] DrawerInteractable drawer;
    XRSocketInteractor drawerSocket;
    [Header("Challenge Settings")]
    [SerializeField] string StartGameString;
    [SerializeField] string[] challengeStrings;
    private bool startGameBool;
    private int challengeNum;
    void Start()
    {
        if (startButton != null)
        {
            startButton.selectEntered.AddListener(StartButtonPressed);
        }
        OnStartGame?.Invoke(StartGameString);
        SetDrawerInteractable();
    }
    private void ChallengeComplete()
    {
        challengeNum++;
        if(challengeNum < challengeStrings.Length)
        {
            OnChallengeComplete?.Invoke(challengeStrings[challengeNum]);
        }
        else if (challengeNum >= challengeStrings.Length)
        {
            
        }
    }
    private void StartButtonPressed(SelectEnterEventArgs arg0)
    {
        if(!startGameBool)
        {
            startGameBool = true;
            if (keyIndicatorLight != null)
            {
                keyIndicatorLight.SetActive(true);
            }
            if (challengeNum < challengeStrings.Length)
            {
                OnStartGame?.Invoke(challengeStrings[challengeNum]);
            }
        }
    }
    private void SetDrawerInteractable()
    {
        if(drawer != null)
        {
            drawer.OnDrawerDetach.AddListener(OnDrawerDetach);
            drawerSocket = drawer.GetKeySocket;
            if (drawerSocket != null)
            {
                drawerSocket.selectEntered.AddListener(OnDrawerSocketed);
            }
        }
    }

    private void OnDrawerSocketed(SelectEnterEventArgs arg0)
    {
        ChallengeComplete();
    }
    private void OnDrawerDetach()
    {
        ChallengeComplete();
    }
}
