using System.Collections.Generic;
using UnityEngine;
using RoomType = RoomCreator.RoomType;

public class Room : MonoBehaviour{

    private RoomType roomType;

    private GameObject keyBoss;
    
    public enum Status
    {
        nonvisited,
        visited,
        cleared
    }

    private Status status;

    private GameObject[] enemiesPrefabs;
    public GameObject turretPrefab;
    private List<GameObject> turretsToDestroy = new List<GameObject>();

    private List<GameObject> enemies = new List<GameObject>();

    private int nEnemies;

    private List<GameObject> doors = new List<GameObject>();

    private Vector2 position;

    public Room(RoomType rt, Status s, Vector2 p)
    {
        roomType = rt;
        status = s;
        position = p;
    }

    public void Visit()
    {
        if (status == Status.nonvisited)
        {
            status = Status.visited;
            SpawnEnemies();
            float i = 0;
            foreach (GameObject door in doors)
            {
                door.GetComponent<BoxCollider2D>().isTrigger = false;
                Vector3 pos = new Vector3(door.transform.position.x, door.transform.position.y, 0.8f);
                turretsToDestroy.Add(Instantiate(turretPrefab, pos, Quaternion.identity));
                i += 0.5f;
            }
        }
    }

    public void SetDoors(List<GameObject> d)
    {
        doors = d;
    }

    public void SetEnemies(GameObject[] prefabs, GameObject turrets)
    {
        enemiesPrefabs = prefabs;
        turretPrefab = turrets;
    }

    private void SpawnEnemies()
    {
        nEnemies = Random.Range(2, 4);
        for (int i = 0; i < nEnemies; i++)
        {
            Vector3 randomPosition = new Vector3(position.x + (int)Random.Range(3, 12), position.y + (int)Random.Range(3, 12), 0f);
            GameObject objectChoice = enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)]; //choose a random tile from the array of game objects tileArray
            GameObject enemy = Object.Instantiate(objectChoice, randomPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetRoom(this);
            enemies.Add(enemy);
        }
    }

    public void SetStatus(Status s)
    {
        status = s;
    }

    public Status GetStatus()
    {
        return status;
    }

    public void SetRoomType(RoomType r)
    {
        roomType = r;
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }

    public void SetKeyBoss(GameObject kb)
    {
        keyBoss = kb;
    }

    public void EnemyDown()
    {
        nEnemies--;
        if(nEnemies <= 0)
        {
            status = Status.cleared;
            foreach(GameObject door in doors)
            {
                door.GetComponent<BoxCollider2D>().isTrigger = true;
            }

            foreach(GameObject turret in turretsToDestroy){
                Destroy(turret);
            }

            if(roomType == RoomType.keyBoss)
                Instantiate(keyBoss, position, Quaternion.identity);
        }
    }
}
