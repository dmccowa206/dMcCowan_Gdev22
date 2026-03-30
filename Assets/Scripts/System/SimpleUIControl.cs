using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class SimpleUIControl : MonoBehaviour
{
    [SerializeField] ProgressControl progCon;
    [SerializeField] TMP_Text[] msgTexts;
    
    void OnEnable() {
        if (progCon != null)
        {
            progCon.OnStartGame.AddListener(StartGame);
            progCon.OnChallengeComplete.AddListener(ChallengeCompleted);
        }
    }
    public void SetText(string msg)
    {
        for (int i=0; i < msgTexts.Length; i++)
        {
            msgTexts[i].text = msg;
        }
    }
    private void StartGame(string arg0)
    {
        SetText(arg0);
    }
    private void ChallengeCompleted(string arg0)
    {
        SetText(arg0);
    }
}
