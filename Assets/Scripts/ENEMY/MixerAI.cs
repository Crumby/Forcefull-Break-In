using UnityEngine;
using System.Collections;

public class MixerAI : MonoBehaviour
{

    public GameObject projectile;
    public GameObject normalCanon_1;
    public bool isHard = false;
    private EnemyMotion enemy;
    private float offset = Random.Range(100, 350), fireRotation = 0;

    // Use this for initialization
    void Start()
    {
        projectile.GetComponent<ProjectileMotion>().OwnerStats = GetComponent<Stats>();
        enemy = GetComponent<EnemyMotion>();
        enemy.roations = false;
    }

    private void Fire()
    {
        if (isHard && Random.Range(0, 100) % 40 == 0)
        {

            enemy.Fire(projectile, normalCanon_1.transform.position + new Vector3(0, 0, -collider.bounds.size.z), Vector3.Normalize(GameData.PlayerPossition.position - transform.position));
            if (fireRotation + 90 == 360)
                fireRotation = 360;
            else fireRotation = (fireRotation + 90) % 360;
        }
        else if (Random.Range(0, 1000) % 100 == 0)
        {
            enemy.Fire(projectile, normalCanon_1.transform.position + new Vector3(0, 0, -collider.bounds.size.z), Vector3.Normalize(GameData.PlayerPossition.position - transform.position));
            if (fireRotation + 90 == 360)
                fireRotation = 360;
            else fireRotation = (fireRotation + 90) % 360;
        }
    }

    private void ChangeHeight()
    {
        if (CameraTransformation.CameraMode == CameraView.Perspective)
        {
            if (Random.Range(0, 250) % 80 == 0)
            {
                if (GameData.originalHeight < GameData.PlayerPossition.position.y)
                    enemy.up = true;
                else
                    enemy.down = true;
            }
        }
    }

    private void ThinkMove()
    {
        if (!enemy.right && GameData.PlayerPossition.position.x + offset >= transform.position.x)
        {
            if (Random.Range(0, 100) % 35 == 0)
                if (!enemy.left)
                    enemy.left = true;
        }
        else enemy.left = false;
        if (!enemy.left && GameData.PlayerPossition.position.x - offset <= transform.position.x)
        {
            if (Random.Range(0, 100) % 35 == 0)
                if (!enemy.right)
                    enemy.right = true;
        }
        else enemy.right = false;
    }

    void Update()
    {
        if (!GameData.PauseGame)
        {
            if (transform.rotation.eulerAngles.y <= fireRotation)
                transform.Rotate(0, 5, 0, Space.Self);
            ThinkMove();
            Fire();
            ChangeHeight();
        }
    }
}
