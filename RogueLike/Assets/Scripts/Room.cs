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

    private GameObject[] enemiesPrefabs;

    private List<GameObject> enemies = new List<GameObject>();

    private int nEnemies;

    private List<GameObject> doors = new List<GameObject>();

    private List<Vector3> gridPositions = new List<Vector3>();

    public Room(RoomType rt, Status s, List<Vector3> gp)
    {
        roomType = rt;
        status = s;
        gridPositions = gp;
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
    void LayoutObjectAtRandom(GameObject[] gObject, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1); //controls how many of a given object will be spawned

        nEnemies = objectCount;

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject objectChoice = gObject[Random.Range(0, gObject.Length)]; //choose a random tile from the array of game objects tileArray
            enemies.Add(Object.Instantiate(objectChoice, randomPosition, Quaternion.identity));
        }
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

    public void SetEnemies(GameObject[] prefabs)
    {
        enemiesPrefabs = prefabs;
    }

    private void SpawnEnemies()
    {
        LayoutObjectAtRandom(enemiesPrefabs, 2, 7);
    }

    public Status GetStatus()
    {
        return status;
    }

    public void EnemyDown(GameObject enemy)
    {
        nEnemies--;
        enemies.Remove(enemy);
        if(nEnemies <= 0)
        {
            status = Status.cleared;
            foreach(GameObject door in doors)
            {
                door.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }
}
