using UnityEngine;
using System.Collections;

public class PlanetSettings : MonoBehaviour
{
    public CommonClass.Planet Name = CommonClass.Planet.Undefined;
    public string SceneName = "";
    public int SelectedStage = 1;
    public bool[] IsLocked;
    private Color OriginalColor;

    public static PlanetSettings SelectedPlanet { get; private set; }
    private static float MenuxOffset, MenuyOffset;
    private static CommonClass.Planet lastPressedPlanet = CommonClass.Planet.Undefined;
    private static GUIText Lock, PlanetName, Stage;
    private static GUITexture Background;

    // Use this for initialization
    void Start()
    {
        if (Name == CommonClass.Planet.Eurasia)
        {
            Lock = CommonClass.GetObject<GUIText>("GUI Text_PlanetAccesibility");
            PlanetName = CommonClass.GetObject<GUIText>("GUI Text_PlanetName");
            Stage = CommonClass.GetObject<GUIText>("GUI Text_StageLvl");
            Background = CommonClass.GetObject<GUITexture>("GUI_PlanetBackground");
            SelectedPlanet = this;
            MenuxOffset = Background.transform.position.x - transform.position.x;
            MenuyOffset = +Background.transform.position.y - transform.position.y;
            updateStage();
        }
        OriginalColor = this.guiTexture.color;

    }

    public void NextStage()
    {
        if (SelectedStage == IsLocked.Length)
            SelectedStage = 1;
        else
            SelectedStage = (SelectedStage + 1) % (IsLocked.Length + 1);
        updateStage();
    }
    public void PrevStage()
    {
        if (SelectedStage == 1)
            SelectedStage = IsLocked.Length;
        else
            SelectedStage = SelectedStage - 1;
        updateStage();
    }

    public void LoadLevel()
    {
        Application.LoadLevel("main");
    }

    public void OnMouseEnter()
    {
        if (SelectedPlanet.Name != Name)
            this.guiTexture.color = Color.white;
    }

    public void OnMouseExit()
    {
        this.guiTexture.color = OriginalColor;
    }

    public void OnMouseDown()
    {
        lastPressedPlanet = Name;

    }

    private void updateStage()
    {
        Stage.guiText.text = SelectedStage.ToString();
        Lock.guiText.enabled = !IsLocked[SelectedStage - 1];
    }

    public void OnMouseUp()
    {
        if (lastPressedPlanet == Name)
        {
            if (SelectedPlanet != null && Name == SelectedPlanet.Name)
            {
                lastPressedPlanet = CommonClass.Planet.Undefined;
                return;
            }
            SelectedPlanet = this;
            Background.transform.position = new Vector3(transform.position.x + MenuxOffset, transform.position.y + MenuyOffset, Background.transform.position.z);
            PlanetName.guiText.text = Name.ToString();
            updateStage();
        }
        lastPressedPlanet = CommonClass.Planet.Undefined;

    }
}
