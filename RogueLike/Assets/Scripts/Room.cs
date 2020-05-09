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

    public void Visit()
    {
        if (status == Status.nonvisited)
        {
            status = Status.visited;
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
        int n = Random.Range(2, 5);
        for (int i = 0; i < n; i++)
        {
            Vector3 position = GameObject.FindGameObjectWithTag("Player").transform.position;
            float xmin = position.x - position.x % 16;
            float ymin = position.y - position.y % 16;
            Vector3 randomPosition = new Vector3(xmin + (int)Random.Range(3, 12), ymin + (int)Random.Range(3, 12), 0f);
            GameObject objectChoice = enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)]; //choose a random tile from the array of game objects tileArray
            enemies.Add(Object.Instantiate(objectChoice, randomPosition, Quaternion.identity));
        }
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
