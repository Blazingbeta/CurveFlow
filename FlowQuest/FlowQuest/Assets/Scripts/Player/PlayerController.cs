using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController player;
	[SerializeField] public Vector3 m_projectileSpawnOffset = Vector3.zero;
	public AbilityManager m_abilityManager;
	public PlayerMovement m_movement;
	private void Awake()
	{
		m_abilityManager = GetComponent<AbilityManager>();
		m_movement = GetComponent<PlayerMovement>();
		if(player != null)
		{
			Debug.Log("Static Playercontroller already exists. Proceeding.");
		}
		player = this;
	}
}
