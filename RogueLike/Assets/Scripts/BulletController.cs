using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("PlayerBullet"))
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().GetHurt(damage);
            }
        }
        else if (CompareTag("EnemyBullet"))
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().GetHurt(damage);
            }
        }
        if (!(tag.Contains("Bullet") && other.tag.Contains("Bullet"))){
            Destroy(gameObject);
        }
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
