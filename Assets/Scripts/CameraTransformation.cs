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
            CameraToOrto(400, 30, 90);
        if (persChange)
            CameraToPers(280, 45, 90);
        //obsolete
        if (moveToTrack)
            MoveToTrack(500);

        if (moveToStart)
            MoveToStart(200);

        if (!PlayerMotion.Pause)
        {
            zRotation(Player.RotationChange / 4);

            xAxisMove(Player.SideChange);
            yAxisMove(5, 100);
        }
    }

    private void xAxisMove(float xChange)
    {
        if (Input.GetButton("Horizontal"))
            if (Player.transform.position.x > transform.position.x + CameraMargin)
                transform.Translate(xChange * Time.deltaTime, 0, 0, Space.World);
            else if (Player.transform.position.x < transform.position.x - CameraMargin)
                transform.Translate(-xChange * Time.deltaTime, 0, 0, Space.World);
    }

    private void MoveToTrack(float xChange)
    {
        if (CameraMode == CameraView.Orthoraphic)
        {
            if (transform.position.x > OrthographicPosition.x)
                xChange = -xChange;
            if (signMTT == 0)
                signMTT = Mathf.Sign(xChange);
            if (signMTT != Mathf.Sign(xChange))
            {
                signMTT = 0;
                moveToTrack = false;

                return;
            }
        }
        else if (CameraMode == CameraView.Perspective)
        {
            if (transform.position.x > PerspectivePosition.x)
                xChange = -xChange;
            if (signMTT == 0)
                signMTT = Mathf.Sign(xChange);
            if (signMTT != Mathf.Sign(xChange))
            {
                signMTT = 0;
                moveToTrack = false;

                return;
            }
        }
        transform.Translate(xChange * Time.deltaTime, 0, 0, Space.World);

    }

    private void zAxisMove()
    {
        if (Input.GetButton("Vertical"))
            if (Input.GetAxis("Vertical") > 0)
            {
                if (CameraTransformation.CameraMode == CameraView.Perspective && Player.transform.position.z + Player.ForwardChange <= Player.zAxis + Player.ForwardLimit)
                    transform.Translate(0, 0, Player.ForwardChange * Time.deltaTime / 2, Space.World);
                else if (CameraTransformation.CameraMode == CameraView.Orthoraphic &&
                    Player.transform.position.z <= Player.activeTrack.renderer.bounds.max.z)
                    transform.Translate(0, 0, Player.ForwardChange * Time.deltaTime / 2, Space.World);
            }
            else
            {
                if (CameraTransformation.CameraMode == CameraView.Perspective && Player.transform.position.z - Player.ForwardChange >= Player.zAxis)
                    transform.Translate(0, 0, -Player.ForwardChange * Time.deltaTime / 2, Space.World);
                else if (CameraTransformation.CameraMode == CameraView.Orthoraphic &&
                    Player.transform.position.z >= Player.activeTrack.renderer.bounds.min.z)
                    transform.Translate(0, 0, -Player.ForwardChange * Time.deltaTime / 2, Space.World);
            }
    }

    private void zRotation(float zRotation)
    {
        if (Input.GetButton("Horizontal"))
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (transform.rotation.z >= -Player.RotationLimit)
                    Camera.main.transform.Rotate(0, 0, -zRotation * Time.deltaTime, Space.Self);
            }
            else
            {
                if (transform.rotation.z <= Player.RotationLimit)
                    Camera.main.transform.Rotate(0, 0, zRotation * Time.deltaTime, Space.Self);
            }
        else
        {
            if (Camera.main.transform.rotation.z < 0)
            {
                Camera.main.transform.Rotate(0, 0, zRotation * Time.deltaTime, Space.Self);
                if (Camera.main.transform.rotation.z > 0)
                    transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
            }
            else if (Camera.main.transform.rotation.z > 0)
            {
                Camera.main.transform.Rotate(0, 0, -zRotation * Time.deltaTime, Space.Self);
                if (Camera.main.transform.rotation.z < 0)
                    transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
            }
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
                    transform.Translate(0, -yChange * Time.deltaTime, 0, Space.World);
                    Camera.main.transform.Rotate(-xRotation * Time.deltaTime, 0, 0, Space.Self);
                }
            }
            else
            {
                if (Player.transform.position.y <= Player.yAxis + Player.HeightLimit)
                {
                    transform.Translate(0, +yChange * Time.deltaTime, 0, Space.World);
                    Camera.main.transform.Rotate(xRotation * Time.deltaTime, 0, 0, Space.Self);
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
        transform.Translate(xChange * Time.deltaTime, 0, 0, Space.World);
        Player.transform.Translate(xChange * Time.deltaTime, 0, 0);
        Player.activeTrack.transform.Translate(xChange * Time.deltaTime, 0, 0);
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
            transform.position = new Vector3(OrthographicPosition.x, OrthographicPosition.y, OrthographicPosition.z);
        else
            transform.position = new Vector3(PerspectivePosition.x, PerspectivePosition.y, PerspectivePosition.z);
    }

    private void CameraToPers(float speed, float angle, float rotationSpeed)
    {
        bool endTest = true;
        if (transform.position.y >= PerspectivePosition.y)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0, Space.World);
            endTest = false;
        }
        if (transform.position.z >= PerspectivePosition.z)
        {
            transform.Translate(0, 0, -5 * speed * Time.deltaTime, Space.World);
            endTest = false;
        }
        if (Camera.main.transform.rotation.eulerAngles.x > p_xRoation)
        {
            Camera.main.transform.Rotate(-rotationSpeed * Time.deltaTime, 0, 0, Space.Self);
            endTest = false;
        }
        else
            Camera.main.transform.rotation.eulerAngles.Set(p_xRoation, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z);

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

    private void CameraToOrto(float speed, float angle, float rotationSpeed)
    {
        bool endTest = true;
        if (transform.position.y < OrthographicPosition.y)
        {
            transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
            endTest = false;
        }
        if (transform.position.z <= OrthographicPosition.z)
        {
            transform.Translate(0, 0, 5 * speed * Time.deltaTime, Space.World);
            endTest = false;
        }
        if (Camera.main.transform.rotation.eulerAngles.x < 90)
        {
            Camera.main.transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0, Space.Self);
            endTest = false;
        }
        else
            Camera.main.transform.rotation.eulerAngles.Set(90, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z);

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
