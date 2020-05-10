using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDrop : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            //apply whatever boost we provide
            Destroy(gameObject);
            print("touched player");
        }

    }
}
