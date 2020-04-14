using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	//enum GridSpace { empty, floor, floorAlt1, floorAlt2, floorAlt3, wallLeft, wallRight, corner1, corner2, corner3, corner4 };
	enum GridSpace { empty, floor, wall};

	Dictionary<GridSpace, GameObject> mapValues;
	
	GridSpace[,] grid;
	int floorHeight, floorWidth;
	Vector2 roomSizeWorldUnits;
	float worldUnitsInOneGridCell = 1;
	struct walker
	{
		public Vector2 dir;
		public Vector2 pos;
	}
	List<walker> walkers;

	[Range(0, 1)]
	public float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
	[Range(0, 1)]
	public float chanceWalkerDestoy = 0.05f;
	
	public int maxWalkers = 10;
	
	[Range(0, 1)]
	public float percentToFill = 0.1f;
	public GameObject wallObj, floorObj, emptyObj;

	Transform boardHolder;

	void Start()
	{
		Setup();
		//CreateFloors();
		CreateRooms();
		//CreateWalls();
		RemoveSingleWalls();
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
		//set first walker
		//init list
		walkers = new List<walker>();

		InitMapValues();

		//create a walker 
		walker walker1 = new walker();
		walker1.dir = RandomDirection();
		
		walker walker2 = new walker();
		walker2.dir = RandomDirection();


		//find center of grid
		Vector2 spawnPos1 = new Vector2(Mathf.RoundToInt(Random.Range(0, floorWidth-1)),
										Mathf.RoundToInt(Random.Range(0, floorHeight-1)));
		walker1.pos = spawnPos1;
		
		Vector2 spawnPos2 = new Vector2(Mathf.RoundToInt(Random.Range(0, floorWidth-1)),
										Mathf.RoundToInt(Random.Range(0, floorHeight-1)));
		walker2.pos = spawnPos2;
		
		//add walker to list
		walkers.Add(walker1);
		walkers.Add(walker2);
	}

	void InitMapValues()
	{
		mapValues = new Dictionary<GridSpace, GameObject>();

		mapValues[GridSpace.floor] = floorObj;
		mapValues[GridSpace.wall] = emptyObj;
		mapValues[GridSpace.empty] = emptyObj;
	}

	void CreateRooms()
	{
		int maxRooms = 10;

		for (int r = 0; r < maxRooms; r++)
		{
			int x; 
			int y;
			do {
				x = Random.Range(0, floorWidth - 2);
				y = Random.Range(0, floorHeight - 2);
			} while (grid[x, y] != GridSpace.empty);

			print(x.ToString() + ", " + y.ToString());

			int maxRoomWidth = Random.Range(5, floorWidth / 4);
			int maxRoomHeight = Random.Range(5, floorHeight / 4);

			for (int i = x - maxRoomWidth; i < x + maxRoomWidth; i++)
			{
				for (int j = y - maxRoomHeight; j < y + maxRoomHeight; j++)
				{
					int iInRange = Mathf.Clamp(i, 1, floorWidth - 3);
					int jInRange = Mathf.Clamp(j, 1, floorHeight - 3);

					if(!(grid[iInRange, jInRange] == GridSpace.floor))
						grid[iInRange, jInRange] = GridSpace.floor;
					else
						grid[iInRange, jInRange] = GridSpace.empty;
				}
			}
		}
	}


	void CreateFloors()
	{
		int iterations = 0;//loop will not run forever
		do
		{
			//create floor at position of every walker
			foreach (walker myWalker in walkers)
			{
				grid[(int)myWalker.pos.x, (int)myWalker.pos.y] = GridSpace.floor;
			}
			//chance: destroy walker
			int numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++)
			{
				//only if its not the only one, and at a low chance
				if (Random.value < chanceWalkerDestoy && walkers.Count > 1)
				{
					walkers.RemoveAt(i);
					break; //only destroy one per iteration
				}
			}
			//chance: walker pick new direction
			for (int i = 0; i < walkers.Count; i++)
			{
				if (Random.value < chanceWalkerChangeDir)
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
				if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
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
				thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, floorWidth - 2);
				thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, floorHeight - 2);
				walkers[i] = thisWalker;
			}
			//check to exit loop
			if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
			{
				break;
			}
			iterations++;
		} while (iterations < 100000);
	}
	void CreateWalls()
	{
		//loop though every grid space
		for (int x = 0; x < floorWidth - 1; x++)
		{
			for (int y = 0; y < floorHeight - 1; y++)
			{
				//if theres a floor, check the spaces around it
				if (grid[x, y] == GridSpace.floor)
				{
					//if any surrounding spaces are empty, place a wall
					if (grid[x, y + 1] == GridSpace.empty)
					{
						grid[x, y + 1] = GridSpace.wall;
					}
					if (grid[x, y - 1] == GridSpace.empty)
					{
						grid[x, y - 1] = GridSpace.wall;
					}
					if (grid[x + 1, y] == GridSpace.empty)
					{
						grid[x + 1, y] = GridSpace.wall;
					}
					if (grid[x - 1, y] == GridSpace.empty)
					{
						grid[x - 1, y] = GridSpace.wall;
					}
				}
			}
		}
	}
	void RemoveSingleWalls()
	{
		//loop though every grid space
		for (int x = 0; x < floorWidth - 1; x++)
		{
			for (int y = 0; y < floorHeight - 1; y++)
			{
				//if theres a wall, check the spaces around it
				if (grid[x, y] == GridSpace.wall)
				{
					//assume all space around wall are floors
					bool allFloors = true;
					//check each side to see if they are all floors
					for (int checkX = -1; checkX <= 1; checkX++)
					{
						for (int checkY = -1; checkY <= 1; checkY++)
						{
							if (x + checkX < 0 || x + checkX > floorWidth - 1 ||
								y + checkY < 0 || y + checkY > floorHeight - 1)
							{
								//skip checks that are out of range
								continue;
							}
							if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
							{
								//skip corners and center
								continue;
							}
							if (grid[x + checkX, y + checkY] != GridSpace.floor)
							{
								allFloors = false;
							}
						}
					}
					if (allFloors)
					{
						grid[x, y] = GridSpace.floor;
					}
				}
			}
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
	void Spawn(float x, float y, GameObject toSpawn)
	{
		//find the position to spawn
		Vector2 offset = roomSizeWorldUnits / 2.0f;
		Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;
		//spawn object
		Instantiate(toSpawn, spawnPos, Quaternion.identity).transform.SetParent(boardHolder);
	}
}