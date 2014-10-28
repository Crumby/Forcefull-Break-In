using UnityEngine;
using System.Collections;

public class EnemyMotion : MonoBehaviour
{
    public float ForwardSpeed, xStep = 500, zRot = 2;
    public int CollisionDmg = 25;
    public bool left = false, right = false, roations = true, up = false, down = false;

    void Start()
    {
    }

    public void Fire(GameObject projectile, Vector3 projectileSpawn,Vector3 pointTo)
    {
        var hlp = (GameObject)Instantiate(projectile, new Vector3(transform.position.x,
            transform.position.y,
            projectileSpawn.z), Quaternion.identity);
        hlp.transform.parent = transform.parent;
        hlp.GetComponent<ProjectileMotion>().forward = pointTo;
    }

    private void MoveLeft()
    {
        if (collider.bounds.max.x + xStep * Time.deltaTime <= GameData.ActiveTrack.renderer.bounds.max.x)
        {
            transform.Translate(xStep * Time.deltaTime, 0, 0, Space.World);
            if (roations && transform.rotation.z > -0.1)
            {
                transform.Rotate(0, 0, -zRot, Space.Self);
            }
        }
        else Balance();
    }

    private void MoveRight()
    {
        if (collider.bounds.min.x - xStep * Time.deltaTime >= GameData.ActiveTrack.renderer.bounds.min.x)
        {
            transform.Translate(-xStep * Time.deltaTime, 0, 0, Space.World);
            if (roations && transform.rotation.z < 0.1)
            {
                transform.Rotate(0, 0, zRot, Space.Self);
            }
        }
        else Balance();
    }

    private void Balance()
    {
        if (roations)
        {
            if (transform.rotation.z < 0)
            {
                transform.Rotate(0, 0, zRot, Space.Self);
                if (transform.rotation.z > 0)
                    transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
            }
            else if (transform.rotation.z > 0)
            {
                transform.Rotate(0, 0, -zRot, Space.Self);
                if (transform.rotation.z < 0)
                    transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
            }
        }
    }

    private void MoveUp()
    {
        if (transform.position.y <= GameData.originalHeight + GameData.maxHeight)
        {
            transform.Translate(0, xStep * Time.deltaTime, 0, Space.World);
            if (roations)
                transform.Rotate(-zRot * Time.deltaTime, 0, 0, Space.Self);
        }
        else if (transform.rotation.x < 0)
        {
            up = false;
            if (roations)
                transform.Rotate(zRot * Time.deltaTime, 0, 0, Space.Self);
        }
    }

    private void MoveDown()
    {
        if (transform.position.y >= GameData.originalHeight)
        {
            transform.Translate(0, -xStep * Time.deltaTime, 0, Space.World);
            if (roations)
                transform.Rotate(zRot * Time.deltaTime, 0, 0, Space.Self);
        }
        else if (transform.rotation.x > 0)
        {
            down = false;
            if (roations)
                transform.Rotate(-zRot * Time.deltaTime, 0, 0, Space.Self);
        }
    }

    void Update()
    {
        if (!GameData.PauseGame)
        {
            transform.Translate(Vector3.back * ForwardSpeed * Time.deltaTime, Space.World);
            if (left)
                MoveLeft();
            else if (right)
                MoveRight();
            else Balance();
            if (up)
                MoveUp();
            else if (down)
                MoveDown();

            if (transform.position.z < -400)
                Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerMotion>() != null)
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
                    Destroy(collision.gameObject);
                }
            }
        }
        else
            if (collision.gameObject.GetComponent<PlayerMotion>() != null)
                GameObject.Find("Canvas").GetComponent<InGameMenu>().GameOver();
            else
                Destroy(collision.gameObject);
    }

}
