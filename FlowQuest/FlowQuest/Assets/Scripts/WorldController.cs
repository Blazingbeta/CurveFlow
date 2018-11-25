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
	[SerializeField] GameObject m_worldTextObject;
	[SerializeField] GameObject m_worldCanvas;
	[SerializeField] GameObject m_blankSquare;

	private static readonly Vector3 CANVASSTARTPOS = new Vector3(0f, -40f, -3.5f);
	private static readonly float CANVASCOORDOFFSET = 30.0f;
	private static readonly float TILESIZE = 30f;

	Dictionary<Coordinate, TileData> m_currentMap = new Dictionary<Coordinate, TileData>();
	void Awake ()
	{
		CurveFlowManager.Initialize("DefaultDungeonTiles");
		CurveFlowManager.SetGUIValues(GameObject.Find("TrackedValuesPanel").transform);
		
		BuildMap(5, new Coordinate());

		surface.BuildNavMesh();
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			m_worldCanvas.gameObject.SetActive(!m_worldCanvas.gameObject.activeInHierarchy);
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			SetBlockingDoors();
		}
	}
	bool m_isBlocking = false;
	private void SetBlockingDoors()
	{
		m_isBlocking = !m_isBlocking;
		foreach(TileData tile in m_currentMap.Values)
		{
			tile.m_exitDoors.gameObject.SetActive(m_isBlocking);
		}
	}
	private void OnApplicationQuit()
	{
		CurveFlowManager.SaveProfile();
	}
	void BuildMap(int recurseCount, Coordinate current)
	{
		TileData entrance = Resources.Load("TileSets/DDungeon/TileEntrance") as TileData;
		m_currentMap.Add(current, entrance);
		entrance.m_instancedPrefab = Instantiate(entrance.m_prefab, Vector3.zero, Quaternion.identity, surface.transform);
		entrance.m_exitDoors = entrance.m_instancedPrefab.transform.GetChild(2);

		RecurseMap(recurseCount, current + entrance.m_doorways[0], entrance.m_doorways[0]);

		//Place Exit Blockers
		foreach (Coordinate space in m_currentMap.Keys)
		{
			foreach(Vector3Int dir in m_currentMap[space].m_doorways)
			{
				if(!m_currentMap.ContainsKey(space + dir))
				{
					SpawnBlocker(dir, m_currentMap[space]);
				}
			}
		}
	}
	void SpawnBlocker(Vector3Int dir, TileData parent)
	{
		Vector3 scale;
		if(dir.x == 0)
		{
			scale = new Vector3(10, 2, 2);
		}
		else
		{
			scale = new Vector3(2, 2, 10);
		}
		Vector3 pos = parent.m_instancedPrefab.transform.position + (new Vector3(dir.x*14f, 1, dir.z*14f));
		Instantiate(m_blankSquare, pos, Quaternion.identity, parent.m_instancedPrefab.transform).transform.localScale = scale;
	}
	void RecurseMap(int recurseCount, Coordinate current, Vector3Int direction)
	{
		if (m_currentMap.ContainsKey(current)) return;
		TileData tile = Instantiate(Resources.Load("TileSets/DDungeon/" + CurveFlowManager.QueryOnCurve(0.25f, Random.Range(0.0f, 2.0f))) as TileData);
		m_currentMap.Add(current, tile);
		Quaternion rot = Quaternion.identity;
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
		GameObject inst = Instantiate(tile.m_prefab, Vector3.one * current, rot, surface.transform);
		tile.m_instancedPrefab = inst;
		tile.m_exitDoors = inst.transform.GetChild(2);
		Transform enemyHolder = inst.transform.GetChild(1);
		//Enemy Spawning
		for(int j = 0; j < tile.m_enemies.Length; j++)
		{
			Instantiate(tile.m_enemies[j].EnemyPrefab, inst.transform.TransformPoint(tile.m_enemies[j].SpawnPosition), Quaternion.identity, enemyHolder);
		}
		//Debug Text display
		Vector3 canvasPos = m_worldCanvas.transform.TransformPoint(CANVASSTARTPOS);
		canvasPos.x += current.x * CANVASCOORDOFFSET;
		canvasPos.z += current.y * CANVASCOORDOFFSET;
		Instantiate(m_worldTextObject, canvasPos, m_worldCanvas.transform.rotation, m_worldCanvas.transform)
			.GetComponent<TMPro.TMP_Text>().text = CurveFlowManager.LastMessage;
		if (recurseCount == 0)
		{
			//Close the doorways (how do i find out which ones to close aa
		}
		else
		{
			foreach (Vector3Int dir in tile.m_doorways)
			{
				RecurseMap(recurseCount - 1, current + dir, dir);
			}
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
