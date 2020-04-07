using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class RoomCreator : MonoBehaviour
{

    public enum RoomType{
        initial,
        normal,
        boss
    }

    private RoomType roomType;

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
    public int columns = Random.Range(8, 16);
    public int rows = Random.Range(8, 16);

    public Count wallCount = new Count(5, 9); //limits of the random number of walls per room
    public Count enemiesCount = new Count(1, 5); //limits of the random number of enemies per room
    public GameObject exitLevel;
    //Arrays of prefabs
    public GameObject[] exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder; //variable to store references to the transform of our Board to keep the hierarchy clean
    private List<Vector3> gridPositions = new List<Vector3>(); //list of possible locations to place titles

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
                gridPositions.Add(new Vector3(i, j, 0f));
            }
        }
    }

    /**
     * Sets up the outer wall and the floor of the game board
     */
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for(int i = -1; i < (columns + 1); i++)
        {
            for(int j = -1; j < (rows + 1); j++)
            {
                GameObject toInstantiate;
                int n = 0;
                if(i == -1 || i == columns || j == -1 || j == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                } else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    n = (int)Random.Range(0, 4);
                }
                Quaternion q = Quaternion.identity;
                q.z = n * 90;
                GameObject instance = Instantiate(toInstantiate, new Vector3(i, j, 0f), q);
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

    /**
     * Generates a random room
     */
    public void SetupRoom()
    {
        BoardSetup();
        InitialiseList();
    }
}
