using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseController : MonoBehaviour
{
    public UniversalDataSO Data;
    public AudioSource Eee;

    private int CurrentScene = 0;
    private float timeSinceLoadedLastScene = 0;
    private float loadingCooldown = 0.1f; // if a scene attempts loading multiple times, ignore it until at least this much time has passed

    public void BaseUpdate()
    {
        timeSinceLoadedLastScene += Time.deltaTime;

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
        if(timeSinceLoadedLastScene > loadingCooldown)
        {
            if(CurrentScene< Data.Scenes.Count)
            {
                CurrentScene++;
            }
            else
            {
                Debug.LogWarning("end of game, looping");
                CurrentScene = 0;
            }
            SceneManager.LoadScene(Data.Scenes[CurrentScene]);
            timeSinceLoadedLastScene = 0f;
        }
        else
        {
            Debug.LogError("Scenes are being loaded too fast!");
        }

    }

}
