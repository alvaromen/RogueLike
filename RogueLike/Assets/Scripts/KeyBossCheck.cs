using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBossCheck : MonoBehaviour
{

    public AudioClip openDoorClip;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player")){
            bool key = collision.collider.GetComponent<PlayerController>().hasBossKey;

            if(key){
                AudioSource audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
                audioSource.PlayOneShot(openDoorClip);
                Destroy(gameObject);
            }
        }
    }
}
