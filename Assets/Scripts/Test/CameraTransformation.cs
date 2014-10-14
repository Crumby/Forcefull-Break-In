using UnityEngine;
using System.Collections;

public class CameraTransformation : MonoBehaviour
{
    public enum CameraView { Perspective, Orthoraphic };

    //perspective
    private static float p_xAxis, p_yAxis = 233, p_zAxis = -1000;
    private static float p_xRoation = 3;
    private static float p_size = 150, p_far = 10000;
    // ortographic
    private static float o_xAxis, o_yAxis = 450, o_zAxis = 500;
    private static float o_size = 1000, o_far = 1000;

    //Moves
    private static bool moveToTrack = false, moveToStart = false;
    private static float speed, angle;
    private static bool ortoChange = false, persChange = false;

    public static mov Player;
    private float signumDirection = 0;
    public static CameraView activeView = CameraView.Perspective;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (ortoChange)
            CameraToOrto();
        if (persChange)
            CameraToPers();
        if (moveToTrack)
            moveCamera();
        if (moveToStart)
            MoveToStart();
        if (!mov.Pause)
        {
            xAxisMove();
            yAxisMove();
        }
    }

    private void moveCamera()
    {
        float xChange = 5;
        if (activeView == CameraView.Orthoraphic)
        {
            if (Camera.main.transform.position.x > o_xAxis)
                xChange = -xChange;
            if (signumDirection == 0)
                signumDirection = Mathf.Sign(xChange);
            if (signumDirection != Mathf.Sign(xChange))
            {
                signumDirection = 0;
                moveToTrack = false;
                MoveObjectsToStart();
                return;
            }
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + xChange,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z);

        }
        else if (activeView == CameraView.Perspective)
        {
            if (Camera.main.transform.position.x > p_xAxis)
                xChange = -xChange;
            if (signumDirection == 0)
                signumDirection = Mathf.Sign(xChange);
            if (signumDirection != Mathf.Sign(xChange))
            {
                signumDirection = 0;
                moveToTrack = false;
                MoveObjectsToStart();
                return;
            }
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + xChange,
                            Camera.main.transform.position.y,
                            Camera.main.transform.position.z);
        }
    }

    public void CameraFlip()
    {
        float speed = 5, angle = 8;
        if (activeView == CameraView.Orthoraphic)
        {
            activeView = CameraView.Perspective;
            CameraTransformation.CameraToPers(speed, angle);
        }
        else
        {
            activeView = CameraView.Orthoraphic;
            CameraTransformation.CameraToOrto(speed, angle);
        }
    }

    private void xAxisMove()
    {
        if (Input.GetButton("Horizontal"))
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (transform.rotation.z >= -Player.RotationLimit)
                    Camera.main.transform.Rotate(0, 0, -0.1f);
            }
            else
            {
                if (transform.rotation.z <= Player.RotationLimit)
                    Camera.main.transform.Rotate(0, 0, 0.1f);
            }
        else
        {
            if (Camera.main.transform.rotation.z < 0)
                Camera.main.transform.Rotate(0, 0, 0.1f);
            else if (Camera.main.transform.rotation.z > 0)
                Camera.main.transform.Rotate(0, 0, -0.1f);
        }
    }

    private void yAxisMove()
    {
        if (!Input.GetButton("Jump"))
        {
            if (Player.transform.position.y >= Player.yAxis)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                                    Camera.main.transform.position.y - 1, Camera.main.transform.position.z);
                Camera.main.transform.Rotate(-0.05f, 0, 0);
            }
        }
        else
        {
            if (Player.transform.position.y <= Player.yAxis + Player.HeightLimit)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                    Camera.main.transform.position.y + 1, Camera.main.transform.position.z);
                Camera.main.transform.Rotate(0.05f, 0, 0);
            }
        }
    }

    //todo somting to change x of projectiles and spawns
    private void MoveToStart()
    {
        float xChange = 2;
        if (Player.activeTrack.transform.position.x > 0)
            xChange = -xChange;
        if (signumDirection == 0)
            signumDirection = Mathf.Sign(xChange);
        if (signumDirection != Mathf.Sign(xChange))
        {
            signumDirection = 0;
            moveToStart = false;
            //redo
            Spawns.leftPlatform = false;
            Spawns.rightPlatform = false;
            return;
        }
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + xChange,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z);
        Player.transform.position = new Vector3(Player.transform.position.x + xChange,
            Player.transform.position.y,
            Player.transform.position.z);
        Player.activeTrack.transform.position = new Vector3(Player.activeTrack.transform.position.x + xChange,
            Player.activeTrack.transform.position.y,
            Player.activeTrack.transform.position.z);
    }

    [System.Obsolete("Only for debuging.")]
    public void MoveAllToStart()
    {
        moveToStart = true;
    }

    public static void MoveObjectsToStart()
    {
        moveToStart = true;
    }

    [System.Obsolete("Only for debuging.")]
    public void MoveCameraToActivTrack()
    {
        MoveCameraToTrack(Player.activeTrack);
    }

    public static void MoveCameraToTrack(GameObject track)
    {
        RecalculateOrto(track);
        RecalculatePers(track);
        moveToTrack = true;
    }

    [System.Obsolete("Only for debuging.")]
    public static void RecalculateCamera(GameObject track)
    {
        track.transform.position = new Vector3(track.transform.position.x,
            track.transform.position.y, 541.62f);
        RecalculateOrto(track);
        RecalculatePers(track);
        if (activeView == CameraView.Orthoraphic)
            Camera.main.transform.position = new Vector3(o_xAxis, o_yAxis, o_zAxis);
        else
            Camera.main.transform.position = new Vector3(p_xAxis, p_yAxis, p_zAxis);
    }

    private static void RecalculateOrto(GameObject track)
    {
        o_xAxis = track.transform.position.x;
        //o_zAxis = track.transform.position.z;
    }

    //bug after changing platforms
    private static void RecalculatePers(GameObject track)
    {
        p_xAxis = track.transform.position.x;
        //p_zAxis = -2 * track.transform.position.z;
    }

    public static void CameraToPers(float speed, float angle)
    {
        if (!ortoChange)
        {
            CameraTransformation.speed = speed;
            CameraTransformation.angle = angle;
            persChange = true;
        }
    }

    private void CameraToPers()
    {
        bool endTest = true;
        if (Camera.main.transform.position.y >= p_yAxis)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y - speed,
                Camera.main.transform.position.z);
            endTest = false;
        }
        if (Camera.main.transform.position.z >= p_zAxis)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z - 5 * speed);
            endTest = false;
        }
        if (Camera.main.transform.rotation.eulerAngles.x > p_xRoation)
        {
            Camera.main.transform.Rotate(-2, 0, 0, Space.World);
            endTest = false;
        }
        else
        {
            Camera.main.transform.rotation.SetEulerAngles(p_xRoation, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z);
        }
        if (Camera.main.transform.eulerAngles.x < angle)
        {
            Camera.main.isOrthoGraphic = false;
            Camera.main.rect = new Rect(0, 0, 1, 1);
            Camera.main.farClipPlane = p_far;
            Camera.main.orthographicSize = p_size;
        }
        if (endTest)
            persChange = false;
    }

    public static void CameraToOrto(float speed, float angle)
    {
        if (!persChange)
        {
            CameraTransformation.speed = speed;
            CameraTransformation.angle = angle;
            ortoChange = true;
        }
    }

    private void CameraToOrto()
    {
        bool endTest = true;
        if (Camera.main.transform.position.y < o_yAxis)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y + speed,
                Camera.main.transform.position.z);
            endTest = false;
        }
        if (Camera.main.transform.position.z <= o_zAxis)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z + 5 * speed);
            endTest = false;
        }
        if (Camera.main.transform.rotation.eulerAngles.x < 90)
        {
            Camera.main.transform.Rotate(2f, 0, 0, Space.World);
            endTest = false;
        }
        else
        {
            Camera.main.transform.rotation.SetEulerAngles(90, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z);
        }
        if (Camera.main.transform.eulerAngles.x > angle)
        {
            Camera.main.isOrthoGraphic = true;
            Camera.main.rect = new Rect(0, 0, 1, 2);
            Camera.main.farClipPlane = o_far;
            Camera.main.orthographicSize = o_size;
        }
        if (endTest)
            ortoChange = false;
    }
}
