using System.Collections;
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

    [Header("Songs")]
    [SerializeField] private AudioSource TigerSongPrefab;
    [SerializeField] private AudioSource SabreSongPrefab;

    private AudioSource tigerSongInstance = null;
    public AudioSource GetTigerSong() { return tigerSongInstance; }

    private AudioSource sabreSongInstance;
    public AudioSource GetSabreSong() { return sabreSongInstance; }

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

    public bool SabreSongExists()
    {
        return sabreSongInstance != null;
    }

    public void CreateSabreSong()
    {
        sabreSongInstance = Instantiate(SabreSongPrefab.gameObject).GetComponent<AudioSource>();
        DontDestroyOnLoad(sabreSongInstance);
    }

    public void DestroySabreSong()
    {
        Destroy(sabreSongInstance);
        sabreSongInstance = null;
    }


}
