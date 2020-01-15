using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseController : MonoBehaviour
{
    public UniversalDataSO Data;
    public AudioSource Eee;

    public void BaseUpdate()
    {
        Data.TimeSinceLoadedLastScene += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Eee.Play();
        }
    }

    public void LoadNextScene()
    {
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
