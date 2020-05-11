using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class RoomCreator : MonoBehaviour
{

    public Vector2 position; 

    public enum RoomType
    {
        initial,
        normal,
        keyBoss,
        boss
    }

    private RoomType roomType;

    public enum Conexions
    {
        T, B, L, R, TB, TL, TR, BL, BR, LR, TBL, TBR, TLR, BLR, TBLR
    }

    private Conexions conexions;

    /**
     * Serializable class to indicate the minimum and maximum of an object in the room
     */
    [Serializable] //Using serializable allows us to embed a class with subproperties in the inspector
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //variables which are going to define the room

    //dimensions of the room
    public int columns;
    public int rows;
    
    //Arrays of prefabs
    public GameObject exit;
    public GameObject exitBoss;
    public GameObject[] floorTiles;
    public GameObject[] outerWallTilesTop;
    public GameObject[] outerWallTilesBot;
    public GameObject[] outerWallTilesLeft;
    public GameObject[] outerWallTilesRight;
    public GameObject[] outerWallTilesCorner;
    public GameObject[] bossFloorTiles;
    public GameObject[] bossOuterWallTilesTop;
    public GameObject[] bossOuterWallTilesBot;
    public GameObject[] bossOuterWallTilesLeft;
    public GameObject[] bossOuterWallTilesRight;
    public GameObject[] bossOuterWallTilesCorner;

    private Transform boardHolder; //variable to store references to the transform of our Board to keep the hierarchy clean

    /**
     * Sets up the outer wall and the floor of the game board
     */
    /**
     * Sets up the outer wall and the floor of the game board
     */
    void BoardSetup(List<GameObject> doors)
    {
        boardHolder = new GameObject("Board").transform;
        
        GameObject[] floors;
        GameObject[] outerTop;
        GameObject[] outerBot;
        GameObject[] outerLeft;
        GameObject[] outerRight;
        GameObject[] corners;

        if(roomType == RoomType.boss){
            floors = bossFloorTiles;
            outerTop = bossOuterWallTilesTop;
            outerBot = bossOuterWallTilesBot;
            outerLeft = bossOuterWallTilesLeft;
            outerRight = bossOuterWallTilesRight;
            corners = bossOuterWallTilesCorner;
        }else{
            floors = floorTiles;
            outerTop = outerWallTilesTop;
            outerBot = outerWallTilesBot;
            outerLeft = outerWallTilesLeft;
            outerRight = outerWallTilesRight;
            corners = outerWallTilesCorner;
        }

        GameObject instance;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject toInstantiate = outerTop[Random.Range(0, outerTop.Length)];
                int n = 0;
                bool exitTile = false;
                if (i == 0 || i == (rows - 1) || j == 0 || j == (columns - 1))
                {
                    if (j == (columns - 1) && (i != 0 && i != (rows - 1)))
                        toInstantiate = outerTop[Random.Range(0, outerTop.Length)];

                    if (j == 0 && (i != 0 && i != (rows - 1)))
                        toInstantiate = outerBot[Random.Range(0, outerBot.Length)];

                    if (i == 0 && (j != 0 && j != (columns - 1)))
                        toInstantiate = outerLeft[Random.Range(0, outerLeft.Length)];

                    if (i == (rows - 1) && (j != 0 && j != (columns - 1)))
                        toInstantiate = outerRight[Random.Range(0, outerRight.Length)];

                    if (i == 0 && j == (columns - 1))
                        toInstantiate = corners[0];

                    if (i == (rows - 1) && j == (columns - 1))
                        toInstantiate = corners[1];

                    if (i == 0 && j == 0)
                        toInstantiate = corners[2];

                    if (i == (rows - 1) && j == 0)
                        toInstantiate = corners[3];

                    if (conexions.ToString().Contains("T"))
                        if ((i == (rows / 2 - 1) && j == (columns - 1)) || (i == (rows / 2) && j == (columns - 1)))
                        {
                            toInstantiate = exit;
                            exitTile = true;
                        }

                    if (conexions.ToString().Contains("B"))
                        if ((i == (rows / 2 - 1) && j == 0) || (i == (rows / 2) && j == 0))
                        {
                            toInstantiate = exit;
                            exitTile = true;
                        }

                    if (conexions.ToString().Contains("L"))
                        if ((i == 0 && j == (columns / 2 - 1)) || (i == 0 && j == (columns / 2)))
                        {
                            toInstantiate = exit;
                            exitTile = true;
                        }

                    if (conexions.ToString().Contains("R"))
                        if ((i == (rows - 1) && j == (columns / 2 - 1)) || (i == (rows - 1) && j == (columns / 2)))
                        {
                            toInstantiate = exit;
                            exitTile = true;
                        }
                }
                else
                {
                    toInstantiate = floors[Random.Range(0, floors.Length)];
                    n = (int)Random.Range(0, 4);
                }
                Quaternion q = Quaternion.identity;
                instance = Instantiate(toInstantiate, new Vector3(position.x + i, position.y + j, 1f), q);
                instance.transform.SetParent(boardHolder);
                if (exitTile)
                {
                    doors.Add(instance);
                    if(roomType == RoomType.boss)
                        Instantiate(exitBoss, new Vector3(position.x + i, position.y + j, 0), q).transform.SetParent(boardHolder);
                }
            }
        }
    }

    public void setRoomType(RoomType type)
    {
        roomType = type;
    }

    public void setConexions(Conexions conex)
    {
        conexions = conex;
    }

    /**
    * Generates a random room
    */
    public Room SetupRoom(Conexions conexion, Vector2 pos, RoomType type, GameObject boss)
    {
        conexions = conexion;
        position = pos;
        roomType = type;
        columns = 16;
        rows = 16;
        List<GameObject> doors = new List<GameObject>();
        BoardSetup(doors);
        Room.Status status = Room.Status.nonvisited;
        if(roomType == RoomType.initial)
            status = Room.Status.cleared;
        Room room = new Room(roomType, status, position, conexions, boss);
        room.SetDoors(doors);
        if (type == RoomCreator.RoomType.boss)
            room.SetDoorBoss(exitBoss);
        return room;
    }
}
