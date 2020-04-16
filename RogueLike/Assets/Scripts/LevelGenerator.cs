using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	//enum GridSpace { empty, floor, floorAlt1, floorAlt2, floorAlt3, wallLeft, wallRight, corner1, corner2, corner3, corner4 };
	enum GridSpace {empty, floor, wall, init};

	Dictionary<GridSpace, GameObject> mapValues;
	
	GridSpace[,] grid;
	int floorHeight, floorWidth;
	Vector2 roomSizeWorldUnits;
	float worldUnitsInOneGridCell = 1;
	

	struct Room
	{
		public Vector2 init;
		public Vector2 dims;
	}

	List<Room> rooms;

	public GameObject wallObj, floorObj, emptyObj;

	Transform boardHolder;

	void Start()
	{
		Setup();
		CreateRooms();
		SpawnLevel();
	}

	void Setup()
	{
		roomSizeWorldUnits = new Vector2(Random.Range(30, 100), Random.Range(30, 100));
		
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
		mapValues[GridSpace.wall] = emptyObj;
		mapValues[GridSpace.empty] = emptyObj;
		mapValues[GridSpace.init] = wallObj;
	}

	void CreateRooms()
	{
		int maxRooms = 30;
		int maxTries = 50;
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
						//print(ovv + ": " + currRoom.init + " | " + (currRoom.init + currRoom.dims) + " || " + room.init + " | " + (room.init + room.dims));
						if (ovv)
						{
							overlap = true;
							break;
						}
					}
				}
			} while (overlap && (numTries <= maxTries));

			//print(numTries + ", " + maxTries);
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
			else
				print("Room failed");
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