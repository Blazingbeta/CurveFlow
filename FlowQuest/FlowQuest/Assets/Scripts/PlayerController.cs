using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public AbilityManager m_abilityManager;
	public PlayerMovement m_movement;
	private void Awake()
	{
		m_abilityManager = GetComponent<AbilityManager>();
		m_movement = GetComponent<PlayerMovement>();
	}
}
