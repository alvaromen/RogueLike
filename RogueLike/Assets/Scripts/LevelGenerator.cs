using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    private Vector2 initialPosition;
    private Vector2 bossPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMainRoad()
    {
        RoomCreator roomScript = GetComponent<RoomCreator>();
        roomScript.SetupRoom(RoomCreator.Conexions.TBLR, new Vector2(0, 0));
        int direction = Random.Range(1, 5);
        bool stop = false;
        do
        {
            //if(direction )
        } while (!stop);
    }
}
