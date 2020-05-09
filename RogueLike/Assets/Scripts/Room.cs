using System.Collections.Generic;
using UnityEngine;
using RoomType = RoomCreator.RoomType;

public class Room {

    private RoomType roomType;
    
    public enum Status
    {
        nonvisited,
        visited,
        cleared
    }

    private Status status;

    private List<GameObject> doors = new List<GameObject>();

    private List<Vector3> gridPositions = new List<Vector3>();

    public Room(RoomType rt, Status s, List<Vector3> gp)
    {
        roomType = rt;
        status = s;
        gridPositions = gp;
    }

    public void Visit()
    {
        if (status == Status.nonvisited)
        {
            SpawnEnemies();
            foreach (GameObject door in doors)
            {
                door.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }

    public void SetDoors(List<GameObject> d)
    {
        doors = d;
    }

    private void SpawnEnemies()
    {

    }

    public Status GetStatus()
    {
        return status;
    }
}
