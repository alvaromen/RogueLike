using System.Collections.Generic;
using UnityEngine;
using RoomType = RoomCreator.RoomType;
using Conexions = RoomCreator.Conexions;

public class Room : MonoBehaviour{

    private Conexions conexions;

    private RoomType roomType;

    private GameObject keyBoss;
    private GameObject bossPrefab;
    private GameObject bossDoor;
    private GameObject bossEnemy;


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
        bossPrefab = b;
        if(roomType == RoomType.boss)
        {
            Vector3 pos = p;
            Quaternion quaternion = Quaternion.identity;
            int direction = 0;
            switch (c)
            {
                case Conexions.T:
                    pos = new Vector3(p.x + 7.5f, p.y + 4, 0);
                    quaternion = Quaternion.identity;
                    direction = 0;
                    break;
                case Conexions.B:
                    pos = new Vector3(p.x + 7.5f, p.y + 12, 0);
                    quaternion = Quaternion.Euler(new Vector3(0, 0, 180));
                    direction = 1;
                    break;
                case Conexions.L:
                    pos = new Vector3(p.x + 12, p.y + 7.5f, 0);
                    quaternion = Quaternion.Euler(new Vector3(0, 0, 90));
                    direction = 2;
                    break;
                case Conexions.R:
                    pos = new Vector3(p.x + 4, p.y + 7.5f, 0);
                    quaternion = Quaternion.Euler(new Vector3(0, 0, 270));
                    direction = 3;
                    break;
                default:
                    break;
            }

            bossEnemy = Instantiate(bossPrefab, pos, quaternion);
            bossEnemy.GetComponent<BossEnemy>().SetDirection(direction);
            bossEnemy.GetComponent<BossEnemy>().SetRoom(this);
            nEnemies = 1;
        }
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
                Vector3 pos = new Vector3(door.transform.position.x, door.transform.position.y, 0.8f);
                turretsToDestroy.Add(Instantiate(turretPrefab, pos, Quaternion.identity));
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
            nEnemies = Random.Range(3, 5);
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
            bossEnemy.GetComponent<BossEnemy>().Activate();
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
