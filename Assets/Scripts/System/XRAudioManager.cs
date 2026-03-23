using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRAudioManager : MonoBehaviour
{
    [SerializeField] TheWall wall;
    [SerializeField] AudioSource wallSource;
    [SerializeField] AudioClip destroyWallClip;
    [SerializeField] private AudioClip fallbackClip;
    private const string FALLBACKCLIP_NAME = "fallbackClip";

    private void OnEnable()
    {
        if (fallbackClip == null)
        {
            fallbackClip = AudioClip.Create(FALLBACKCLIP_NAME, 1, 1, 1000, true);
        }
        if (wall != null)
        {
            destroyWallClip = wall.GetDestroyClip;
            if(destroyWallClip == null)
            {
                destroyWallClip = fallbackClip;
            }
            wall.OnDestroy.AddListener(OnDestroyWall);
        }
    }
    private void OnDisable()
    {
        if (wall != null)
        {
            wall.OnDestroy.RemoveListener(OnDestroyWall);
        }
    }
    private void OnDestroyWall()
    {
        if (wallSource != null)
        {
            wallSource.Play();
        }
    }
}
