using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Room room;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoom(Room r)
    {
        room = r;
    }

    public void Entering()
    {
        if(room.GetStatus() == Room.Status.nonvisited)
        {
            room.Visit();
        }
    }
}
