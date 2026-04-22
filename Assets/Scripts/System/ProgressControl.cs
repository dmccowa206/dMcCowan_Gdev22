using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class ProgressControl : MonoBehaviour
{
    public UnityEvent<string> OnStartGame, OnChallengeComplete;
    [Header("Start Button")]
    [SerializeField] XRButtonInteractable startButton;
    [SerializeField] GameObject keyIndicatorLight;
    [Header("Drawer Interactable")]
    [SerializeField] DrawerInteractable drawer;
    XRSocketInteractor drawerSocket;
    [Header("Combo Lock")]
    [SerializeField] CombinationLock comboLock;
    [Header("The Wall")]
    [SerializeField] TheWall wall;
    XRSocketInteractor wallSocket;
    [SerializeField] GameObject teleportAreas;
    [Header("Library")]
    [SerializeField] SimpleSliderControl librarySlider;
    [Header("The Robot")]
    [SerializeField] NavMeshRobot robot;
    [Header("Challenge Settings")]
    [SerializeField] string startGameString, endGameString;
    [SerializeField] string[] challengeStrings;
    [SerializeField] int wallCubeDestroyQuota;
    private int wallCubesDestroyed;
    private bool startGameBool, challengesCompleted;
    [SerializeField] private int challengeNum;
    void Start()
    {
        if (startButton != null)
        {
            startButton.selectEntered.AddListener(StartButtonPressed);
        }
        OnStartGame?.Invoke(startGameString);
        SetDrawerInteractable();
        if(comboLock != null)
        {
            comboLock.UnlockAction += OnComboUnlocked;
        }
        if(wall != null)
        {
            SetWall();
        }
        if(librarySlider != null)
        {
            librarySlider.OnSliderActive.AddListener(LibrarySliderActive);
        }
        if (robot != null)
        {
            robot.OnDestroyWallCube.AddListener(OnDestroyWallCube);
        }
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
            OnChallengeComplete?.Invoke(endGameString);
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
            if (challengeNum < challengeStrings.Length && challengeNum == 0)
            {
                OnStartGame?.Invoke(challengeStrings[challengeNum]);
            }
        }
    }
    private void OnDrawerSocketed(SelectEnterEventArgs arg0)
    {
        if (challengeNum == 0)
        {
            ChallengeComplete();
        }
    }
    private void OnDrawerDetach()
    {
        if (challengeNum == 1)
        {
            ChallengeComplete();
        }
    }
    private void OnComboUnlocked()
    {
        if (challengeNum == 2)
        {
            ChallengeComplete();
        }
    }
    private void OnWallSocketed(SelectEnterEventArgs arg0)
    {
        if (challengeNum == 3)
        {
            ChallengeComplete();
        }
    }
    private void OnDestroyWall()
    {
        if (challengeNum == 4)
        {
            ChallengeComplete();
        }
        if(teleportAreas != null)
        {
            teleportAreas.SetActive(true);
        }
    }
    private void LibrarySliderActive()
    {
        if (challengeNum == 5)
        {
            ChallengeComplete();
        }
    }
    private void OnDestroyWallCube()
    {
        wallCubesDestroyed++;
        if (wallCubesDestroyed >= wallCubeDestroyQuota && !challengesCompleted)
        {
            challengesCompleted = true;
            if (challengeNum == 6)
            {
                ChallengeComplete();
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
    private void SetWall()
    {
        wall.OnDestroy.AddListener(OnDestroyWall);
        wallSocket = wall.GetWallSocket;
        if (wallSocket != null)
        {
            wallSocket.selectEntered.AddListener(OnWallSocketed);
        }
    }



}
