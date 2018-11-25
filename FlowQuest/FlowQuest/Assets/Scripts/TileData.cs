using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TileData", menuName = "TileData", order = 0)]
public class TileData : ScriptableObject
{
	[SerializeField] public GameObject m_prefab;
	[SerializeField] public EnemySpawnData[] m_enemies;
	[SerializeField] public Vector3Int[] m_doorways;
	[HideInInspector] public GameObject m_instancedPrefab;
	[HideInInspector] public Transform m_exitDoors;
	public bool AcceptsDirection(Vector3Int dir)
	{
		for(int j = 0; j < m_doorways.Length; j++)
		{
			if(m_doorways[j] == dir * -1)
			{
				return true;
			}
		}
		return false;
	}
}
[System.Serializable]
public struct EnemySpawnData
{
	[SerializeField] public GameObject EnemyPrefab;
	[SerializeField] public Vector3 SpawnPosition;
}