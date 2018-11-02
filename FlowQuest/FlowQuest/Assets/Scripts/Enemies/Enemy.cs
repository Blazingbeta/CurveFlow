using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	protected enum EState { OTHER, IDLE, MOVING, PREPARING, ATTACKING, RECOVERY } 

	[SerializeField] protected float m_moveSpeed = 3.0f;
	[SerializeField] protected int m_health = 7;

	protected StateMachine<EState> m_states = new StateMachine<EState>();
	void Update() 
	{
		m_states.Update();
	}
	protected EState DoNothing()
	{
		return m_states.State;
	}
}
