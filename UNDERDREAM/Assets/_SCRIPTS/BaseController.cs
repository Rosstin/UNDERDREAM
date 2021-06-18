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
                break;
            }
        }


        // TODO(Rosstin): currentscene should work differently to force the actual right scene?
        Debug.Log("currentScene: " + Data.Scenes[Data.CurrentScene] + ".. password: " + Data.Passwords[Data.CurrentScene]);
    }

    public void BaseUpdate()
    {
        Data.TimeSinceLoadedLastScene += Time.deltaTime;
        UpdateKeycodesToCommands();

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.F9)){
            LoadNextScene();
        }
    }

    public void LoadNextScene(float delay = 0f)
    {
        StartCoroutine(LoadNextSceneCoroutine(delay));
    }

    private IEnumerator LoadNextSceneCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("LoadNextScene.. " + " from " + Data.Scenes[Data.CurrentScene] + " to whatever this index represents: " + Data.CurrentScene + 1);
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
            LoadScene(Data.Scenes[Data.CurrentScene]);
            Data.TimeSinceLoadedLastScene = 0f;
        }
        else
        {
            Debug.LogError("Scenes are being loaded too fast!");
        }
    }

    public void LoadHintScene()
    {


        Debug.LogWarning("loading hint scene.. Data.Scenes[Data.CurrentScene]: " + Data.Scenes[Data.CurrentScene]);
        if (Data.Scenes[Data.CurrentScene] == "m17")
        {
            LoadScene("h17");
            Data.TimeSinceLoadedLastScene = 0f;
        }
        else if (Data.Scenes[Data.CurrentScene] == "m23")
        {
            LoadScene("h23");
            Data.TimeSinceLoadedLastScene = 0f;
        }
        else if(Data.Scenes[Data.CurrentScene] == "m90")
        {
            LoadScene("h90");
            Data.TimeSinceLoadedLastScene = 0f;
        }
        else
        {
            Debug.LogWarning("this scene doesn't have a hint scene!");
        }
    }

    public void LoadRegularScene()
    {
        LoadScene(Data.Scenes[Data.CurrentScene]);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Data.TimeSinceLoadedLastScene = 0f;
    }


    #region UniversalInput
    public enum Command
    {
        Up,
        Down,
        Left,
        Right,
        Fire,
        Quit
    }

    [NonSerialized] public Dictionary<Command, bool> CommandsStartedThisFrame = new Dictionary<Command, bool>();
    [NonSerialized] public Dictionary<Command, bool> CommandsHeldThisFrame= new Dictionary<Command, bool>();

    private void UpdateKeycodesToCommands()
    {
        CommandsStartedThisFrame.Clear();
        CommandsHeldThisFrame.Clear();

        KeycodesToCommands(KeyCode.UpArrow, Command.Up);
        KeycodesToCommands(KeyCode.DownArrow, Command.Down);
        KeycodesToCommands(KeyCode.LeftArrow, Command.Left);
        KeycodesToCommands(KeyCode.RightArrow, Command.Right);
        KeycodesToCommands(KeyCode.Space, Command.Fire);
    }


    /// <summary>
    /// Adds commands to a list
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="command"></param>
    private void KeycodesToCommands(KeyCode keyCode, Command command)
    {
        if (Input.GetKeyDown(keyCode))
        {
            CommandsStartedThisFrame.Add(command, true);
        }

        if (Input.GetKey(keyCode))
        {
            CommandsHeldThisFrame.Add(command, true);
        }
    }
    #endregion

}
