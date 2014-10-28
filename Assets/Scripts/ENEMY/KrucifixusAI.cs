using UnityEngine;
using System.Collections;

public class KrucifixusAI : MonoBehaviour
{

    public GameObject projectile;
    public GameObject normalCanon_1, normalCanon_2;
    public GameObject hardCanon_1, hardCanon_2;
    public bool isHard = false;
    private EnemyMotion enemy;
    private float offset = Random.Range(100, 350);

    // Use this for initialization
    void Start()
    {
        projectile.GetComponent<ProjectileMotion>().OwnerStats = GetComponent<Stats>();
        enemy = GetComponent<EnemyMotion>();
    }

    private void Fire()
    {
        if (Random.Range(0, 1000) % 100 == 0)
            if (Random.Range(0, 53) % 2 == 0)
                enemy.Fire(projectile, normalCanon_1.transform.position + new Vector3(0, 0, -collider.bounds.size.z), Vector3.back);
            else
                enemy.Fire(projectile, normalCanon_2.transform.position + new Vector3(0, 0, -collider.bounds.size.z), Vector3.back);

        if (isHard)
        {
            if (Random.Range(0, 1000) % 80 == 0)
                if (Random.Range(0, 53) % 2 == 0)
                    enemy.Fire(projectile, normalCanon_1.transform.position + new Vector3(0, 0, -collider.bounds.size.z * 0.5f), Vector3.back);
                else
                    enemy.Fire(projectile, normalCanon_2.transform.position + new Vector3(0, 0, -collider.bounds.size.z * 0.5f), Vector3.back);
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
            if (Random.Range(0, 100) % 10 == 0)
                if (!enemy.left)
                    enemy.left = true;
        }
        else enemy.left = false;
        if (!enemy.left && GameData.PlayerPossition.position.x - offset <= transform.position.x)
        {
            if (Random.Range(0, 100) % 10 == 0)
                if (!enemy.right)
                    enemy.right = true;
        }
        else enemy.right = false;
    }

    void Update()
    {
        if (!GameData.PauseGame)
        {
            ThinkMove();
            Fire();
            ChangeHeight();
        }
    }
}
