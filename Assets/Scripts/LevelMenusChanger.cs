using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMenusChanger : MonoBehaviour
{

    public CommonClass.LevelButtonNames buttonName = CommonClass.LevelButtonNames.Undefined;

    private const string MainMenuScene = "Menus";
    private static bool IsShopShowed = false;
    private static CommonClass.LevelButtonNames lastPressedButton = CommonClass.LevelButtonNames.Undefined;
    private static Dictionary<CommonClass.LevelButtonNames, GUIText> buttons = new Dictionary<CommonClass.LevelButtonNames, GUIText>();

    public void Start()
    {
        if (!buttons.ContainsKey(buttonName))
            buttons.Add(buttonName, this.guiText);
        IsShopShowed = false;
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
                case CommonClass.LevelButtonNames.Undefined:
                    {
                        Debug.LogError(this.ToString() + "Is undefined button.");
                        break;
                    }
                case CommonClass.LevelButtonNames.MainMenu:
                    {
                        Application.LoadLevel(MainMenuScene);
                        break;
                    }
                case CommonClass.LevelButtonNames.Shop:
                    {
                        if (IsShopShowed)
                            HideSubMenus();
                        else
                            ShowShop();
                        break;
                    }
                case CommonClass.LevelButtonNames.NextStage:
                    {
                        PlanetSettings.SelectedPlanet.NextStage();
                        break;
                    }
                case CommonClass.LevelButtonNames.PrevStage:
                    {
                        PlanetSettings.SelectedPlanet.PrevStage();
                        break;
                    }
                case CommonClass.LevelButtonNames.Start:
                    {
                        PlanetSettings.SelectedPlanet.LoadLevel();
                        break;
                    }
            }

        }
        lastPressedButton = CommonClass.LevelButtonNames.Undefined;
    }

    private void ShowShop()
    {
        Camera.main.cullingMask = (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("UIShop")) | (1 << LayerMask.NameToLayer("UIBackground"));
        IsShopShowed = true;
    }

    private void HideSubMenus()
    {
        Camera.main.cullingMask = (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("UIBackground")) | (1 << LayerMask.NameToLayer("UIPlanets"));
        IsShopShowed = false;
    }
}
