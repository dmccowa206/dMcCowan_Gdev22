using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class ProgressControl : MonoBehaviour
{
    public UnityEvent<string> OnStartGame, OnChallengeComplete;
    [SerializeField] XRButtonInteractable startButton;
    [SerializeField] GameObject keyIndicatorLight;
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
}
