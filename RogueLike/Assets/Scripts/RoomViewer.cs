using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomViewer : MonoBehaviour
{

    private RoomCreator roomScript;
    public RoomCreator.Conexions conexion = RoomCreator.Conexions.TBLR;
    public Vector2 position = new Vector2(0, 0);

    // Awake is called before Start
    void Awake()
    {
        roomScript = GetComponent<RoomCreator>();
        InitGame();
    }

    void InitGame()
    {
        roomScript.SetupRoom(conexion, position);
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
