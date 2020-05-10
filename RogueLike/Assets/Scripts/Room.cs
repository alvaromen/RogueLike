using System.Collections.Generic;
using UnityEngine;
using RoomType = RoomCreator.RoomType;
using Conexions = RoomCreator.Conexions;

public class Room : MonoBehaviour{

    private Conexions conexions;

    private RoomType roomType;

    private GameObject keyBoss;
    private GameObject boss;
    private GameObject bossDoor;
    
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

    public Room(RoomType rt, Status s, Vector2 p, Conexions c, GameObject b)
    {
        roomType = rt;
        status = s;
        position = p;
        conexions = c;
        boss = b;
        if(roomType == RoomType.boss)
        {
            Vector3 pos = p;
            Quaternion quaternion = Quaternion.identity;
            switch (c)
            {
                case Conexions.T:
                    pos = new Vector3(p.x + 7.5f, p.y + 5, 0);
                    quaternion = Quaternion.identity;
                    break;
                case Conexions.B:
                    pos = new Vector3(p.x + 7.5f, p.y + 11, 0);
                    quaternion = Quaternion.Euler(new Vector3(0, 0, 180));
                    break;
                case Conexions.L:
                    pos = new Vector3(p.x + 11, p.y + 7.5f, 0);
                    quaternion = Quaternion.Euler(new Vector3(0, 0, 90));
                    break;
                case Conexions.R:
                    pos = new Vector3(p.x + 5, p.y + 7.5f, 0);
                    quaternion = Quaternion.Euler(new Vector3(0, 0, 270));
                    break;
                default:
                    break;
            }

            Instantiate(boss, pos, quaternion);
        }
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
        if (roomType == RoomType.normal || roomType == RoomType.keyBoss)
        {
            nEnemies = Random.Range(2, 4);
            for (int i = 0; i < nEnemies; i++)
            {
                Vector3 randomPosition = new Vector3(position.x + (int)Random.Range(3, 12), position.y + (int)Random.Range(3, 12), 0f);
                GameObject objectChoice = enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)]; //choose a random tile from the array of game objects tileArray
                GameObject enemy = Instantiate(objectChoice, randomPosition, Quaternion.identity);
                enemy.GetComponent<Enemy>().SetRoom(this);
                enemies.Add(enemy);
            }
        } else if (roomType == RoomType.boss)
        {
            foreach (GameObject door in doors)
            {
                Vector3 pos = new Vector3(door.transform.position.x, door.transform.position.y, 0.8f);
                Instantiate(keyBoss, pos, Quaternion.identity);
            }
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

    public void SetDoorBoss(GameObject bd)
    {
        bossDoor = bd;
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
                Instantiate(keyBoss, position + Vector2.one * 8, Quaternion.identity);
        }
    }
}
