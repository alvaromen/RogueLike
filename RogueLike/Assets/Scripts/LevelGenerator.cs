using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
	//enum GridSpace { empty, floor, floorAlt1, floorAlt2, floorAlt3, wallLeft, wallRight, corner1, corner2, corner3, corner4 };
	enum GridSpace {empty, floor, wall, init, passage};

	Dictionary<GridSpace, GameObject> mapValues;
	
	GridSpace[,] grid;
	int floorHeight, floorWidth;
	Vector2 roomSizeWorldUnits;
	float worldUnitsInOneGridCell = 1;


	struct Walker
	{
		public Vector2 dir;
		public Vector2 pos;
	}

	//public float percentToFill = .6f;
	
	public int maxPathways = 40;
	public int maxRooms = 100;
	public int maxTries = 50;


	struct Room
	{
		public Vector2 init;
		public Vector2 dims;
	}

	List<Room> rooms;

	public GameObject wallObj, floorObj, pathObj, emptyObj;

	Transform boardHolder;

	void Start()
	{
		Setup();
		CreateRooms();
		CreateWalkWays();
		SpawnLevel();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(0);
		}
	}

	void Setup()
	{
		roomSizeWorldUnits = new Vector2(Random.Range(100, 200), Random.Range(100, 200));

		boardHolder = new GameObject("Board").transform;

		//find grid size
		floorHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
		floorWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);
		//create grid
		grid = new GridSpace[floorWidth, floorHeight];
		//set grid's default state
		for (int x = 0; x < floorWidth - 1; x++)
		{
			for (int y = 0; y < floorHeight - 1; y++)
			{
				//make every cell "empty"
				grid[x, y] = GridSpace.empty;
			}
		}

		rooms = new List<Room>();

		InitMapValues();
	}

	void InitMapValues()
	{
		mapValues = new Dictionary<GridSpace, GameObject>();

		mapValues[GridSpace.floor] = floorObj;
		mapValues[GridSpace.passage] = pathObj;
		mapValues[GridSpace.wall] = emptyObj;
		mapValues[GridSpace.empty] = emptyObj;
		mapValues[GridSpace.init] = wallObj;
	}

	void CreateRooms()
	{
		for (int r = 0; r < maxRooms; r++)
		{
			int x;
			int y;

			int maxRoomWidth;
			int maxRoomHeight;

			Room currRoom = new Room();

			bool overlap = false;
			int numTries = 0;
			do
			{
				numTries++;
				do
				{
					x = Random.Range(5, floorWidth - 5);
					y = Random.Range(5, floorHeight - 5);
				} while (grid[x, y] != GridSpace.empty);
				
				maxRoomWidth = Random.Range(5, 15);
				maxRoomHeight = Random.Range(5, 15);

				currRoom.init = new Vector2(x, y);
				currRoom.dims = new Vector2(maxRoomWidth, maxRoomHeight);

				if (rooms.Count > 0)
				{
					foreach (Room room in rooms)
					{
						bool ovv = doOverlap(currRoom, room, 2);
						if (ovv)
						{
							overlap = true;
							break;
						}
					}
				}
			} while (overlap && (numTries <= maxTries));

			if (!overlap)
			{
				rooms.Add(currRoom);

				for (int i = x; i < x + maxRoomWidth; i++)
				{
					for (int j = y; j < y + maxRoomHeight; j++)
					{
						int iInRange = Mathf.Clamp(i, 1, floorWidth - 3);
						int jInRange = Mathf.Clamp(j, 1, floorHeight - 3);

						if (!(grid[iInRange, jInRange] == GridSpace.floor))
							grid[iInRange, jInRange] = GridSpace.floor;
						else
							grid[iInRange, jInRange] = GridSpace.empty;

						grid[x, y] = GridSpace.init;
					}
				}
			}
		}
	}

	void CreateWalkWays()
	{
		for (int i = 0; i < maxPathways; i++)
		{
			int randInitIndex = Random.Range(0, rooms.Count - 1);
			int randEndIndex;

			do
			{
				randEndIndex = Random.Range(0, rooms.Count - 1);
			} while (randEndIndex == randInitIndex);

			// we have our starting and ending rooms
			Room initialRoom = rooms[randInitIndex];
			Room finalRoom = rooms[randEndIndex];

			// more precisely, we have our starting and ending points
			int randInitX = Random.Range((int)initialRoom.init.x, (int)(initialRoom.init.x + initialRoom.dims.x));
			int randInitY = Random.Range((int)initialRoom.init.y, (int)(initialRoom.init.y + initialRoom.dims.y));

			int randEndX = Random.Range((int)finalRoom.init.x, (int)(finalRoom.init.x + finalRoom.dims.x));
			int randEndY = Random.Range((int)finalRoom.init.y, (int)(finalRoom.init.y + finalRoom.dims.y));

			print("init: " + randInitX + ", " + randInitY + ". end: " + randEndX + ", " + randEndY);

			// we spawn at the beginning
			Vector2 spawnPos = new Vector2(randInitX, randInitY);
			Walker walker = new Walker
			{
				pos = spawnPos
			};

			int diffX = (int)walker.pos.x - randEndX;
			int diffY = (int)walker.pos.y - randEndY;

			while (Mathf.Abs(diffX) > 0) //difference is growing instead of diminishing, FIX
			{
				print("going for x");
				int xInRange = (int)Mathf.Clamp(walker.pos.x, 1, floorWidth - 2);
				int yInRange = (int)Mathf.Clamp(walker.pos.y, 1, floorHeight - 2);
				if (grid[xInRange, yInRange] != GridSpace.floor)
				{
					grid[xInRange, yInRange] = GridSpace.passage;
				}
				walker.pos.x -= Mathf.Sign(diffX);
				diffX = (int)walker.pos.x - randEndX;
			}

			while (Mathf.Abs(diffY) > 0)
			{
				print("going for y");
				int xInRange = (int)Mathf.Clamp(walker.pos.x, 1, floorWidth - 2);
				int yInRange = (int)Mathf.Clamp(walker.pos.y, 1, floorHeight - 2);
				if (grid[xInRange, yInRange] != GridSpace.floor)
				{
					grid[xInRange, yInRange] = GridSpace.passage;
				}
				walker.pos.y -= Mathf.Sign(diffY);
				diffY = (int)walker.pos.y - randEndY;
			}
		}
	}

	bool doOverlap(Room room1, Room room2, int margin)
	{
		Vector2 l1 = room1.init;
		Vector2 r1 = room1.init + room1.dims;

		Vector2 l2 = room2.init;
		Vector2 r2 = room2.init + room2.dims;

		

		if(r1.x + margin < l2.x || r1.y + margin < l2.y || l1.x - margin > r2.x || l1.y - margin > r2.y)
		{
			return false;
		}

		return true;
	}

	int NumberOfFloors()
	{
		int count = 0;
		foreach (GridSpace space in grid)
		{
			if (space == GridSpace.floor)
			{
				count++;
			}
		}
		return count;
	}

	Vector2 RandomDirection()
	{
		//pick random int between 0 and 3
		int choice = Mathf.FloorToInt(Random.value * 3.99f);
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

	void SpawnLevel()
	{
		for (int x = 0; x < floorWidth; x++)
		{
			for (int y = 0; y < floorHeight; y++)
			{
				Spawn(x, y, mapValues[grid[x, y]]);
			}
		}
	}
	
	void Spawn(float x, float y, GameObject toSpawn)
	{
		//find the position to spawn
		Vector2 offset = roomSizeWorldUnits / 2.0f;
		Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;
		//spawn object
		Instantiate(toSpawn, spawnPos, Quaternion.identity).transform.SetParent(boardHolder);
	}
}