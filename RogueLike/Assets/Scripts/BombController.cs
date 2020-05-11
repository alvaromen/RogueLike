using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    float damage;

    float hp;

    public GameObject explosion;
    public AudioClip explosionSfx;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        hp = 3;
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void GetHurt()
    {
        hp--;
        if (hp <= 0){
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddPoints(50);
            audioSource.PlayOneShot(explosionSfx);
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tag == "EnemyBomb" && other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(explosionSfx);
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerController>().GetHurt(damage);
        }
    }
}
