using UnityEngine;
using System.Collections;

namespace UnityEngine
{
    public enum CameraView { Perspective, Orthoraphic };
}

public class CameraTransformation : MonoBehaviour
{
    public static CameraView CameraMode
    {
        get
        {
            if (Camera.main.isOrthoGraphic)
                return CameraView.Orthoraphic;
            else
                return CameraView.Perspective;
        }
        private set
        {
            if (value == CameraView.Orthoraphic)
            {
                if (!persChange)
                {
                    Camera.main.isOrthoGraphic = true;
                    ortoChange = true;
                }
            }
            else
            {
                if (!ortoChange)
                {
                    Camera.main.isOrthoGraphic = false;
                    persChange = true;
                }
            }
        }
    }
    //perspective
    public Vector3 PerspectivePosition = new Vector3(0, 233, -1000);
    private float p_xRoation = 3;
    private float p_size = 150, p_far = 10000;
    // ortographic
    public Vector3 OrthographicPosition = new Vector3(0, 450, 500);
    private float o_size = 1000, o_far = 1000;

    //Moves
    public float CameraMargin;
    private bool moveToTrack, moveToStart;
    private static bool ortoChange = false, persChange = false;
    private float signMTS = 0, signMTT = 0;

    public static PlayerMotion Player;

    [System.Obsolete("Only for debug !!")]
    public void TestCameraFlip()
    {
        if (CameraMode == CameraView.Orthoraphic)
            CameraMode = CameraView.Perspective;
        else
            CameraMode = CameraView.Orthoraphic;
    }

    void Start()
    {
        moveToTrack = false;
        moveToStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ortoChange)
            CameraToOrto(5, 8);
        if (persChange)
            CameraToPers(5, 8);

        //obsolete
        if (moveToTrack)
            MoveToTrack(5);

        if (moveToStart)
            MoveToStart(2);

        if (!PlayerMotion.Pause)
        {
            zRotation(0.1f);
            xAxisMove(5.5f);
            yAxisMove(0.05f, 1);
        }
    }

    private void xAxisMove(float xChange)
    {
        if (Player.transform.position.x > transform.position.x + CameraMargin)
        {
            transform.position = new Vector3(transform.position.x + xChange,
                transform.position.y,
                transform.position.z);
        }
        else if (Player.transform.position.x < transform.position.x - CameraMargin)
        {
            transform.position = new Vector3(transform.position.x - xChange,
                transform.position.y,
                transform.position.z);
        }
    }

    private void MoveToTrack(float xChange)
    {
        if (CameraMode == CameraView.Orthoraphic)
        {
            if (Camera.main.transform.position.x > OrthographicPosition.x)
                xChange = -xChange;
            if (signMTT == 0)
                signMTT = Mathf.Sign(xChange);
            if (signMTT != Mathf.Sign(xChange))
            {
                signMTT = 0;
                moveToTrack = false;

                return;
            }
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + xChange,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z);
        }
        else if (CameraMode == CameraView.Perspective)
        {
            if (Camera.main.transform.position.x > PerspectivePosition.x)
                xChange = -xChange;
            if (signMTT == 0)
                signMTT = Mathf.Sign(xChange);
            if (signMTT != Mathf.Sign(xChange))
            {
                signMTT = 0;
                moveToTrack = false;

                return;
            }
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + xChange,
                            Camera.main.transform.position.y,
                            Camera.main.transform.position.z);
        }
    }

    private void zRotation(float zRotation)
    {
        if (Input.GetButton("Horizontal"))
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (transform.rotation.z >= -Player.RotationLimit)
                    Camera.main.transform.Rotate(0, 0, -zRotation);
            }
            else
            {
                if (transform.rotation.z <= Player.RotationLimit)
                    Camera.main.transform.Rotate(0, 0, zRotation);
            }
        else
        {
            if (Camera.main.transform.rotation.z < 0)
                Camera.main.transform.Rotate(0, 0, zRotation);
            else if (Camera.main.transform.rotation.z > 0)
                Camera.main.transform.Rotate(0, 0, -zRotation);
        }
    }

    private void yAxisMove(float xRotation, float yChange)
    {
        if (CameraMode == CameraView.Perspective)
        {
            if (!Input.GetButton("Jump"))
            {
                if (Player.transform.position.y >= Player.yAxis)
                {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                                        Camera.main.transform.position.y - yChange, Camera.main.transform.position.z);
                    Camera.main.transform.Rotate(-xRotation, 0, 0);
                }
            }
            else
            {
                if (Player.transform.position.y <= Player.yAxis + Player.HeightLimit)
                {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                        Camera.main.transform.position.y + yChange, Camera.main.transform.position.z);
                    Camera.main.transform.Rotate(xRotation, 0, 0);
                }
            }
        }
    }

    //todo somting to change x of projectiles and spawns
    private void MoveToStart(float xChange)
    {
        if (Player.activeTrack.transform.position.x > 0)
            xChange = -xChange;
        if (signMTS == 0)
            signMTS = Mathf.Sign(xChange);
        if (signMTS != Mathf.Sign(xChange))
        {
            signMTS = 0;
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

    [System.Obsolete("only for debug")]
    public void MoveToStartAll()
    {
        moveToStart = true;
    }

    //nvm dafuq
    public void RecalculateCamera()
    {
        var track = Player.activeTrack;
        track.transform.position = new Vector3(track.transform.position.x,
            track.transform.position.y, 541.62f);
        //orto
        OrthographicPosition.x = track.transform.position.x;
        //pers
        PerspectivePosition.x = track.transform.position.x;
        if (CameraMode == CameraView.Orthoraphic)
            Camera.main.transform.position = new Vector3(OrthographicPosition.x, OrthographicPosition.y, OrthographicPosition.z);
        else
            Camera.main.transform.position = new Vector3(PerspectivePosition.x, PerspectivePosition.y, PerspectivePosition.z);
    }

    private void CameraToPers(float speed, float angle)
    {
        bool endTest = true;
        if (Camera.main.transform.position.y >= PerspectivePosition.y)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y - speed,
                Camera.main.transform.position.z);
            endTest = false;
        }
        if (Camera.main.transform.position.z >= PerspectivePosition.z)
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

    private void CameraToOrto(float speed, float angle)
    {
        bool endTest = true;
        if (Camera.main.transform.position.y < OrthographicPosition.y)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y + speed,
                Camera.main.transform.position.z);
            endTest = false;
        }
        if (Camera.main.transform.position.z <= OrthographicPosition.z)
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
