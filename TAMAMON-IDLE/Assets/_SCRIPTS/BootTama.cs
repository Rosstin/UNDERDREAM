using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootTama : MonoBehaviour
{
    // LOCATIONS
    private const string PLAYER_LOCATION_KEY = "PLAYER_LOCATION_KEY";
    private const string CAVE_LOC_KEY = "CAVE_LOC_KEY";


    void Awake()
    {
        Debug.LogWarning("PlayerPrefs.GetString(PLAYER_LOCATION_KEY): " + PlayerPrefs.GetString(PLAYER_LOCATION_KEY));

        var currentLoc = PlayerPrefs.GetString(PLAYER_LOCATION_KEY);

        switch (currentLoc)
        {
            default:
                Debug.LogWarning("player not init");
                PlayerPrefs.SetString(PLAYER_LOCATION_KEY, CAVE_LOC_KEY);
                break;
            case CAVE_LOC_KEY:
                Debug.LogWarning("player in cave!");
                LoadScene(CAVE_LOC_KEY);
                break;

        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
