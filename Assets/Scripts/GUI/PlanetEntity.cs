using UnityEngine;
using System.Collections;

public class PlanetEntity : MonoBehaviour
{
    public bool[] LevelsLock;
    public PlanetNames PlanetName;
    [HideInInspector]
    public int SelectedLevel = 1;

    public bool IsLocked()
    {
        if (LevelsLock.Length > 0)
            return LevelsLock[SelectedLevel - 1];
        else
            return true;
    }

    public void NextLevel()
    {
        if (SelectedLevel == LevelsLock.Length)
            SelectedLevel = 1;
        else
            SelectedLevel = SelectedLevel + 1;

    }

}
