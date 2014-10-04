using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelMenusChanger : MonoBehaviour
{
    public enum LevelButtonNames { Undefined, MainMenu, Shop, Start, NextStage, PrevStage, Stage, PlanetName, PlanetAcessibility };

    public LevelButtonNames buttonName = LevelButtonNames.Undefined;

    private static string MainMenuScene = "Menus";
    private static bool IsShopShowed = false;
    private static LevelButtonNames lastPressedButton = LevelButtonNames.Undefined;
    private static Dictionary<LevelButtonNames, GUIText> buttons = new Dictionary<LevelButtonNames, GUIText>();
    public static Dictionary<PlanetSettings.Planet, PlanetSettings> planets = new Dictionary<PlanetSettings.Planet, PlanetSettings>();
    private static PlanetSettings.Planet selectedPlanet = PlanetSettings.Planet.Eurasia;
    private static PlanetSettings.Planet SelectedPlanet
    {
        get { return selectedPlanet; }
        set
        {
            selectedPlanet = value;
            buttons[LevelButtonNames.PlanetName].text = selectedPlanet.ToString();
            buttons[LevelButtonNames.Stage].text = planets[selectedPlanet].SelectedStage.ToString();
            buttons[LevelButtonNames.PlanetAcessibility].enabled = !planets[selectedPlanet].IsLocked;
        }
    }



    public void Update()
    {

    }

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
                case LevelButtonNames.Undefined:
                    {
                        Debug.LogError(this.ToString() + "Is undefined button.");
                        break;
                    }
                case LevelButtonNames.MainMenu:
                    {
                        Application.LoadLevel(MainMenuScene);
                        break;
                    }
                case LevelButtonNames.Shop:
                    {
                        if (IsShopShowed)
                            HideSubMenus();
                        else
                            ShowShop();
                        break;
                    }
                case LevelButtonNames.NextStage:
                    {
                        planets[selectedPlanet].NextStage();
                        buttons[LevelButtonNames.Stage].text = planets[selectedPlanet].SelectedStage.ToString();
                        break;
                    }
                case LevelButtonNames.PrevStage:
                    {
                        planets[selectedPlanet].PrevStage();
                        buttons[LevelButtonNames.Stage].text = planets[selectedPlanet].SelectedStage.ToString();
                        break;
                    }
                case LevelButtonNames.Start:
                    {
                        planets[selectedPlanet].LoadLevel();
                        break;
                    }
            }

        }
        lastPressedButton = LevelButtonNames.Undefined;
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
