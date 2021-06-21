using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
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

        if (Input.GetKeyDown(KeyCode.F9)){
            LoadNextScene();
        }

        if(Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl))
        {
            Application.Quit();
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

        // Fire/Jump
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            if (!CommandsStartedThisFrame.ContainsKey(Command.Fire)) CommandsStartedThisFrame.Add(Command.Fire, true);
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            if (!CommandsHeldThisFrame.ContainsKey(Command.Fire)) CommandsHeldThisFrame.Add(Command.Fire, true);
        }
        KeycodesToCommands(KeyCode.Space, Command.Fire);
        KeycodesToCommands(KeyCode.E, Command.Fire);
        KeycodesToCommands(KeyCode.Q, Command.Fire);
        KeycodesToCommands(KeyCode.LeftShift, Command.Fire);
        KeycodesToCommands(KeyCode.RightShift, Command.Fire);

        // Up
        KeycodesToCommands(KeyCode.UpArrow, Command.Up);
        KeycodesToCommands(KeyCode.W, Command.Up);

        // Down
        KeycodesToCommands(KeyCode.DownArrow, Command.Down);
        KeycodesToCommands(KeyCode.S, Command.Down);

        // Left
        KeycodesToCommands(KeyCode.LeftArrow, Command.Left);
        KeycodesToCommands(KeyCode.A, Command.Left);

        // Right
        KeycodesToCommands(KeyCode.RightArrow, Command.Right);
        KeycodesToCommands(KeyCode.D, Command.Right);

                if(Gamepad.current != null){
        ButtonControlsToCommands(Gamepad.current.rightTrigger, Command.Fire);
        ButtonControlsToCommands(Gamepad.current.leftTrigger, Command.Fire);
        ButtonControlsToCommands(Gamepad.current.buttonNorth, Command.Fire);
        ButtonControlsToCommands(Gamepad.current.buttonSouth, Command.Fire);
        ButtonControlsToCommands(Gamepad.current.buttonEast, Command.Fire);
        ButtonControlsToCommands(Gamepad.current.buttonWest, Command.Fire);

        ButtonControlsToCommands(Gamepad.current.leftStick.up, Command.Up);
        ButtonControlsToCommands(Gamepad.current.rightStick.up, Command.Up);
        ButtonControlsToCommands(Gamepad.current.dpad.up, Command.Up);

        ButtonControlsToCommands(Gamepad.current.leftStick.down, Command.Down);
        ButtonControlsToCommands(Gamepad.current.rightStick.down, Command.Down);
        ButtonControlsToCommands(Gamepad.current.dpad.down, Command.Down);

        ButtonControlsToCommands(Gamepad.current.leftStick.left, Command.Left);
        ButtonControlsToCommands(Gamepad.current.rightStick.left, Command.Left);
        ButtonControlsToCommands(Gamepad.current.dpad.left, Command.Left);
        
        ButtonControlsToCommands(Gamepad.current.leftStick.right, Command.Right);
        ButtonControlsToCommands(Gamepad.current.rightStick.right, Command.Right);
        ButtonControlsToCommands(Gamepad.current.dpad.right, Command.Right);
        }

    }


    private void ButtonControlsToCommands(ButtonControl buttonControl, Command command)
    {
        if (buttonControl.wasPressedThisFrame)
        {
            if(!CommandsStartedThisFrame.ContainsKey(command)) CommandsStartedThisFrame.Add(command, true);
        }

        if (buttonControl.isPressed)
        {
            if (!CommandsHeldThisFrame.ContainsKey(command)) CommandsHeldThisFrame.Add(command, true);
        }
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
            if (!CommandsStartedThisFrame.ContainsKey(command)) CommandsStartedThisFrame.Add(command, true);
        }

        if (Input.GetKey(keyCode))
        {
            if (!CommandsHeldThisFrame.ContainsKey(command)) CommandsHeldThisFrame.Add(command, true);
        }
    }
    #endregion

}
