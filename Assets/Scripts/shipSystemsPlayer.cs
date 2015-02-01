using UnityEngine;
using System.Collections;

public class shipSystemsPlayer : MonoBehaviour
{
    [Range(0.0F, 5.0F)]
    public float shieldRegen;
    [Range(0.0F, 500.0F)]
    public float collisionDmg, powerDrain;
    [Range(0.0F, 500.0F)]
    public int maxHealth, maxShield, maxPower;
    public float Health { get; private set; }
    public float Shield { get; private set; }
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
    public inGameMenu menus;
    [HideInInspector]
    public bool idkfa = false, noShield=false;

    // Use this for initialization
    void Start()
    {
        Health = maxHealth+gameData.bonusHP;
        Shield = maxShield+gameData.bonusShields;
        Power = 0;
    }

    private void powerDraining()
    {
        if (Power > 0)
        {
            Power -= powerDrain/gameData.ultiDerease * Time.deltaTime;
            if (Power < 0)
                Power = 0;
            if (Power > maxPower)
                powerTexture.rectTransform.localScale = Vector3.one;
            else powerTexture.rectTransform.localScale = new Vector3(Power / maxPower, 1, 1);

        }
    }

    private void shieldRegeneration()
    {
        if (Shield < maxShield && !noShield)
        {
            if ((shieldRegen + gameData.bonusShieldRegen) * Time.deltaTime + Shield > maxShield)
                Shield = maxShield;
            else Shield += (shieldRegen +gameData.bonusShieldRegen)* Time.deltaTime;
            shieldTexture.rectTransform.localScale = new Vector3(shieldTexture.rectTransform.localScale.x,
                Shield / (float)maxShield, shieldTexture.rectTransform.localScale.z);
            shieldText.text = Mathf.CeilToInt(Shield).ToString();
        }
        else if (noShield && shieldText.text != "0")
        {
            Shield = 0;
            shieldText.text = Shield.ToString();
            shieldTexture.rectTransform.localScale = new Vector3(shieldTexture.rectTransform.localScale.x,
                Shield / (float)maxShield, shieldTexture.rectTransform.localScale.z);
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
                    rocket.Fire(gameData.aimNavigation, Vector3.forward, true);
                    break;
                }
            }
    }

    public bool recieveDmg(float dmg, Vector3 where)
    {
        Instantiate(smallExplosion, where, Quaternion.identity);
        if (!idkfa)
        {
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
        }
        return false;
    }

    private void destroyShip()
    {
        Instantiate(bigExplosion, transform.position, Quaternion.identity);
        gameData.pausedGame = true;
        menus.showMenu();
        menus.showGameOver();
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
        if (Power >= maxPower)
        {
            Instantiate(powerWeapon, gameData.playerPosition, Quaternion.identity);
            Power = 0;
            powerTexture.rectTransform.localScale = new Vector3(Power / maxPower, 1, 1);
        }
    }

    private void fireSecondary()
    {
        railGun.Fire();
    }
}
