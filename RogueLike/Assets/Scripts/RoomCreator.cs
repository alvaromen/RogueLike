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
                gridPositions.Add(new Vector3(position.x + i, position.y + j, 0f));
            }
        }
    }

    /**
     * Sets up the outer wall and the floor of the game board
     */
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        GameObject instance;
        for (int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                GameObject toInstantiate;
                int n = 0;
                if(i == 0 || i == (columns - 1) || j == 0 || j == (rows - 1))
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                } else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    n = (int)Random.Range(0, 4);
                }
                Quaternion q = Quaternion.identity;
                q.z = n * 90;
                instance = Instantiate(toInstantiate, new Vector3(position.x + i, position.y + j, 0f), q);
                instance.transform.SetParent(boardHolder);
            }
        }

        switch (conexions)
        {
            case Conexions.T:
                Debug.Log(position.x + " " + position.y + " " + (position.x + columns) + " " + (position.y + rows));
                Debug.Log((position.x + (columns / 2 - 1)) + " " + (position.y + rows));
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y + rows - 1, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y + rows - 1, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.B:
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.L:
                instance = Instantiate(exit, new Vector3(position.x, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.R:
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.TB:
                //T
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //B
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.TL:
                //T
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //L
                instance = Instantiate(exit, new Vector3(position.x, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.TR:
                //T
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //R
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.BL:
                //B
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //L
                instance = Instantiate(exit, new Vector3(position.x, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.BR:
                //B
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //R
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.LR:
                //L
                instance = Instantiate(exit, new Vector3(position.x, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //R
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.TBL:
                //T
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //B
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //L
                instance = Instantiate(exit, new Vector3(position.x, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.TBR:
                //T
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //B
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //R
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.TLR:
                //T
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //L
                instance = Instantiate(exit, new Vector3(position.x, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //R
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.BLR:
                //B
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //L
                instance = Instantiate(exit, new Vector3(position.x, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //R
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
            case Conexions.TBLR:
                //T
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y + rows, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //B
                instance = Instantiate(exit, new Vector3(position.x + (columns / 2 - 1), position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns / 2, position.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //L
                instance = Instantiate(exit, new Vector3(position.x, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                //R
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + (rows / 2 - 1), 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                instance = Instantiate(exit, new Vector3(position.x + columns, position.y + rows / 2, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
                break;
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
    public void SetupRoom(Conexions conexion, Vector2 pos)
    {
        conexions = conexion;
        position = pos;
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
