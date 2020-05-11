using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    protected Room room;
    public GameObject enemyExplosion;
    public GameObject[] mobDrops;
    public AudioClip[] shootAudioClips;
    public AudioClip[] explosionClips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public void SetRoom(Room r)
    {
        room = r;
    }

    public void GetHurt(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            room.EnemyDown();

            if(UnityEngine.Random.Range(0f, 1f) < 0.2f){
                int randIndex = UnityEngine.Random.Range(0, mobDrops.Length);
                Instantiate(mobDrops[randIndex], gameObject.transform.position, Quaternion.identity);
            }
            Instantiate(enemyExplosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            audioSource.PlayOneShot(explosionClips[Random.Range(0, explosionClips.Length)]);
        }
    }
}
