using UnityEngine;
using System.Collections;

public class PlanetSettings : MonoBehaviour
{

    public enum Planet { Undefined, Eurasia, Karbon, Ising };

    public Planet Name = Planet.Undefined;
    public uint NumberOfStages = 5;
    public uint SelectedStage = 1;
    public bool IsLocked = true;
    private Color OriginalColor;

    // Use this for initialization
    void Start()
    {
        if (!LevelMenusChanger.planets.ContainsKey(Name))
            LevelMenusChanger.planets.Add(Name, this);
        OriginalColor = this.guiTexture.color;
    }

    public void NextStage()
    {
        if (SelectedStage == 5)
            SelectedStage = 1;
        else
            SelectedStage = (SelectedStage + 1) % 6;
    }
    public void PrevStage()
    {
        if (SelectedStage == 1)
            SelectedStage = 5;
        else
            SelectedStage = SelectedStage - 1;
    }

    public void LoadLevel()
    {
        Application.LoadLevel(Name.ToString() + SelectedStage);
    }

    public void OnMouseEnter()
    {
        this.guiTexture.color = Color.white;
    }

    public void OnMouseExit()
    {
        this.guiTexture.color = OriginalColor;
    }

}
