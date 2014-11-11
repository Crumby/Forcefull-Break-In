using UnityEngine;
using System.Collections;

public class shipSystemsPlayer : MonoBehaviour
{
    [Range(0.0F, 500.0F)]
    public float shieldRegen, collisionDmg;
    [Range(0.0F, 500.0F)]
    public int maxHealth, maxShield;
    public int health { get; private set; }
    public int shield { get; private set; }
    public int Score
    {
        get { return score; }
        set { score = value; scoreText.text = score.ToString(); }
    }
    private int score;
    public UnityEngine.UI.RawImage healthTexture, shieldTexture;
    public UnityEngine.UI.Text healthText, shieldText, scoreText;
    public weaponRocketLaucher weaponModel;
    public GameObject smallExplosion, bigExplosion, shieldField;

    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        shield = maxShield;
    }

    private void shieldRegeneration()
    {
        if (shield < maxShield)
        {
            if (Mathf.CeilToInt(shieldRegen * Time.deltaTime) + shield > maxShield)
                shield = maxShield;
            else shield += Mathf.CeilToInt(shieldRegen * Time.deltaTime);
            shieldTexture.rectTransform.localScale = new Vector3(shieldTexture.rectTransform.localScale.x,
                shield / (float)maxShield, shieldTexture.rectTransform.localScale.z);
            shieldText.text = shield.ToString();
        }

    }

    public void fire()
    {
        weaponModel.Fire(Vector3.forward, true);
    }

    public bool recieveDmg(float dmg, Vector3 where)
    {
        Instantiate(smallExplosion, where, Quaternion.identity);
        if (shield - dmg <= 0)
        {
            shield = 0;
            shieldTexture.rectTransform.localScale = new Vector3(shieldTexture.rectTransform.localScale.x,
                shield / (float)maxShield, shieldTexture.rectTransform.localScale.z);
            shieldText.text = shield.ToString();
            health += Mathf.CeilToInt(shield - dmg);
            healthTexture.rectTransform.localScale = new Vector3(healthTexture.rectTransform.localScale.x,
                health / (float)maxHealth, healthTexture.rectTransform.localScale.z);
            healthText.text = health.ToString();
            return health <= 0;
        }
        else shield -= Mathf.CeilToInt(dmg);
        shieldText.text = shield.ToString();
        shieldTexture.rectTransform.localScale = new Vector3(shieldTexture.rectTransform.localScale.x,
                shield / (float)maxShield, shieldTexture.rectTransform.localScale.z);
        return false;
    }

    private void destroyShip()
    {
        Instantiate(bigExplosion, transform.position, Quaternion.identity);
        gameData.pausedGame = true;
        DestroyObject(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<shipSystemsEnemy>();
        if (enemy != null)
        {
            if (enemy.recieveDmg(collisionDmg, collision.contacts[0].point))
            {
                Score += enemy.score;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            shieldRegeneration();
            if (health <= 0) destroyShip();
            if (Input.GetButtonDown("Fire1")) fire();
            if (shield <= 0) shieldField.SetActive(false);
            else if (!shieldField.activeInHierarchy) shieldField.SetActive(true);

        }
    }
}
