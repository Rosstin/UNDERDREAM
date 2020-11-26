﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UniversalDataSO", order = 1)]
public class UniversalDataSO : ScriptableObject
{
    [Header("Scene Data")]
    public List<string> Scenes;
    public List<string> Passwords;

    [Header("Modified During Runtime")]
    public int CurrentScene = 0;
    public float TimeSinceLoadedLastScene = 0;

    [Header("Loading Cooldown")]
    public float LoadingCooldown = 0.1f; // if a scene attempts loading multiple times, ignore it until at least this much time has passed

    [Header("Tiger Song")]
    [SerializeField] private AudioSource TigerSongPrefab;

    private AudioSource tigerSongInstance = null;

    public AudioSource GetTigerSong() { return tigerSongInstance; }

    public bool TigerSongExists()
    {
        return (tigerSongInstance != null);
    }

    public void CreateTigerSong()
    {
        tigerSongInstance = Instantiate(TigerSongPrefab.gameObject).GetComponent<AudioSource>();
        DontDestroyOnLoad(tigerSongInstance);
    }

    public void DestroyTigerSong()
    {
        Destroy(tigerSongInstance);
        tigerSongInstance = null;
    }


}
