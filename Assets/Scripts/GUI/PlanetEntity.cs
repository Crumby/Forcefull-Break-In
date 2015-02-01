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
        if (MenusLogic.levelsLocks[(int)PlanetName].Length > 0)
            return MenusLogic.levelsLocks[(int)PlanetName][SelectedLevel - 1];
        else
            return true;
    }

    public void NextLevel()
    {
        if (SelectedLevel == MenusLogic.levelsLocks[(int)PlanetName].Length)
            SelectedLevel = 1;
        else
            SelectedLevel = SelectedLevel + 1;

    }

    public void unlockNext() {
        if (SelectedLevel< MenusLogic.levelsLocks[(int)PlanetName].Length)
            MenusLogic.levelsLocks[(int)PlanetName][SelectedLevel] = true;
    }
}
