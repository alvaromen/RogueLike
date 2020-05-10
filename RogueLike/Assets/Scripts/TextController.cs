using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class TextController : MonoBehaviour
{
    public Text points;
    public Text hasKey;
    public Text speed;
    public Text fireRate;

    public GameObject player;



    void Update(){
        points.text = "POINTS: " + player.GetComponent<PlayerController>().points;
        hasKey.text = "KEY: " + player.GetComponent<PlayerController>().hasBossKey;
        speed.text = "Speed Boost: " + player.GetComponent<PlayerController>().speedIncrementFactor + "x";
        fireRate.text = "Fire Speed Boost: " + (0.5f / player.GetComponent<PlayerController>().speedIncrementFactor) + "x";
    }


}
