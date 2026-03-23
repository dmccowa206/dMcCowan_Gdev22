using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRAudioManager : MonoBehaviour
{
    [Header("Grab Interactables")]
    [SerializeField] XRGrabInteractable[] grabInteractables;
    [SerializeField] AudioSource grabSound;
    [SerializeField] AudioClip grabClip, keyClip;
    [SerializeField] AudioSource activatedSound;
    [SerializeField] AudioClip grabActivatedClip, wandActivatedClip;
    [Header("Drawer Interactable")]
    [SerializeField] DrawerInteractable drawer;
    [SerializeField] XRSocketInteractor drawerSocket;
    [SerializeField] AudioSource drawerSound, drawerSocketSound;
    [SerializeField] AudioClip drawerMoveClip, drawerSocketClip;
    [Header("The Wall")]
    [SerializeField] TheWall wall;
    [SerializeField] XRSocketInteractor wallSocket;
    [SerializeField] AudioSource wallSound, wallSocketSound;
    [SerializeField] AudioClip destroyWallClip, wallSocketClip;
    [SerializeField] private AudioClip fallbackClip;
    private const string FALLBACKCLIP_NAME = "fallbackClip";

    private void OnEnable()
    {
        if (fallbackClip == null)
        {
            fallbackClip = AudioClip.Create(FALLBACKCLIP_NAME, 1, 1, 1000, true);
        }
        SetGrabbables();
        if(drawer != null)
        {
            SetDrawerInteractable();
        }
        if (wall != null)
        {
            SetWall();
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
        drawerSound.Play();
    }

    private void OnDrawerMove(SelectEnterEventArgs arg0)
    {
        drawerSound.Stop();
    }
    private void OnDrawerSocketed(SelectEnterEventArgs arg0)
    {
        drawerSocketSound.Play();
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
