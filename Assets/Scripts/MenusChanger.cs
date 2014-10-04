using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenusChanger : MonoBehaviour
{
    public enum ButtonNames { Undefined, NewGame, Setting, Credits, QuitGame, SoundEnable, Difficulty, Resolution, BackToMenu };
    public enum Difficulty { Normal, Hard, Brutal };
    public enum Resolution { low, medium, high };

    public ButtonNames buttonName = ButtonNames.Undefined;

    private static bool IsSettingShowed = false;
    private static string LevelLoaderScene = "LevelMenus";
    private static bool SoundEnabled = true;
    private static Difficulty CurentDificulity;
    private static ButtonNames lastPressedButton = ButtonNames.Undefined;
    private static Dictionary<ButtonNames, GUIText> buttons = new Dictionary<ButtonNames, GUIText>();

    public void Start()
    {
        if (!buttons.ContainsKey(buttonName))
            buttons.Add(buttonName, this.guiText);
    }

    public void OnMouseDown()
    {
        lastPressedButton = buttonName;
    }

    public void OnMouseUp()
    {
        if (lastPressedButton == buttonName)
        {
            print(lastPressedButton);
            switch (lastPressedButton)
            {
                case ButtonNames.Undefined:
                    {
                        Debug.LogError(this.ToString() + "Is undefined button.");
                        break;
                    }
                case ButtonNames.NewGame:
                    {
                        Application.LoadLevel(LevelLoaderScene);
                        break;
                    }
                case ButtonNames.Credits:
                    {
                        ShowCredits();
                        break;
                    }
                case ButtonNames.QuitGame:
                    {
                        OnClosing();
                        Application.Quit();
                        break;
                    }
                case ButtonNames.Setting:
                    {
                        if (IsSettingShowed)
                            HideSubMenus();
                        else
                            ShowOptions();
                        break;
                    }
                case ButtonNames.SoundEnable:
                    {
                        if (SoundEnabled)
                            buttons[ButtonNames.SoundEnable].text = "Off";
                        else
                            buttons[ButtonNames.SoundEnable].text = "On";
                        SoundEnabled = !SoundEnabled;
                        break;
                    }
                case ButtonNames.Resolution:
                    {
                        break;
                    }
                case ButtonNames.Difficulty:
                    {

                        if (CurentDificulity == Difficulty.Brutal)
                            CurentDificulity = Difficulty.Normal;
                        else
                            CurentDificulity = CurentDificulity + 1;
                        buttons[ButtonNames.Difficulty].text = CurentDificulity.ToString();
                        break;
                    }
                case ButtonNames.BackToMenu:
                    {
                        HideSubMenus();
                        break;
                    }
            }

        }
        lastPressedButton = ButtonNames.Undefined;
    }

    private void ShowCredits()
    {
        Camera.main.cullingMask = (1 << LayerMask.NameToLayer("UIBackground")) | (1 << LayerMask.NameToLayer("UICredits"));
    }
    private void ShowOptions()
    {
        Camera.main.cullingMask = (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("UIOptions")) | (1 << LayerMask.NameToLayer("UIBackground"));
        IsSettingShowed = true;
    }

    private void HideSubMenus()
    {
        Camera.main.cullingMask = (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("UIBackground"));
        IsSettingShowed = false;
    }
    private void OnClosing() { }
}
