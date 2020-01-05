namespace Maoxun
{
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.SceneManagement;

    /// <summary>
    /// This script allows you to easily switch between editing a scene and playing from the init scene
    /// Old workflow-- edit your scene, open _init.unity, play, see changes, stop, open your original scene again, continue editing
    /// New workflow-- edit your scene, hit HOME, see changes, hit HOME, continue editing
    /// </summary>
    public class HotkeySmartPlay : Editor
    {
        private const string EDITOR_PREF_CODE_ORIGINAL_SCENE = "OriginalScene";
        private const string EDITOR_PREF_CODE_STOPPING_WITH_HOTKEY = "StoppingWithHotkey";
        private const string PLAY_FROM_FIRST_SCENE_MENU_ITEM = "Play From First Scene %F12";
        private static string initPath = "Assets/_SCENES/m01.unity";
        private static readonly string PLAY_MENU_OPTION = "Edit/Play";

        [MenuItem("Edit/Play From First Scene %F12", false, 100)]
        public static void SmartPlay()
        {
            // if you're in a scene, switch to _INIT and play
            if (ValidatePlayFromInit())
            {
                PlayFromInit();
            }
            // if you're playing, stop and return back to your scene
            else if (ValidateStopAndReturnToYourScene())
            {
                StopAndReturnToYourScene();
            }
            // if you're in _INIT and not playing, return back to your scene
            else if (ValidateReturnToYourScene())
            {
                ReturnToYourScene();
            }
        }

        private static bool ValidatePlayFromInit()
        {
            return !EditorApplication.isPlaying && EditorSceneManager.GetActiveScene().path != initPath;
        }

        private static void PlayFromInit()
        {
            EditorPrefs.SetString(EDITOR_PREF_CODE_ORIGINAL_SCENE, EditorSceneManager.GetActiveScene().path);
            EditorSceneManager.OpenScene(initPath);

            EditorApplication.ExecuteMenuItem(PLAY_MENU_OPTION);
        }

        private static bool ValidateStopAndReturnToYourScene()
        {
            return EditorApplication.isPlaying;
        }

        private static void StopAndReturnToYourScene()
        {
            EditorPrefs.SetBool(EDITOR_PREF_CODE_STOPPING_WITH_HOTKEY, true);
            EditorApplication.isPlaying = false;
            RegisterCallback();
        }

        private static bool ValidateReturnToYourScene()
        {
            return EditorSceneManager.GetActiveScene().path == initPath && !EditorApplication.isPlaying;
        }

        private static void ReturnToYourScene()
        {
            OpenOriginalScene();
        }

        private static void RegisterCallback()
        {
            EditorApplication.playModeStateChanged += WillOpenOriginalScene;
        }

        private static void DeregisterCallback()
        {
            EditorApplication.playModeStateChanged -= WillOpenOriginalScene;
        }

        // required because you have to wait for playback to fully stop before you switch scenes
        public static void WillOpenOriginalScene(PlayModeStateChange state)
        {
            DeregisterCallback();
            // Once you're done playing, jump back to the original scene you were editing
            if (!EditorApplication.isPlaying && EditorPrefs.GetBool(EDITOR_PREF_CODE_STOPPING_WITH_HOTKEY))
            {
                OpenOriginalScene();
            }
        }

        private static void OpenOriginalScene()
        {
            EditorSceneManager.OpenScene(EditorPrefs.GetString(EDITOR_PREF_CODE_ORIGINAL_SCENE));
            EditorPrefs.SetBool(EDITOR_PREF_CODE_STOPPING_WITH_HOTKEY, false);
        }
    }
#endif
}