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
	public static WorldController i;

	public static string ProfileName = "DefaultPlayer";
	public static string DungeonName = "DD2";
	public static int RecurseCount = 2;

	[SerializeField] NavMeshSurface surface;
	[SerializeField] GameObject m_worldTextObject;
	[SerializeField] GameObject m_worldCanvas;
	[SerializeField] GameObject m_blankSquare;

	private static readonly Vector3 CANVASSTARTPOS = new Vector3(0f, -40f, -3.5f);
	private static readonly float CANVASCOORDOFFSET = 30.0f;
	private static readonly float TILESIZE = 30f;

	Dictionary<Coordinate, TileData> m_currentMap = new Dictionary<Coordinate, TileData>();

	private int m_currentEnemyCount = 0;
	private int m_remainingRooms = 0;
	void Awake ()
	{
		i = this;
		CurveFlowManager.Initialize(DungeonName + "Tiles");
		CurveFlowManager.SetGUIValues(GameObject.Find("TrackedValuesPanel").transform);

		BuildMap(RecurseCount, new Coordinate());
		m_remainingRooms = m_currentMap.Keys.Count-1;

		surface.BuildNavMesh();
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			m_worldCanvas.gameObject.SetActive(!m_worldCanvas.gameObject.activeInHierarchy);
			CameraController.currentCam.DebugViewEnable = m_worldCanvas.gameObject.activeInHierarchy;
		}
	}
	public void StartCombat(Transform tile)
	{
		SetBlockingDoors(true);
		Enemy[] enems = tile.GetChild(1).GetComponentsInChildren<Enemy>();
		m_currentEnemyCount = enems.Length;
		Transform playerTransform = PlayerController.player.transform;
		for(int j = 0; j < enems.Length; j++)
		{
			enems[j].PlayerEnterRoom(playerTransform);
		}
	}
	public void EnemyKilled()
	{
		m_currentEnemyCount--;
		if(m_currentEnemyCount == 0)
		{
			EndCombat();
		}
	}
	public void EndCombat()
	{
		SetBlockingDoors(false);
		m_remainingRooms--;
		if(m_remainingRooms == 0)
		{
			Debug.Log("Floor Cleared!");
			SetupBossFight();
		}
	}
	public void SetupBossFight()
	{
		for(int j = 0; j < surface.transform.childCount; j++)
		{
			surface.transform.GetChild(j).gameObject.SetActive(false);
		}
		PlayerController.player.transform.position = Vector3.zero;

		CurveFlowManager.LoadQuery(DungeonName + "Bosses");
		TileData tile = Instantiate(Resources.Load("TileSets/" + DungeonName + '/' + CurveFlowManager.Query(0.0f)) as TileData);
		Instantiate(tile.m_prefab, Vector3.zero, Quaternion.identity).transform.GetChild(1).GetChild(0).GetComponent<Enemy>().PlayerEnterRoom(PlayerController.player.transform);

		surface.BuildNavMesh();

		//Boss is already spawned, enemies in the tile are actual spawnpoints to be used by group selection
		CurveFlowManager.LoadQuery(DungeonName + "BossMinions");
		string[] minions = CurveFlowManager.GroupQuery(0.0f, tile.m_enemies.Length);
		for(int j = 0; j < minions.Length; j++)
		{
			Debug.Log(j + ": " + minions[j]);
			Enemy enem = Instantiate(Resources.Load("Enemies/" + minions[j]) as GameObject, tile.m_enemies[j].SpawnPosition, Quaternion.identity).GetComponent<Enemy>();
			enem.PlayerEnterRoom(PlayerController.player.transform);
		}
	}
	private void SetBlockingDoors(bool isBlocking)
	{
		foreach(TileData tile in m_currentMap.Values)
		{
			tile.m_exitDoors.gameObject.SetActive(isBlocking);
		}
	}
	private void OnApplicationQuit()
	{
		CurveFlowManager.SaveProfile();
	}
	void BuildMap(int recurseCount, Coordinate current)
	{
		TileData entrance = Resources.Load("TileSets/" + DungeonName + "/TileEntrance") as TileData;
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
		TileData tile = Instantiate(Resources.Load("TileSets/" + DungeonName +  '/' + CurveFlowManager.QueryOnCurve(0.25f, Random.Range(0.0f, 2.0f))) as TileData);
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
		Vector3 canvasPos = CANVASSTARTPOS;//m_worldCanvas.transform.TransformPoint(CANVASSTARTPOS);
		canvasPos.x += current.x * CANVASCOORDOFFSET;
		canvasPos.y += current.y * CANVASCOORDOFFSET;
		canvasPos.y += 40f;
		GameObject worldText = Instantiate(m_worldTextObject, Vector3.up * 3, m_worldCanvas.transform.rotation, m_worldCanvas.transform);
		worldText.GetComponent<RectTransform>().anchoredPosition = canvasPos;
		worldText.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = CurveFlowManager.LastMessage;

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
