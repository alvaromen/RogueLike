using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomViewer : MonoBehaviour
{

    public RoomCreator roomScript;

    // Awake is called before Start
    void Awake()
    {
        roomScript = GetComponent<RoomCreator>();
        InitGame();
    }

    void InitGame()
    {
        roomScript.SetupRoom();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
