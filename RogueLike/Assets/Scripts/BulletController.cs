﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    int damage;
    public GameObject explosionPrefab;
    bool isBeingDestroyed = false;

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
            GameObject explosionRef = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
}
