using UnityEngine;
using System.Collections;

public class aiBoss3 : MonoBehaviour
{
    public basicEnemySystems smallCanon, bigCannon;
    public weaponBigBossCanon big_0, big_1, small_0;
    public weaponRocketsLauncherBoss small_1;
    [Range(0f, 250f)]
    public float speed, maxRotChange = 45;
    public Transform endPointL, endPointR;
    private int hpFreze = -1;
    private bool moveSwitch = false;
    private float rotHlp = 0;
    public GameObject explosion;
    public Transform where;

    // Use this for initialization
    void Start()
    {
        hpFreze = bigCannon.maxHealth;
        bigCannon.imortal = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame && gameData.inReach(transform.position))
        {
            var hlp = Quaternion.LookRotation(transform.position - gameData.playerPosition, Vector3.up);
            if (smallCanon.health > 0)
            {
                if (hlp.eulerAngles.y < 180)
                {
                    if (rotHlp < hlp.eulerAngles.y)
                        smallCanon.gameObject.transform.Rotate(new Vector3(0, 0, 1), 20 * Time.deltaTime, Space.Self);
                    else if (rotHlp > hlp.eulerAngles.y)
                        smallCanon.gameObject.transform.Rotate(new Vector3(0, 0, 1), -20 * Time.deltaTime, Space.Self);
                }
                else if (hlp.eulerAngles.y > 180)
                    if (rotHlp < hlp.eulerAngles.y)
                        smallCanon.gameObject.transform.Rotate(new Vector3(0, 0, 1), 20 * Time.deltaTime, Space.Self);
                    else if (rotHlp > hlp.eulerAngles.y)
                        smallCanon.gameObject.transform.Rotate(new Vector3(0, 0, 1), -20 * Time.deltaTime, Space.Self);
                rotHlp = hlp.eulerAngles.y;

                if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
                    small_0.Fire(gameData.playerPosition);
                if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
                    small_1.Fire(gameData.playerPosition);
            }
            if (bigCannon.health > 0)
            {
                var pl = gameData.playerPosition.x;
                if (pl <= transform.position.x + 50 && pl >= transform.position.x - 50)
                {
                    if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
                        big_0.Fire(gameData.playerPosition);
                    if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
                        big_1.Fire(gameData.playerPosition);
                }
            }
            if (smallCanon.health > 0 || bigCannon.health > 0)
            {
                if (moveSwitch)
                    if (transform.position.x < endPointR.position.x)
                        transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
                    else moveSwitch = !moveSwitch;
                else
                    if (transform.position.x > endPointL.position.x)
                        transform.Translate(0, 0, -speed * Time.deltaTime, Space.Self);
                    else moveSwitch = !moveSwitch;
            }
            if (hpFreze != -1 && smallCanon.health <= 0)
            {
                bigCannon.imortal = false;
                hpFreze = -1;
            }
            else if (smallCanon.health <= 0 && bigCannon.health <= 0)
            {
                if (gameData.gameEnded == 1)
                    Instantiate(explosion, where.position, Quaternion.identity);
                gameData.gameEnded++;
            }
        }
    }
}
