using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// dont forget to call baseupdate in classes that inherit from this 
/// </summary>
public class BaseController : MonoBehaviour
{
    public UniversalDataSO Data;

    public void Start()
    {
        // set the current scene based on where this scene is in the build order
        for(int i = 0; i < Data.Scenes.Count; i++)
        {
            if(Data.Scenes[i] == SceneManager.GetActiveScene().name)
            {
                Data.CurrentScene = i;
            }
        }

        Debug.Log("currentScene: " + Data.CurrentScene);
    }

    public void BaseUpdate()
    {
        Data.TimeSinceLoadedLastScene += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.F9)){
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        Debug.Log("LoadNextScene");
        if (Data.TimeSinceLoadedLastScene > Data.LoadingCooldown)
        {
            if (Data.CurrentScene < Data.Scenes.Count - 1)
            {
                Data.CurrentScene++;
            }
            else
            {
                Debug.LogWarning("end of game, looping");
                Data.CurrentScene = 0;
            }
            SceneManager.LoadScene(Data.Scenes[Data.CurrentScene]);
            Data.TimeSinceLoadedLastScene = 0f;
        }
        else
        {
            Debug.LogError("Scenes are being loaded too fast!");
        }

    }

}
