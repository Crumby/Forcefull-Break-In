using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public enum CameraView { Perspective, Orthoraphic };
}

public class GameData : MonoBehaviour
{
    public static GameObject ActiveTrack { get; set; }
    public static GameObject Fog { get; private set; }
    public static bool PauseGame;
    public static List<GameObject> Tracks = new List<GameObject>();
    public static float MaxTracksX
    {
        get
        {
            float res = float.MinValue;
            foreach (var tr in Tracks)
            {
                res = Mathf.Max(res, tr.renderer.bounds.max.x);
            }

            return res;
        }
    }
    public static float MinTracksX
    {
        get
        {
            float res = float.MaxValue;
            foreach (var tr in Tracks)
            {
                res = Mathf.Min(res, tr.renderer.bounds.min.x);
            }
            return res;
        }
    }
    public static Transform PlayerPossition { get; private set; }
    public static float originalHeight { get; private set; }
    public static float maxHeight { get; private set; }

    public static void ResetData()
    {
        ActiveTrack = null;
        Fog = null;
        PlayerPossition = null;
        PauseGame = false;
        Tracks.Clear();
    }

    public static void InitData(PlayerMotion player, GameObject initTrack, GameObject initFog)
    {
        GameData.ActiveTrack = initTrack;
        GameData.Fog = initFog;
        PlayerPossition = player.transform;
        originalHeight = player.yAxis;
        maxHeight = player.HeightLimit;
    }

}
