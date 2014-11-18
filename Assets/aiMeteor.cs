using UnityEngine;
using System.Collections;

public class aiMeteor : MonoBehaviour
{

    [Range(0, 100F)]
    public float speed;
    private Vector3 moving;
    private float hp;
    public GameObject smalExplosion, bugexplosion;

    // Use this for initialization
    void Start()
    {
        moving = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
        moving = Vector3.Normalize(moving);
        transform.localScale.Scale(new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5)));
        hp = transform.localScale.x + transform.localScale.y + transform.localScale.z;
        hp = hp * Random.Range(0, 10);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<shipSystemsEnemy>() != null)
        {
            var tmp = (shipSystemsEnemy)collision.gameObject.GetComponent<shipSystemsEnemy>();
            tmp.recieveDmg(hp, tmp.transform.position);
        }
        else if (collision.gameObject.GetComponent<motionPlayer>() != null)
        {
            var tmp = (shipSystemsPlayer)collision.gameObject.GetComponent<shipSystemsPlayer>();
            tmp.recieveDmg(hp, tmp.transform.position);
        }
    }

    private void Destroy()
    {
        Instantiate(bugexplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void recieveDmg(float dmg, Vector3 where)
    {
        Instantiate(smalExplosion, where, Quaternion.identity);
        hp -= dmg;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < gameData.cameraOffsite.z + gameData.playerPosition.z)
            DestroyObject(gameObject);
        if (!gameData.pausedGame)
        {
            transform.Translate(moving * speed * Time.deltaTime);
        }
    }
}
