using UnityEngine;
using System.Collections;

public class aiBoss2Cannon : MonoBehaviour
{

    public float fireSpeed;
    public GameObject projectile;
    public GameObject blast;
    public Transform[] where;
    private float timer = 0;
    private int cannon = 0;

    public void Fire(Vector3 to)
    {
        if (timer >= fireSpeed / (float)gameData.difficulty && where.Length > 0 && Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
        {
            cannon++;
            if (cannon >= where.Length)
                cannon = 0;
            Instantiate(blast, where[cannon].position, Quaternion.identity);
            var tmp_1 = (GameObject)Instantiate(projectile, where[cannon].position, Quaternion.identity);
            var tmp = tmp_1.GetComponent<motionProjectile>();
            tmp.transform.LookAt(to, Vector3.up);
            tmp.launch = true;            
        }
        else timer += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame && gameData.inReach(transform.position) && GetComponent<basicEnemySystems>() != null && GetComponent<basicEnemySystems>().health > 0)
        {
            Fire(gameData.playerPosition);
        }
    }
}
