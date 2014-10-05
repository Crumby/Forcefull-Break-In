using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenusChanger : MonoBehaviour
{
    public CommonClass.ButtonNames buttonName = CommonClass.ButtonNames.Undefined;

    private static bool IsSettingShowed = false;
    private const string LevelLoaderScene = "LevelMenus";
    private static bool SoundEnabled = true;
    private static CommonClass.Difficulty CurentDificulity = CommonClass.Difficulty.Normal;
    private static CommonClass.ButtonNames lastPressedButton = CommonClass.ButtonNames.Undefined;
    private static Dictionary<CommonClass.ButtonNames, GUIText> buttons = new Dictionary<CommonClass.ButtonNames, GUIText>();

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
                case CommonClass.ButtonNames.Undefined:
                    {
                        Debug.LogError(this.ToString() + "Is undefined button.");
                        break;
                    }
                case CommonClass.ButtonNames.NewGame:
                    {
                        Application.LoadLevel(LevelLoaderScene);
                        break;
                    }
                case CommonClass.ButtonNames.Credits:
                    {
                        ShowCredits();
                        break;
                    }
                case CommonClass.ButtonNames.QuitGame:
                    {
                        OnClosing();
                        Application.Quit();
                        break;
                    }
                case CommonClass.ButtonNames.Setting:
                    {
                        if (IsSettingShowed)
                            HideSubMenus();
                        else
                            ShowOptions();
                        break;
                    }
                case CommonClass.ButtonNames.SoundEnable:
                    {
                        if (SoundEnabled)
                            buttons[CommonClass.ButtonNames.SoundEnable].text = "Off";
                        else
                            buttons[CommonClass.ButtonNames.SoundEnable].text = "On";
                        SoundEnabled = !SoundEnabled;
                        break;
                    }
                case CommonClass.ButtonNames.Resolution:
                    {
                        break;
                    }
                case CommonClass.ButtonNames.Difficulty:
                    {

                        if (CurentDificulity == CommonClass.Difficulty.Brutal)
                            CurentDificulity = CommonClass.Difficulty.Normal;
                        else
                            CurentDificulity = CurentDificulity + 1;
                        buttons[CommonClass.ButtonNames.Difficulty].text = CurentDificulity.ToString();
                        break;
                    }
                case CommonClass.ButtonNames.BackToMenu:
                    {
                        HideSubMenus();
                        break;
                    }
            }

        }
        lastPressedButton = CommonClass.ButtonNames.Undefined;
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
