using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    float damage;

    float hp;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        hp = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void GetHurt()
    {
        hp--;
        if (hp <= 0){
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tag == "EnemyBomb" && other.CompareTag("Player"))
        {
            Destroy(gameObject);
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            other.gameObject.GetComponent<PlayerController>().GetHurt(damage);
        }
    }
}
