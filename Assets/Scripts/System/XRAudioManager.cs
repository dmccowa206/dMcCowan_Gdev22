using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRAudioManager : MonoBehaviour
{
    [Header("Progress Control")]
    [SerializeField] ProgressControl progCon;
    [SerializeField] AudioSource progSound;
    [SerializeField] AudioClip startGameClip, chalCompleteClip;
    [Header("Grab Interactables")]
    [SerializeField] XRGrabInteractable[] grabInteractables;
    [SerializeField] AudioSource grabSound;
    [SerializeField] AudioClip grabClip, keyClip;
    [SerializeField] AudioSource activatedSound;
    [SerializeField] AudioClip grabActivatedClip, wandActivatedClip;
    [Header("Drawer Interactable")]
    [SerializeField] DrawerInteractable drawer;
    XRSocketInteractor drawerSocket;
    AudioSource drawerSound, drawerSocketSound;
    AudioClip drawerMoveClip, drawerSocketClip;
    [Header("Door Interactable")]
    [SerializeField] SimpleHingeInteractable[] cabinetDoors = new SimpleHingeInteractable[2];
    AudioSource[] cabinetDoorSound;
    AudioClip cabinetDoorMoveClip;
    [Header("Combo Lock")]
    [SerializeField] CombinationLock comboLock;
    AudioSource comboLockSound;
    AudioClip comboLockClip, comboUnlockClip, comboButtonPressClip;
    [Header("The Wall")]
    [SerializeField] TheWall wall;
    XRSocketInteractor wallSocket;
    [SerializeField] AudioSource wallSound;
    AudioSource wallSocketSound;
    AudioClip destroyWallClip, wallSocketClip;
    [Header("Local Audio Settings")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioClip backgroundMusicClip, fallbackClip;
    private const string FALLBACKCLIP_NAME = "fallbackClip";
    private bool startAudioBool;

    private void OnEnable()
    {
        if (progCon != null)
        {
            progCon.OnStartGame.AddListener(StartGame);
            progCon.OnChallengeComplete.AddListener(ChallengeComplete);
        }
        if (fallbackClip == null)
        {
            fallbackClip = AudioClip.Create(FALLBACKCLIP_NAME, 1, 1, 1000, true);
        }
        SetGrabbables();
        if(drawer != null)
        {
            SetDrawerInteractable();
        }
        cabinetDoorSound = new AudioSource[cabinetDoors.Length];
        for (int i = 0; i < cabinetDoors.Length; i++)
        {
            if(cabinetDoors[i] != null)
            {
                SetCabinetDoors(i);
            }
        }
        if (comboLock != null)
        {
            SetComboLock();
        }
        if (wall != null)
        {
            SetWall();
        }
    }

    private void ChallengeComplete(string arg0)
    {
        if(progSound != null && chalCompleteClip != null)
        {
            progSound.clip = chalCompleteClip;
            progSound.Play();
        }
    }

    private void StartGame(string arg0)
    {
        if (!startAudioBool)
        {
            startAudioBool = true;
            if (backgroundMusic != null && backgroundMusicClip != null)
            {
                backgroundMusic.clip = backgroundMusicClip;
                backgroundMusic.Play();
            }
        }
        else
        {
            if(progSound != null && startGameClip != null)
            {
                progSound.clip = startGameClip;
                progSound.Play();
            }
        }
    }

    private void OnDisable()
    {
        if (wall != null)
        {
            wall.OnDestroy.RemoveListener(OnDestroyWall);
        }
    }
    private void SetGrabbables()
    {
        grabInteractables = FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.None);
        for (int i = 0; i < grabInteractables.Length; i++)
        {
            grabInteractables[i].selectEntered.AddListener(OnSelectEnteredGrabbable);
            grabInteractables[i].selectExited.AddListener(OnSelectExitedGrabbable);
            grabInteractables[i].activated.AddListener(OnActivatedGrabbable);
        }
    }
    private void SetDrawerInteractable()
    {
            drawerSound = drawer.transform.AddComponent<AudioSource>();
            drawerMoveClip = drawer.GetDrawerMoveClip;
            CheckClip(ref drawerMoveClip);
            drawerSound.clip = drawerMoveClip;
            drawerSound.loop = true;
            drawer.selectEntered.AddListener(OnDrawerMove);
            drawer.selectExited.AddListener(OnDrawerStop);
            drawerSocket = drawer.GetKeySocket;
            if (drawerSocket != null)
        {
            drawerSocketSound = drawerSocket.transform.AddComponent<AudioSource>();
            drawerSocketClip = drawer.GetSocketedClip;
            CheckClip(ref drawerSocketClip);
            drawerSocketSound.clip = drawerSocketClip;
            drawerSocket.selectEntered.AddListener(OnDrawerSocketed);
        }
    }
    private void SetCabinetDoors(int index)
    {
        cabinetDoorSound[index] = cabinetDoors[index].transform.AddComponent<AudioSource>();
        cabinetDoorMoveClip = cabinetDoors[index].GetHingeMoveClip;
        CheckClip(ref cabinetDoorMoveClip);
        cabinetDoorSound[index].clip = cabinetDoorMoveClip;
        cabinetDoors[index].OnHingeSelected.AddListener(OnDoorMove);
        cabinetDoors[index].selectExited.AddListener(OnDoorStop);
    }
    private void SetComboLock()
    {
        comboLockSound = comboLock.transform.AddComponent<AudioSource>();
        comboLockClip = comboLock.GetLockClip;
        CheckClip(ref comboLockClip);
        comboUnlockClip = comboLock.GetUnlockClip;
        CheckClip(ref comboUnlockClip);
        comboButtonPressClip = comboLock.GetComboButtonPressClip;
        CheckClip(ref comboButtonPressClip);
        comboLock.LockAction += OnComboLocked;
        comboLock.UnlockAction += OnComboUnlocked;
        comboLock.ComboButtonPressed += OnComboButtonPress;
    }
    private void SetWall()
    {
            destroyWallClip = wall.GetDestroyClip;
            CheckClip(ref destroyWallClip);
            wall.OnDestroy.AddListener(OnDestroyWall);
            wallSocket = wall.GetWallSocket;
            if (wallSocket != null)
        {
            wallSocketSound = wallSocket.transform.AddComponent<AudioSource>();
            wallSocketClip = wall.GetSocketClip;
            CheckClip(ref wallSocketClip);
            wallSocketSound.clip = wallSocketClip;
            wallSocket.selectEntered.AddListener(OnWallSocketed);
        }
    }

    private void CheckClip(ref AudioClip clip)
    {
        if(clip == null)
        {
            clip = fallbackClip;
        }
    }
    private void OnSelectEnteredGrabbable(SelectEnterEventArgs arg0)
    {
        if(arg0.interactableObject.transform.CompareTag("Key"))
        {
            grabSound.clip = keyClip;
        }
        else
        {
            grabSound.clip = grabClip;
        }
        grabSound.Play();
    }

    private void OnSelectExitedGrabbable(SelectExitEventArgs arg0)
    {
        grabSound.clip = grabClip;
        grabSound.Play();
    }
    private void OnActivatedGrabbable(ActivateEventArgs arg0)
    {
        GameObject tempGameObject = arg0.interactableObject.transform.gameObject;
        if (tempGameObject.GetComponent<WandControl>() != null)
        {
            activatedSound.clip = wandActivatedClip;
        }
        else
        {
            activatedSound.clip = grabActivatedClip;
        }
        activatedSound.Play();
    }
    private void OnDrawerStop(SelectExitEventArgs arg0)
    {
        drawerSound.Stop();
    }

    private void OnDrawerMove(SelectEnterEventArgs arg0)
    {
        drawerSound.Play();
    }
    private void OnDrawerSocketed(SelectEnterEventArgs arg0)
    {
        drawerSocketSound.Play();
    }
    private void OnDoorMove(SimpleHingeInteractable arg0)
    {
        for (int i = 0; i < cabinetDoors.Length; i++)
        {
            if(arg0 == cabinetDoors[i])
            {
                cabinetDoorSound[i].Play();
            }            
        }
    }
    private void OnDoorStop(SelectExitEventArgs arg0)
    {
        for (int i = 0; i < cabinetDoors.Length; i++)
        {
            if(arg0.interactableObject == cabinetDoors[i])
            {
                cabinetDoorSound[i].Stop();
            }            
        }
    }
    private void OnComboLocked()
    {
        comboLockSound.clip = comboLockClip;
        comboLockSound.Play();
    }
    private void OnComboUnlocked()
    {
        comboLockSound.clip = comboUnlockClip;
        comboLockSound.Play();
    }
    private void OnComboButtonPress()
    {
        comboLockSound.clip = comboButtonPressClip;
        comboLockSound.Play();
    }
    private void OnDestroyWall()
    {
        if (wallSound != null)
        {
            wallSound.Play();
        }
    }
    private void OnWallSocketed(SelectEnterEventArgs arg0)
    {
        wallSocketSound.Play();
    }
}
