﻿using System.Collections;
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

    public Count wallCount = new Count(5, 9); //limits of the random number of walls per room
    public Count enemiesCount = new Count(1, 5); //limits of the random number of enemies per room
    public GameObject exitLevel;
    //Arrays of prefabs
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTilesTop;
    public GameObject[] outerWallTilesBot;
    public GameObject[] outerWallTilesLeft;
    public GameObject[] outerWallTilesRight;
    public GameObject[] outerWallTilesCorner;

    private Transform boardHolder; //variable to store references to the transform of our Board to keep the hierarchy clean
    private List<Vector3> gridPositions = new List<Vector3>(); //list of possible locations to place tiles

    /**
     * Clears the gridPositions and then generates a new list of possible positions where spawning objects
     */
    void InitialiseList()
    {
        gridPositions.Clear();

        for(int i = 1; i < (columns - 1); i++)
        {
            for(int j = 1; j < (rows - 1); j++)
            {
                gridPositions.Add(new Vector3(position.x + i, position.y + j, 0f));
            }
        }
    }

    /**
     * Sets up the outer wall and the floor of the game board
     */
    /**
     * Sets up the outer wall and the floor of the game board
     */
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        GameObject instance;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject toInstantiate = outerWallTilesTop[Random.Range(0, outerWallTilesTop.Length)];
                int n = 0;
                if (i == 0 || i == (rows - 1) || j == 0 || j == (columns - 1))
                {
                    if (j == (columns - 1) && (i != 0 && i != (rows - 1)))
                        toInstantiate = outerWallTilesTop[Random.Range(0, outerWallTilesTop.Length)];
                    if (j == 0 && (i != 0 && i != (rows - 1)))
                        toInstantiate = outerWallTilesBot[Random.Range(0, outerWallTilesBot.Length)];
                    if (i == 0 && (j != 0 && j != (columns - 1)))
                        toInstantiate = outerWallTilesLeft[Random.Range(0, outerWallTilesLeft.Length)];
                    if (i == (rows - 1) && (j != 0 && j != (columns - 1)))
                        toInstantiate = outerWallTilesRight[Random.Range(0, outerWallTilesRight.Length)];
                    if (i == 0 && j == (columns - 1))
                        toInstantiate = outerWallTilesCorner[0];
                    if (i == (rows - 1) && j == (columns - 1))
                        toInstantiate = outerWallTilesCorner[1];
                    if (i == 0 && j == 0)
                        toInstantiate = outerWallTilesCorner[2];
                    if (i == (rows - 1) && j == 0)
                        toInstantiate = outerWallTilesCorner[3];
                    if (conexions.ToString().Contains("T"))
                        if ((i == (rows / 2 - 1) && j == (columns - 1)) || (i == (rows / 2) && j == (columns - 1)))
                            toInstantiate = exit;
                    if (conexions.ToString().Contains("B"))
                        if ((i == (rows / 2 - 1) && j == 0) || (i == (rows / 2) && j == 0))
                            toInstantiate = exit;
                    if (conexions.ToString().Contains("L"))
                        if ((i == 0 && j == (columns / 2 - 1)) || (i == 0 && j == (columns / 2)))
                            toInstantiate = exit;
                    if (conexions.ToString().Contains("R"))
                        if ((i == (rows - 1) && j == (columns / 2 - 1)) || (i == (rows - 1) && j == (columns / 2)))
                            toInstantiate = exit;
                }
                else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    n = (int)Random.Range(0, 4);
                }
                Quaternion q = Quaternion.identity;
                instance = Instantiate(toInstantiate, new Vector3(position.x + i, position.y + j, 1f), q);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /**
    * Returns a random position of the possibles positions in gridPositions
    */
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    /**
     * Spawns the specified tile in the position chosen
     */
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1); //controls how many of a given object will be spawned

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)]; //choose a random tile from the array of game objects tileArray
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
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
    public void SetupRoom(Conexions conexion, Vector2 pos, RoomType type)
    {
        conexions = conexion;
        position = pos;
        roomType = type;
        /*
        do
        {
            columns = Random.Range(8, 16);
        } while (columns % 2 != 0);
        do
        {
            rows = Random.Range(8, 16);
        } while (rows % 2 != 0);
        */
        columns = 16;
        rows = 16;
        BoardSetup();
        InitialiseList();
    }
}
