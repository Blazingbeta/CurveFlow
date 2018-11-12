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
	List<NavMeshDataInstance> m_navMeshInstances = new List<NavMeshDataInstance>();
	// Use this for initialization
	void Awake ()
	{
		SpawnTestTile(Vector3.zero);
		SpawnTestTile(Vector3.right * 30.0f);
	}
	void SpawnTestTile(Vector3 offset)
	{
		Instantiate(Resources.Load("TileSets/Dungeon/Tiles/NavMeshDemo") as GameObject, offset, Quaternion.identity);
		m_navMeshInstances.Add(NavMesh.AddNavMeshData(Resources.Load<NavMeshData>("TileSets/Dungeon/NavMesh/NavMeshDemo"), offset, Quaternion.identity));

	}
	// Update is called once per frame
	void Update ()
	{
		
	}
	private void OnApplicationQuit()
	{
		foreach(NavMeshDataInstance data in m_navMeshInstances)
		{
			NavMesh.RemoveNavMeshData(data);
		}
	}
}
