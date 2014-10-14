using UnityEngine;
using System.Collections;

public class PlanetEntity : MonoBehaviour
{
    public bool[] LevelsLock;
    public string PlanetName, LevelScene;
    public int SelectedLevel { get; private set; }

    public void Start()
    {
        SelectedLevel = 1;
    }

    public bool IsLocked()
    {
        return LevelsLock[SelectedLevel - 1];
    }

    public void NextLevel()
    {
        if (SelectedLevel == LevelsLock.Length)
            SelectedLevel = 1;
        else
            SelectedLevel = SelectedLevel + 1;

    }

}
