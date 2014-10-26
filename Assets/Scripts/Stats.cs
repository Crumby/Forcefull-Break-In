using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour
{

    public float MaxHp, MaxShield, ShieldRegeneration;
    public float Hp
    {
        get { return hp; }
        set
        {
            if (value >= 0)
                hp = value;
            else
                hp = 0;
            if (HealthImg != null)
                HealthImg.rectTransform.localScale = new Vector3(hp / MaxHp, 1, 1);
            if (HpText != null)
                HpText.text = ((int)hp).ToString();
        }
    }
    public float Shield
    {
        get { return shield; }
        set
        {
            if (value >= 0)
                shield = value;
            else
                shield = 0;
            if (ShieldImg != null)
                ShieldImg.rectTransform.localScale = new Vector3(shield / MaxShield, 1, 1);
            if (ShieldText != null)
            {
                ShieldText.text = ((int)shield).ToString();
            }
        }
    }
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            if (ScoreText != null)
                ScoreText.text = score.ToString();
        }
    }

    public int InitScore;

    private float hp, shield;
    private int score;
    public UnityEngine.UI.Text ScoreText, HpText, ShieldText;
    public UnityEngine.UI.RawImage HealthImg, ShieldImg;

    void Start()
    {
        Score = InitScore;
        Hp = MaxHp;
        Shield = MaxShield;
    }

    void Update()
    {
        if (Shield + ShieldRegeneration < MaxShield)
            Shield += ShieldRegeneration;
    }
}
