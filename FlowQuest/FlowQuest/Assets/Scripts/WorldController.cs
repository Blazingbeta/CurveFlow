using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
 * 
 * On load new level:
 * Get the outputs from curveflow (all done on level load so no runtime changes)
 * instatiate the prefabs
 * create object pools
 * bake the navmesh
 * 
 */
public class WorldController : MonoBehaviour {
	[SerializeField] NavMeshSurface surface;

	private static readonly float TILESIZE = 30f;

	Dictionary<Coordinate, TileData> m_currentMap = new Dictionary<Coordinate, TileData>();
	void Awake ()
	{
		DEBUGTEMP.Add(Resources.Load("TileSets/DDungeon/TileIIWalls") as TileData);
		DEBUGTEMP.Add(Resources.Load("TileSets/DDungeon/Tile=Walls") as TileData);
		DEBUGTEMP.Add(Resources.Load("TileSets/DDungeon/Tile+Corridor") as TileData);
		BuildMap(2, new Coordinate(), Vector3Int.zero);
		surface.BuildNavMesh();
	}
	List<TileData> DEBUGTEMP = new List<TileData>();
	void BuildMap(int recurseCount, Coordinate current, Vector3Int direction)
	{
		if (m_currentMap.ContainsKey(current)) return;
		TileData tile = Instantiate(DEBUGTEMP[Random.Range(0, DEBUGTEMP.Count)]);
		m_currentMap.Add(current, tile);
		Quaternion rot = Quaternion.identity;
		if (!tile.AcceptsDirection(direction))
		{
			//Select a random valid direction to be the new doorway
			Vector3Int oldDoor = tile.m_doorways[Random.Range(0, tile.m_doorways.Length)];
			//Get the new rotation of the tile
			float angle = Vector3.SignedAngle(oldDoor, direction * -1, Vector3.up);
			rot = Quaternion.AngleAxis(angle, Vector3.up);
			//Also change the m_doorways
			for(int j = 0; j < tile.m_doorways.Length; j++)
			{
				Vector3 floatVec = tile.m_doorways[j];
				floatVec = rot * floatVec;
				tile.m_doorways[j] = Vector3Int.RoundToInt(floatVec);
			}
		}
		Instantiate(tile.m_prefab, Vector3.one * current, rot, surface.transform);
		if (recurseCount == 0) return;
		foreach(Vector3Int dir in tile.m_doorways)
		{
			BuildMap(recurseCount - 1, current + dir, dir);
		}
	}
	private struct Coordinate
	{
		public int x;
		public int y;
		public static Vector3 operator* (Vector3 a, Coordinate b)
		{
			a.x *= TILESIZE * b.x;
			a.y = 0f;
			a.z *= TILESIZE * b.y;
			return a;
		}
		public static Coordinate operator+(Coordinate a, Vector3Int b)
		{
			a.x += b.x;
			a.y += b.z;
			return a;
		}
	}
}
