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

    public GameObject initTrack;
    public GameObject initFog;

    //fire things
    public GameObject projectile;
    public Vector3 projectileSpawn;
    public int CollisionDmg = 25;

    void Start()
    {
        GameData.InitData(this, initTrack, initFog);
        initFog = null;
        initTrack = null;
        projectile.GetComponent<ProjectileMotion>().OwnerStats = this.GetComponent<Stats>();
        CameraTransformation.Player = this;
        Spawns.Player = this;
        PlatformMotion.Player = this;
        yAxis = transform.position.y;
        zAxis = transform.position.z;
        Accelaration = 0;
    }

    void Update()
    {
        if (!GameData.PauseGame)
        {
            xAxisMove();
            HooverJump(200, 30);
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


    public void SetCurrentTrackActive()
    {
        if (transform.collider.bounds.max.x > GameData.ActiveTrack.renderer.bounds.max.x)
        {
            foreach (var t in GameData.Tracks)
            {
                if (t != GameData.ActiveTrack && t.transform.position.x > GameData.ActiveTrack.transform.position.x)
                {
                    GameData.ActiveTrack = t;
                    break;
                }
            }
        }
        else if (transform.collider.bounds.min.x < GameData.ActiveTrack.renderer.bounds.min.x)
        {
            foreach (var t in GameData.Tracks)
            {
                if (t != GameData.ActiveTrack && t.transform.position.x < GameData.ActiveTrack.transform.position.x)
                {
                    GameData.ActiveTrack = t;
                    break;
                }
            }
        }
    }

    [System.Obsolete("Redooo")]
    public void PauseGame(bool val)
    {
        GameData.PauseGame = val;
    }

    private void fireObject()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var hlp = (GameObject)Instantiate(projectile, new Vector3(transform.position.x,
                transform.position.y,
                projectileSpawn.z + transform.position.z), Quaternion.identity);
            hlp.transform.parent = GameData.ActiveTrack.transform;
        }
    }

    private void PlatformJump(float zChange)
    {
        if (CameraTransformation.CameraMode == CameraView.Perspective)
            if (Input.GetButtonDown("Jump"))
            {
                normalHeight = !normalHeight;
                if (normalHeight)
                    GameData.Fog.SetActive(false);
                changeHeight = true;
            }
        if (changeHeight)
        {
            if (normalHeight)
            {
                if (transform.position.y >= yAxis)
                    transform.Translate(0, -zChange * Time.deltaTime, 0, Space.World);
            }
            else
            {
                if (transform.position.y <= yAxis + HeightLimit)
                    transform.Translate(0, zChange * Time.deltaTime, 0, Space.World);
                else GameData.Fog.SetActive(true);
            }
        }

    }

    private void HooverJump(float zChange, float xRotation)
    {
        if (CameraTransformation.CameraMode == CameraView.Perspective)
            if (!Input.GetButton("Jump"))
            {
                if (transform.position.y >= yAxis)
                {
                    normalHeight = true;
                    transform.Translate(0, -zChange * Time.deltaTime, 0, Space.World);
                    transform.Rotate(xRotation * Time.deltaTime, 0, 0, Space.Self);
                }
                else if (transform.rotation.x > 0)
                {
                    transform.Rotate(-xRotation * Time.deltaTime, 0, 0, Space.Self);
                }
            }
            else
            {
                if (transform.position.y <= yAxis + HeightLimit)
                {
                    transform.Translate(0, zChange * Time.deltaTime, 0, Space.World);
                    transform.Rotate(-xRotation * Time.deltaTime, 0, 0, Space.Self);
                }
                else if (transform.rotation.x < 0)
                {
                    normalHeight = false;
                    GameData.Fog.SetActive(true);
                    transform.Rotate(xRotation * Time.deltaTime, 0, 0, Space.Self);
                }
            }
        if (normalHeight)
            GameData.Fog.SetActive(false);
    }

    private void GravityJump(float zChange)
    {
        if (CameraTransformation.CameraMode == CameraView.Perspective)
        {
            PlatformJump(zChange);
            //split ...
            if (!changeHeight)
                PlatformJump(zChange);
        }
    }

    private void xAxisMove()
    {
        if (Input.GetButton("Horizontal"))
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (GameData.MaxTracksX >= transform.position.x + (SideChange + Accelaration) * Time.deltaTime + collider.bounds.size.x)
                {
                    if (transform.rotation.z <= 0)
                    {
                        transform.Translate(SideChange * Time.deltaTime, 0, 0, Space.World);
                        if (Accelaration <= AccelerationLimit)
                            Accelaration += AccelerationChange;
                    }
                    else BalanceModel();
                    if (transform.rotation.z > -RotationLimit)
                        transform.Rotate(0, 0, -RotationChange * Time.deltaTime, Space.Self);
                }
            }
            else
            {
                if (GameData.MinTracksX <= transform.position.x + (-SideChange + Accelaration) * Time.deltaTime - collider.bounds.size.x)
                {
                    if (transform.rotation.z >= 0)
                    {
                        transform.Translate(-SideChange * Time.deltaTime, 0, 0, Space.World);
                        if (Accelaration >= -AccelerationLimit)
                            Accelaration -= AccelerationChange;
                    }
                    else BalanceModel();
                    if (transform.rotation.z < RotationLimit)
                        transform.Rotate(0, 0, RotationChange * Time.deltaTime, Space.Self);
                }
            }
        else
            BalanceModel();
    }

    private void BalanceModel()
    {
        if (Accelaration < 0)
        {
            Accelaration += AccelerationChange / 8;
            if (Accelaration > 0)
                Accelaration = 0;
            else
                if (GameData.ActiveTrack.renderer.bounds.min.x <= transform.position.x + Accelaration * Time.deltaTime - collider.bounds.size.x)
                    transform.Translate(Accelaration * Time.deltaTime, 0, 0, Space.World);
        }
        else if (Accelaration > 0)
        {
            Accelaration -= AccelerationChange / 8;
            if (Accelaration < 0)
                Accelaration = 0;
            else
                if (GameData.ActiveTrack.renderer.bounds.max.x >= transform.position.x + Accelaration * Time.deltaTime + collider.bounds.size.x)
                    transform.Translate(Accelaration * Time.deltaTime, 0, 0, Space.World);
        }
        if (transform.rotation.z != 0)
        {
            if (this.transform.rotation.z > 0)
            {
                transform.Rotate(0, 0, -RotationChange * Time.deltaTime / 2, Space.Self);
                if (this.transform.rotation.z < 0)
                    transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
            }
            else if (this.transform.rotation.z < 0)
            {
                transform.Rotate(0, 0, RotationChange * Time.deltaTime / 2, Space.Self);
                if (this.transform.rotation.z > 0)
                    transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
            }
        }
    }

    private void zAxisMove()
    {
        if (Input.GetButton("Vertical"))
            if (Input.GetAxis("Vertical") > 0)
            {
                if (CameraTransformation.CameraMode == CameraView.Perspective && transform.position.z + ForwardChange * Time.deltaTime <= zAxis + ForwardLimit)
                    transform.Translate(0, 0, ForwardChange * Time.deltaTime, Space.World);
                else if (CameraTransformation.CameraMode == CameraView.Orthoraphic &&
                    Camera.main.WorldToScreenPoint(transform.renderer.bounds.max).y <= Camera.main.pixelHeight)
                    transform.Translate(0, 0, ForwardChange * Time.deltaTime, Space.World);
            }
            else
            {
                if (CameraTransformation.CameraMode == CameraView.Perspective && transform.position.z - ForwardChange * Time.deltaTime >= zAxis)
                    transform.Translate(0, 0, -ForwardChange * Time.deltaTime, Space.World);
                else if (CameraTransformation.CameraMode == CameraView.Orthoraphic &&
                    Camera.main.WorldToScreenPoint(transform.renderer.bounds.min).y >= 0)
                    transform.Translate(0, 0, -ForwardChange * Time.deltaTime, Space.World);
            }
    }
}
