using UnityEngine;
using System.Collections;

public enum typeOfBonus { SPEEDUP, SPEEDDOWN, IDKFA, NOSHIELD, REVERSECONSTROL, SCOREBONUS }

public class bonusDrops : MonoBehaviour
{
    [Range(0.5f, 50f)]
    public float duration;
    public typeOfBonus type;
    private float timer = 0;
    public bool activated { get; private set; }
    // Use this for initialization
    void Start()
    {
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            if (activated)
                timer += Time.fixedDeltaTime;
            else if (Random.Range(0, 5) <= 3)
            {
                if (type == typeOfBonus.SCOREBONUS)
                    type = typeOfBonus.SPEEDUP;
                else
                    type++;
            }
            if (timer >= duration)
                DeActivate();
            if (gameData.gameBounds != null)
                if (transform.position.x > gameData.gameBounds.collider.bounds.max.x) Destroy(gameObject);
                else if (transform.position.x < gameData.gameBounds.collider.bounds.min.x) Destroy(gameObject);
                else if (transform.position.y > gameData.gameBounds.collider.bounds.max.y) Destroy(gameObject);
                else if (transform.position.y < gameData.gameBounds.collider.bounds.min.y) Destroy(gameObject);
                else if (transform.position.z > gameData.playerPosition.z + gameData.aiActivation) Destroy(gameObject);
                else if (transform.position.z <= gameData.cameraOffsite.z + gameData.playerPosition.z) Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var tmp = other.gameObject.GetComponent<motionPlayer>();
        if (tmp != null)
        {
            transform.position = tmp.gameObject.transform.position;
            transform.parent = tmp.gameObject.transform;
            renderer.enabled = false;
            Activate();
        }
    }

    public void Activate()
    {
        switch (type)
        {
            case typeOfBonus.SPEEDUP:
                Time.timeScale *= 2;
                break;
            case typeOfBonus.SPEEDDOWN:
                Time.timeScale *= 0.5f;
                break;
            case typeOfBonus.IDKFA:
                gameData.playerSystems.noShield = false;
                gameData.playerSystems.idkfa = true;
                break;
            case typeOfBonus.NOSHIELD:
                gameData.playerSystems.idkfa = false;
                gameData.playerSystems.noShield = true;
                break;
            case typeOfBonus.REVERSECONSTROL:
                gameData.playerMotion.movementReverse *= -1;
                break;
            case typeOfBonus.SCOREBONUS:
                gameData.ScoreMultiplier *= 2;
                break;
        }
        activated = true;
        gameData.nonStatic.bonusPopup = type.ToString();
        this.audio.Play();
    }

    public void DeActivate()
    {
        switch (type)
        {
            case typeOfBonus.SPEEDUP:
                Time.timeScale *= 0.5f;
                break;
            case typeOfBonus.SPEEDDOWN:
                Time.timeScale *= 2;
                break;
            case typeOfBonus.IDKFA:
                gameData.playerSystems.idkfa = false;
                break;
            case typeOfBonus.NOSHIELD:
                gameData.playerSystems.noShield = false;
                break;
            case typeOfBonus.REVERSECONSTROL:
                gameData.playerMotion.movementReverse *= -1;
                break;
            case typeOfBonus.SCOREBONUS:
                gameData.ScoreMultiplier /= 2;
                break;
        }
        activated = false;
        if (gameData.nonStatic.bonusPopup == type.ToString()) gameData.nonStatic.bonusPopup = "";
        Destroy(gameObject);
    }
}
