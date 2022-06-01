using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyButtons { Interact, MoveUp, MoveDown, MoveLeft, MoveRight, MenuUp, MenuDown, MenuSelect, PauseResume, SwapInteraction, AdvanceCutscene }

public class GameSettings : MonoBehaviour
{
    private static GameSettings instance;

    public static GameSettings Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }

    public static float ChangeDirectionDelay = .15f;

    public static float DefaultCamSize = 5f;

    public static float MainCameraSpeed = 1f;

    public static float MainCameraSizeSpeed = 1f;

    public static string NewGameSceneName = "Puzzle_Lobby_1";


    public static float CutsceneSlideFadeSpeed = 1f;

    // KeyControls

    private static Dictionary<KeyButtons, List<KeyCode>> BindDict = new Dictionary<KeyButtons, List<KeyCode>>
    {
        { KeyButtons.Interact, new List<KeyCode> { KeyCode.Space, KeyCode.E } },
        { KeyButtons.MoveUp, new List<KeyCode> { KeyCode.UpArrow, KeyCode.W } },
        { KeyButtons.MoveDown, new List<KeyCode> { KeyCode.DownArrow, KeyCode.S } },
        { KeyButtons.MoveLeft, new List<KeyCode> { KeyCode.LeftArrow, KeyCode.A } },
        { KeyButtons.MoveRight, new List<KeyCode> { KeyCode.RightArrow, KeyCode.D } },
        { KeyButtons.MenuUp, new List<KeyCode> { KeyCode.UpArrow, KeyCode.W } },
        { KeyButtons.MenuDown, new List<KeyCode> { KeyCode.DownArrow, KeyCode.S } },
        { KeyButtons.MenuSelect, new List<KeyCode> { KeyCode.Space, KeyCode.E } },
        { KeyButtons.PauseResume, new List<KeyCode> { KeyCode.Escape } },
        { KeyButtons.SwapInteraction, new List<KeyCode> { KeyCode.X } },
        { KeyButtons.AdvanceCutscene, new List<KeyCode> { KeyCode.Z, KeyCode.Space } },
    };

    public bool GetKeyBinding(KeyButtons kb)
    {
        List<KeyCode> keycodes = BindDict[kb];
        foreach  (KeyCode k in keycodes)
        {
            if (Input.GetKey(k))
                return true;
        }

        return false;
    }

    public bool GetKeyBindingDown(KeyButtons kb)
    {
        List<KeyCode> keycodes = BindDict[kb];
        foreach (KeyCode k in keycodes)
        {
            if (Input.GetKeyDown(k))
                return true;
        }

        return false;
    }

    // Can add main menu states here for having main menu anywhere...
    public bool TimePausedState()
    {
        return GameManager.Instance.State == GameState.PauseMenu || GameManager.Instance.State == GameState.SaveMenu;
    }
}
