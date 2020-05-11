using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBossController : BombController
{
    private Vector3 destination;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        hp = 3;
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetParameters(Vector3 d, Vector3 v)
    {
        print(velocity);
        destination = d;
        velocity = v;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tag == "EnemyBomb" && other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().GetHurt(damage);
        }

        audioSource.PlayOneShot(explosionSfx);
        Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
