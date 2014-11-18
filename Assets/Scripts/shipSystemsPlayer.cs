using UnityEngine;
using System.Collections;

public class shipSystemsPlayer : MonoBehaviour
{
    [Range(0.0F, 500.0F)]
    public float shieldRegen, collisionDmg, powerDrain;
    [Range(0.0F, 500.0F)]
    public int maxHealth, maxShield, maxPower;
    public int Health { get; private set; }
    public int Shield { get; private set; }
    public float Power { get; set; }
    public int Score
    {
        get { return score; }
        set { score = value; scoreText.text = score.ToString(); }
    }
    private int score;
    public UnityEngine.UI.RawImage healthTexture, shieldTexture, powerTexture;
    public UnityEngine.UI.Text healthText, shieldText, scoreText;
    public weaponRocketLaucher[] Rockets;
    public weaponRailGun railGun;
    public GameObject powerWeapon;
    public GameObject smallExplosion, bigExplosion, shieldField;

    // Use this for initialization
    void Start()
    {
        Health = maxHealth;
        Shield = maxShield;
        Power = 0;
    }

    private void powerDraining()
    {
        if (Power > 0)
        {
            Power -= powerDrain * Time.deltaTime;
            if (Power < 0)
                Power = 0;
            if (Power > maxPower)
                powerTexture.rectTransform.localScale = Vector3.one;
            else powerTexture.rectTransform.localScale = new Vector3(Power / maxPower, 1, 1);

        }
    }

    private void shieldRegeneration()
    {
        if (Shield < maxShield)
        {
            if (Mathf.CeilToInt(shieldRegen * Time.deltaTime) + Shield > maxShield)
                Shield = maxShield;
            else Shield += Mathf.CeilToInt(shieldRegen * Time.deltaTime);
            shieldTexture.rectTransform.localScale = new Vector3(shieldTexture.rectTransform.localScale.x,
                Shield / (float)maxShield, shieldTexture.rectTransform.localScale.z);
            shieldText.text = Shield.ToString();
        }

    }

    public void firePrimary()
    {
        int charg = 0;
        for (int i = 0; i < Rockets.Length; i++)
        {
            if (Rockets[i].charged)
                charg++;
        }
        if (charg == Rockets.Length)
            Rockets[Random.Range(0, 153) % Rockets.Length].Fire(gameData.aimNavigation, Vector3.forward, true);
        else
            foreach (var rocket in Rockets)
            {
                if (rocket.charged)
                {
                    //rocket.destination = gameData.aimPoint;
                    rocket.Fire(gameData.aimNavigation, Vector3.forward, true);
                    break;
                }
            }
    }

    public bool recieveDmg(float dmg, Vector3 where)
    {
        Instantiate(smallExplosion, where, Quaternion.identity);
        if (Shield - dmg <= 0)
        {
            Shield = 0;
            shieldTexture.rectTransform.localScale = new Vector3(shieldTexture.rectTransform.localScale.x,
                Shield / (float)maxShield, shieldTexture.rectTransform.localScale.z);
            shieldText.text = Shield.ToString();
            Health += Mathf.CeilToInt(Shield - dmg);
            if (Health <= 0) Health = 0;
            healthTexture.rectTransform.localScale = new Vector3(healthTexture.rectTransform.localScale.x,
                Health / (float)maxHealth, healthTexture.rectTransform.localScale.z);
            healthText.text = Health.ToString();
            return Health == 0;
        }
        else Shield -= Mathf.CeilToInt(dmg);
        shieldText.text = Shield.ToString();
        shieldTexture.rectTransform.localScale = new Vector3(shieldTexture.rectTransform.localScale.x,
                Shield / (float)maxShield, shieldTexture.rectTransform.localScale.z);
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
        else if (collision.gameObject.GetComponent<TerrainCollider>() != null) destroyShip();
    }


    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            powerDraining();
            shieldRegeneration();
            if (Health <= 0) destroyShip();
            if (Input.GetButtonDown("Fire1")) firePrimary();
            if (Input.GetButton("Fire2")) fireSecondary();
            if (Input.GetButton("Fire3")) fireUlti();
            if (Shield <= 0) shieldField.SetActive(false);
            else if (!shieldField.activeInHierarchy) shieldField.SetActive(true);

        }
    }
    private void fireUlti()
    {
        if (Power >= maxPower) {
            Instantiate(powerWeapon, gameData.playerPosition, Quaternion.identity);
            Power = 0;
        }
    }

    private void fireSecondary()
    {
        railGun.Fire();
    }
}
