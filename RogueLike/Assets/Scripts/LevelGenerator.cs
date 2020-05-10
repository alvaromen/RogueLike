using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelGenerator : MonoBehaviour
{
    public GameObject keyBoss;

    private RoomCreator roomScript;

    private Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();

	int roomSize = 16;
	
	bool[,] grid;
	string[,] names;
	List<Vector2> possibleBoss;
	List<Vector2> possibleInit;

	int roomHeight, roomWidth;
	Vector2 roomSizeWorldUnits = new Vector2(15, 15);
	struct walker
	{
		public Vector2 dir;
		public Vector2 pos;
	}
	List<walker> walkers;

	[Range(0, 1)]
	public float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f, chanceWalkerDestroy = 0.05f;

	[Range(0, 50)]
	public int maxWalkers = 10, numIterations = 10;

    public GameObject[] enemiesPrefabs;
    public GameObject turretsPrefabs;

    void Start()
	{
		Setup();
		CreateRooms();
		NameRooms();
		SpawnLevel();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(0);
		}
	}
	void Setup()
	{
		//find grid size
		roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x);
		roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y);
		//create grid
		grid = new bool[roomWidth, roomHeight];
		names = new string[roomWidth, roomHeight];

		possibleBoss = new List<Vector2>();
		possibleInit = new List<Vector2>();
		//set grid's default state
		for (int x = 0; x < roomWidth - 1; x++)
		{
			for (int y = 0; y < roomHeight - 1; y++)
			{
				
				grid[x, y] = false;
				names[x, y] = "";
			}
		}

		roomScript = GetComponent<RoomCreator>();

		//set first walker
		//init list
		walkers = new List<walker>();
		//create a walker 
		walker newWalker = new walker();
		newWalker.dir = RandomDirection();
		//find center of grid
		Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2.0f),
										Mathf.RoundToInt(roomHeight / 2.0f));
		newWalker.pos = spawnPos;
		//add walker to list
		walkers.Add(newWalker);
	}
	void CreateRooms()
	{
		int iterations = 0;//loop will not run forever
		do
		{
			//create floor at position of every walker
			foreach (walker myWalker in walkers)
			{
				grid[(int)myWalker.pos.x, (int)myWalker.pos.y] = true;
			}
			//chance: destroy walker
			int numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++)
			{
				//only if its not the only one, and at a low chance
				if (UnityEngine.Random.value < chanceWalkerDestroy && walkers.Count > 1)
				{
					walkers.RemoveAt(i);
					break; //only destroy one per iteration
				}
			}
			//chance: walker pick new direction
			for (int i = 0; i < walkers.Count; i++)
			{
				if (UnityEngine.Random.value < chanceWalkerChangeDir)
				{
					walker thisWalker = walkers[i];
					thisWalker.dir = RandomDirection();
					walkers[i] = thisWalker;
				}
			}
			//chance: spawn new walker
			numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++)
			{
				//only if # of walkers < max, and at a low chance
				if (UnityEngine.Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
				{
					//create a walker 
					walker newWalker = new walker();
					newWalker.dir = RandomDirection();
					newWalker.pos = walkers[i].pos;
					walkers.Add(newWalker);
				}
			}
			//move walkers
			for (int i = 0; i < walkers.Count; i++)
			{
				walker thisWalker = walkers[i];
				thisWalker.pos += thisWalker.dir;
				walkers[i] = thisWalker;
			}
			//avoid boarder of grid
			for (int i = 0; i < walkers.Count; i++)
			{
				walker thisWalker = walkers[i];
				//clamp x,y to leave a 1 space boarder: leave room for walls
				thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
				thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight - 2);
				walkers[i] = thisWalker;
			}
			
			iterations++;
		} while (iterations < numIterations);
	}

	void NameRooms()
	{
		for (int x = 0; x < roomWidth; x++)
		{
			for (int y = 0; y < roomHeight; y++)
			{
				if(grid[x, y]){
					string result = "";
					if (y + 1 < roomHeight)
						if (grid[x, y + 1])
							result += "T";
					if (y - 1 >= 0)
						if (grid[x, y - 1])
							result += "B";
					if (x - 1 >= 0)
						if (grid[x - 1, y])
							result += "L";
					if (x + 1 < roomWidth)
						if (grid[x + 1, y])
							result += "R";

					if (result.Length == 1){
						possibleBoss.Add(new Vector2(x, y));
					}

					if (result.Length > 2){
						possibleInit.Add(new Vector2(x, y));
					}
					names[x, y] = result;
				}
			}
		}
	}
	void SpawnLevel()
	{
		int bossRoom = (int)UnityEngine.Random.Range(0, possibleBoss.Count);
        int initRoom = (int)UnityEngine.Random.Range(0, possibleInit.Count);

        if (possibleInit.Count > 0){
			Vector2 playerRoom = possibleInit[initRoom];

            Vector2 position = new Vector2(playerRoom.x * roomSize, playerRoom.y * roomSize) + Vector2.one * 8;
            GameObject.FindGameObjectWithTag("Player").transform.position = position;
            Vector3 cameraPosition = new Vector3(position.x, position.y, -10);
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = cameraPosition;
        }

		for (int x = 0; x < roomWidth; x++)
		{
			for (int y = 0; y < roomHeight; y++)
			{
				if (grid[x, y])
				{
					RoomCreator.RoomType type = RoomCreator.RoomType.normal;
					
					if (possibleBoss.Count > 0 && possibleBoss[bossRoom] == new Vector2(x, y)){
						type = RoomCreator.RoomType.boss;
					}

                    if (possibleInit.Count > 0 && possibleInit[initRoom] == new Vector2(x, y))
                    {
                        type = RoomCreator.RoomType.initial;
                    }

                    Spawn(x * roomSize, y * roomSize, names[x, y], type);
				}
			}
		}

        float posX;
        float posY;
        do
        {
            do
            {
                posX = UnityEngine.Random.Range(0, roomWidth * 16);
                posY = UnityEngine.Random.Range(0, roomHeight * 16);
            } while (!rooms.ContainsKey(new Vector2(posX, posY)));
        } while(rooms[new Vector2(posX, posY)].GetRoomType() != RoomCreator.RoomType.normal);

        rooms[new Vector2(posX, posY)].SetRoomType(RoomCreator.RoomType.keyBoss);
        rooms[new Vector2(posX, posY)].SetKeyBoss(keyBoss);
    }
	Vector2 RandomDirection()
	{
		//pick UnityEngine.Random int between 0 and 3
		int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);
		//use that int to chose a direction
		switch (choice)
		{
			case 0:
				return Vector2.down;
			case 1:
				return Vector2.left;
			case 2:
				return Vector2.up;
			default:
				return Vector2.right;
		}
	}
	int NumberOfFloors()
	{
		int count = 0;
		foreach (bool space in grid)
		{
			if (space == true)
			{
				count++;
			}
		}
		return count;
	}

	void Spawn(float x, float y, string conexions, RoomCreator.RoomType type)
	{
		Vector2 spawnPos = new Vector2(x, y);

		RoomCreator.Conexions nameEnum = (RoomCreator.Conexions)Enum.Parse(typeof(RoomCreator.Conexions), conexions);
        Room room = roomScript.SetupRoom(nameEnum, spawnPos, type);
        room.SetEnemies(enemiesPrefabs, turretsPrefabs);
        rooms.Add(spawnPos, room);
	}

    public void VisitRoom(Vector2 position)
    {
        Room room = rooms[position];
        room.Visit();
    }
}


