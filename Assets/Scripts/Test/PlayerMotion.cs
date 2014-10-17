using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMotion : MonoBehaviour
{

    public float Accelaration { get; private set; }
    public float yAxis { get; private set; }
    public float zAxis { get; private set; }
    private bool normalHeight = true, changeHeight = false;

    //only 3D
    public float HeightLimit, ForwardLimit;
    public float ForwardChange, SideChange;

    public float AccelerationLimit, AccelerationChange;
    public float RotationLimit, RotationChange;

    public GameObject activeTrack;
    public static List<GameObject> Tracks = new List<GameObject>();
    public float MaxTracksX
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
    public float MinTracksX
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
    //redo
    private float signBM = 0;

    public static bool Pause;
    public GameObject Fog;

    //fire things
    public GameObject projectile;
    public Vector3 projectileSpawn;
    public int CollisionDmg = 25;

    void Start()
    {
        Tracks.Clear();
        Pause = false;
        projectile.GetComponent<ProjectileMotion>().OwnerStats = this.GetComponent<Stats>();
        CameraTransformation.Player = this;
        Spawns.Player = this;
        PlatformMotion.Player = this;
        yAxis = this.transform.position.y;
        zAxis = this.transform.position.z;
        Accelaration = 0;
    }

    void Update()
    {
        if (!Pause)
        {
            xAxisMove();
            HooverJump();
            zAxisMove();
            fireObject();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyMotion>() != null)
        {
            var stats = collision.gameObject.GetComponent<Stats>();
            if (stats != null)
            {
                if (stats.Shield - CollisionDmg <= 0)
                {
                    stats.Hp = stats.Hp + stats.Shield - CollisionDmg;
                    stats.Shield = 0;
                }
                else
                {
                    stats.Shield = stats.Shield - CollisionDmg;
                }
                if (stats.Hp <= 0)
                {
                    this.GetComponent<Stats>().Score += stats.Score;
                    Destroy(collision.gameObject);
                }
            }
        }
        else
            Destroy(collision.gameObject);
    }

    void OnDestroy()
    {
        GameObject.Find("Canvas").GetComponent<InGameMenu>().GameOver();
    }

    public void SetCurrentTrackActive()
    {
        if (transform.collider.bounds.max.x > activeTrack.renderer.bounds.max.x)
        {
            foreach (var t in PlayerMotion.Tracks)
            {
                if (t != activeTrack && t.transform.position.x > activeTrack.transform.position.x)
                {
                    activeTrack = t;
                    break;
                }
            }
            //CameraTransformation.MoveCameraToTrack(activeTrack);
        }
        else if (transform.collider.bounds.min.x < activeTrack.renderer.bounds.min.x)
        {
            foreach (var t in PlayerMotion.Tracks)
            {
                if (t != activeTrack && t.transform.position.x < activeTrack.transform.position.x)
                {
                    activeTrack = t;
                    break;
                }
            }
            //CameraTransformation.MoveCameraToTrack(activeTrack);
        }
    }

    [System.Obsolete("Redooo")]
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


    private void PlatformJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            normalHeight = !normalHeight;
            if (normalHeight)
                Fog.SetActive(false);
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
                else Fog.SetActive(true);
            }
        }

    }

    private void HooverJump()
    {
        if (!Input.GetButton("Jump"))
        {
            if (this.transform.position.y >= yAxis)
            {
                normalHeight = true;
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
                normalHeight = false;
                Fog.SetActive(true);
                this.transform.Rotate(0.2f, 0, 0);
            }
        }
        if (normalHeight)
            Fog.SetActive(false);
    }

    //not implemented
    private void GravityJump()
    {
        PlatformJump();
        //split ...
        if (!changeHeight)
            PlatformJump();
    }

    private void xAxisMove()
    {
        if (Input.GetButton("Horizontal"))
            if (Input.GetAxis("Horizontal") > 0)
            {
                var where = new Vector3(this.transform.position.x + SideChange + Accelaration, this.transform.position.y, this.transform.position.z);
                if (MaxTracksX >= where.x + collider.bounds.size.x)
                {
                    this.transform.position = where;
                    if (Accelaration <= AccelerationLimit)
                        Accelaration += AccelerationChange;
                    if (this.transform.rotation.z >= -RotationLimit)
                    {
                        this.transform.Rotate(0, 0, -RotationChange);
                    }
                }
            }
            else
            {
                var where = new Vector3(this.transform.position.x - SideChange + Accelaration, this.transform.position.y, this.transform.position.z);
                if (MinTracksX <= where.x - collider.bounds.size.x)
                {
                    this.transform.position = where;
                    if (Accelaration >= -AccelerationLimit)
                        Accelaration -= AccelerationChange;
                    if (this.transform.rotation.z <= RotationLimit)
                    {
                        this.transform.Rotate(0, 0, RotationChange);
                    }
                }
            }
        else
        {
            BalanceModel();
        }
    }

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
        if (transform.rotation.z != 0)
        {
            float xChange = RotationChange;
            if (this.transform.rotation.z > 0)
                xChange = -xChange;
            if (signBM == 0)
                signBM = Mathf.Sign(xChange);
            if (signBM != Mathf.Sign(xChange))
            {
                signBM = 0;
                this.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0,
                    transform.rotation.w);
                return;
            }
            this.transform.Rotate(0, 0, xChange);
        }
    }

    private void zAxisMove()
    {
        if (Input.GetButton("Vertical"))
            if (Input.GetAxis("Vertical") > 0)
            {
                if (CameraTransformation.CameraMode == CameraView.Perspective && this.transform.position.z + ForwardChange <= zAxis + ForwardLimit)
                    MoveInZ(1);
                else if (CameraTransformation.CameraMode == CameraView.Orthoraphic &&
                    this.transform.position.z <= activeTrack.renderer.bounds.max.z)
                    MoveInZ(1);
            }
            else
            {
                if (CameraTransformation.CameraMode == CameraView.Perspective && this.transform.position.z - ForwardChange >= zAxis)
                    MoveInZ(-1);
                else if (CameraTransformation.CameraMode == CameraView.Orthoraphic &&
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
