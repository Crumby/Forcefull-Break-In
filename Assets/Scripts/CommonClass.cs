using UnityEngine;
using System.Collections;

public class CommonClass : MonoBehaviour
{
    public enum Planet { Undefined, Eurasia, Karbon, Ising };
    public enum LevelButtonNames { Undefined, MainMenu, Shop, Start, NextStage, PrevStage, Stage, PlanetName, PlanetAcessibility };
    public enum ButtonNames { Undefined, NewGame, Setting, Credits, QuitGame, SoundEnable, Difficulty, Resolution, BackToMenu };
    public enum Difficulty { Normal, Hard, Brutal };


    public static T GetObject<T>(string name) where T : UnityEngine.Component
    {
        var obj = GameObject.Find(name);
        if (obj != null)
        {
            var sobj = obj.GetComponent<T>();
            if (sobj != null)
                return sobj;
        }
        return default(T);
    }


}
