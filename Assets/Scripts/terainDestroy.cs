using UnityEngine;
using System.Collections;

public class terainDestroy : MonoBehaviour
{

    public GameObject explosion;

    // Use this for initialization
    void Start()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<shipSystemsPlayer>() != null)
        {
            collision.transform.GetComponent<shipSystemsPlayer>().recieveDmg(float.MaxValue, collision.contacts[0].point);
        }
        else if (collision.transform.GetComponent<shipSystemsEnemy>() != null)
        {
            collision.transform.GetComponent<shipSystemsEnemy>().recieveDmg(float.MaxValue, collision.contacts[0].point);
        }
        else
        {
            Destroy(collision.gameObject);
            Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
        }
    }
}
