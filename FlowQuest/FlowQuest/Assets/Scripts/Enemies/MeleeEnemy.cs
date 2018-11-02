using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy 
{
	[SerializeField] Vector3[] m_wayPoints = null;
	[SerializeField] float m_reachedRange = 0.5f;
	[SerializeField] float m_detectionRange = 2.0f;
	[SerializeField] float m_attackWindupTime = 1.0f;
	[SerializeField] float m_attackRecoveryTime = 2.0f;
	private Transform m_playerTransform = null;

	int m_currentWaypoint = 0;
	private void Start() 
	{
		m_states[EState.OTHER] = DoNothing; //During Misc Coroutine (shouldn't be used?)
		m_states[EState.ATTACKING] = DoNothing; //While attack windup is happening
		m_states[EState.RECOVERY] = DoNothing; //While attaack recovery is happening
		m_states[EState.IDLE] = SearchForPlayer; //While nothing is happening, searches for player
		m_states[EState.MOVING] = ChargeTowardsPlayer; //Moving towards found player
		m_states[EState.PREPARING] = PrepareAttack; //Begins attack prep (only in state for one frame)

		m_states.State = EState.IDLE;

		m_playerTransform = PlayerController.player.transform;
	}
	void Update()
	{
		m_states.Update();
	}
	protected EState SearchForPlayer()
	{
		transform.position = Vector3.MoveTowards(transform.position, m_wayPoints[m_currentWaypoint], 
			m_moveSpeed*Time.deltaTime);
		if((transform.position - m_wayPoints[m_currentWaypoint]).sqrMagnitude < .3f)
		{
			m_currentWaypoint++;
			m_currentWaypoint %= m_wayPoints.Length;
		}
		//Check if player is found
		if((transform.position - m_playerTransform.position).sqrMagnitude < m_detectionRange)
		{
			return EState.MOVING;
		}
		return EState.IDLE;
	}
	protected EState ChargeTowardsPlayer()
	{
		transform.position = Vector3.MoveTowards(transform.position, m_playerTransform.position, 
			m_moveSpeed*Time.deltaTime);
		if((transform.position - m_playerTransform.position).sqrMagnitude < m_reachedRange)
		{
			return EState.PREPARING;
		}
		return EState.MOVING;
	}
	protected EState PrepareAttack()
	{
		StartCoroutine(MeleeAttack());
		return EState.ATTACKING;
	}
	protected IEnumerator MeleeAttack()
	{
		//Attack Windup
		yield return new WaitForSeconds(m_attackWindupTime);
		Debug.Log("Get Attacked Nerd");
		//Attacl Recovery
		m_states.State = EState.RECOVERY;
		yield return new WaitForSeconds(m_attackRecoveryTime);
		//Next State
		if((transform.position - m_playerTransform.position).sqrMagnitude < m_detectionRange)
			m_states.State = EState.MOVING;
		else
			m_states.State = EState.IDLE;
	}
}
