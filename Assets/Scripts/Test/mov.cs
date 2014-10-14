using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mov : MonoBehaviour
{

    public float Accelaration { get; private set; }
    public float yAxis { get; private set; }
    public float zAxis { get; private set; }
    private bool normalHeight, changeHeight;

    //only 3D
    public float HeightLimit = 50, ForwardLimit = 600;
    public float ForwardChange = 15, SideChange = 1;

    public float AccelerationLimit = 2, AccelerationChange = 0.2f;
    public float RotationLimit = 0.10f, RotationChange = 0.2f;

    public GameObject activeTrack;
    public static List<GameObject> tracks = new List<GameObject>();
    public static bool Pause = false;
    //public GameObject secondTrack;

    //fire things
    public GameObject projectile;
    public Vector3 projectileSpawn;

    public static int Score = 0;
    public UnityEngine.UI.Text ScoreText;

    // Use this for initialization
    void Start()
    {
        CameraTransformation.Player = this;
        Spawns.Player = this;
        moverPlatform.Player = this;
        yAxis = this.transform.position.y;
        zAxis = this.transform.position.z;
        Accelaration = 0;
        normalHeight = true;
        changeHeight = false;
    }

    void Update()
    {
        if (!Pause)
        {
            xAxisMove();
            yAxisMove();
            zAxisMove();
            fireObject();
            ScoreText.text = Score.ToString();
        }
    }

    public void SetRightActive()
    {
        foreach (var t in mov.tracks)
        {
            if (t != activeTrack && t.transform.position.x > activeTrack.transform.position.x)
            {
                activeTrack = t;
                return;
            }
        }
    }

    public void SetLeftActive()
    {
        foreach (var t in mov.tracks)
        {
            if (t != activeTrack && t.transform.position.x < activeTrack.transform.position.x)
            {
                activeTrack = t;
                return;
            }
        }
    }

    public void PauseGame(bool val)
    {
        Pause = val;
    }

    private void fireObject()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(projectile, new Vector3(this.transform.position.x,
                projectileSpawn.y + this.transform.position.y,
                projectileSpawn.z + this.transform.position.z), Quaternion.identity);
        }
    }

    [System.Obsolete("use yAxisMove")]
    private void yAxisMoveJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            normalHeight = !normalHeight;
            //if (normalHeight)
            //secondTrack.SetActive(false);
            changeHeight = true;
        }
        if (changeHeight)
        {
            if (normalHeight)
            {
                if (this.transform.position.y >= yAxis)
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 2, this.transform.position.z);
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                                        Camera.main.transform.position.y - 1, Camera.main.transform.position.z);
                    Camera.main.transform.Rotate(-0.05f, 0, 0);
                }
            }
            else
            {
                if (this.transform.position.y <= yAxis + HeightLimit)
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z);
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                        Camera.main.transform.position.y + 1, Camera.main.transform.position.z);
                    Camera.main.transform.Rotate(0.05f, 0, 0);
                }
                //else secondTrack.SetActive(true);
            }
        }

    }

    private void yAxisMove()
    {
        if (!Input.GetButton("Jump"))
        {
            if (this.transform.position.y >= yAxis)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 2, this.transform.position.z);
                this.transform.Rotate(0.2f, 0, 0);
            }
            else if (this.transform.rotation.x > 0)
            {
                this.transform.Rotate(-0.2f, 0, 0);
            }
        }
        else
        {
            if (this.transform.position.y <= yAxis + HeightLimit)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z);
                this.transform.Rotate(-0.2f, 0, 0);
            }
            else if (this.transform.rotation.x < 0)
            {
                this.transform.Rotate(0.2f, 0, 0);
            }
        }
    }

    private float xTrackMax()
    {
        float res = float.MinValue;
        foreach (var tr in tracks)
        {
            res = Mathf.Max(res, tr.renderer.bounds.max.x);
        }
        return res;
    }

    private float xTrackMin()
    {
        float res = float.MaxValue;
        foreach (var tr in tracks)
        {
            res = Mathf.Min(res, tr.renderer.bounds.min.x);
        }
        return res;
    }

    //remove oscilating
    private void xAxisMove()
    {
        if (Input.GetButton("Horizontal"))
            if (Input.GetAxis("Horizontal") > 0)
            {
                var where = new Vector3(this.transform.position.x + SideChange + Accelaration, this.transform.position.y, this.transform.position.z);
                if (xTrackMax() >= where.x + collider.bounds.size.x)
                {
                    this.transform.position = where;
                    if (Accelaration <= AccelerationLimit)
                        Accelaration += AccelerationChange;
                    if (this.transform.rotation.z >= -RotationLimit)
                    {
                        this.transform.Rotate(0, 0, -RotationChange);
                    }
                    if (transform.collider.bounds.max.x > activeTrack.renderer.bounds.max.x)
                    {
                        SetRightActive();
                        CameraTransformation.MoveCameraToTrack(activeTrack);
                    }
                }
            }
            else
            {
                var where = new Vector3(this.transform.position.x - SideChange + Accelaration, this.transform.position.y, this.transform.position.z);
                if (xTrackMin() <= where.x - collider.bounds.size.x)
                {
                    this.transform.position = where;
                    if (Accelaration >= -AccelerationLimit)
                        Accelaration -= AccelerationChange;
                    if (this.transform.rotation.z <= RotationLimit)
                    {
                        this.transform.Rotate(0, 0, RotationChange);
                    }
                    if (transform.collider.bounds.min.x < activeTrack.renderer.bounds.min.x)
                    {
                        SetLeftActive();
                        CameraTransformation.MoveCameraToTrack(activeTrack);
                    }
                }
            }
        else
        {
            BalanceModel();
        }
    }

    //remove oscilations
    private void BalanceModel()
    {
        if (Accelaration < 0)
        {
            Accelaration += AccelerationChange / 4;
            if (Accelaration > 0)
                Accelaration = 0;
            else
            {
                var where = new Vector3(this.transform.position.x + Accelaration, this.transform.position.y, this.transform.position.z);
                if (activeTrack.renderer.bounds.min.x <= where.x - collider.bounds.size.x)
                    this.transform.position = where;
            }
        }
        else if (Accelaration > 0)
        {
            Accelaration -= AccelerationChange / 4;
            if (Accelaration < 0)
                Accelaration = 0;
            else
            {
                var where = new Vector3(this.transform.position.x + Accelaration, this.transform.position.y, this.transform.position.z);
                if (activeTrack.renderer.bounds.max.x >= where.x + collider.bounds.size.x)
                    this.transform.position = where;
            }
        }
        if (this.transform.rotation.z < 0)
            this.transform.Rotate(0, 0, RotationChange);
        else if (this.transform.rotation.z > 0)
            this.transform.Rotate(0, 0, -RotationChange);
    }

    private void zAxisMove()
    {
        if (Input.GetButton("Vertical"))
            if (Input.GetAxis("Vertical") > 0)
            {
                if (CameraTransformation.activeView == CameraTransformation.CameraView.Perspective && this.transform.position.z + ForwardChange <= zAxis + ForwardLimit)
                    MoveInZ(1);
                else if (CameraTransformation.activeView == CameraTransformation.CameraView.Orthoraphic &&
                    this.transform.position.z <= activeTrack.renderer.bounds.max.z)
                    MoveInZ(1);
            }
            else
            {
                if (CameraTransformation.activeView == CameraTransformation.CameraView.Perspective && this.transform.position.z - ForwardChange >= zAxis)
                    MoveInZ(-1);
                else if (CameraTransformation.activeView == CameraTransformation.CameraView.Orthoraphic &&
                    this.transform.position.z >= activeTrack.renderer.bounds.min.z)
                    MoveInZ(-1);
            }
    }

    private void MoveInZ(float signum)
    {
        if (signum == 0)
            return;
        var hlp = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + ForwardChange * signum);
        this.transform.position = hlp;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
            Camera.main.transform.position.y, Camera.main.transform.position.z + (ForwardChange * signum) / 2);
    }
}
