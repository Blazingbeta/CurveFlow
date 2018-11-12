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
	// Use this for initialization
	void Awake ()
	{
		GameObject tile = Resources.Load("TileSets/DDungeon/TileIIWalls") as GameObject;
		Instantiate(tile, Vector3.zero, Quaternion.identity, surface.transform);
		Instantiate(tile, Vector3.right * 30.0f, Quaternion.identity, surface.transform);
		surface.BuildNavMesh();
	}
}
