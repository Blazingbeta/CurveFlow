using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy 
{
	[SerializeField] protected int m_attackDamage = 3;
	[SerializeField] protected float m_attackArcAngle = 15.0f;
	[SerializeField] protected float m_attackDistance = 4f;
	[SerializeField] protected float m_attackCloseRadius = 1f;
	[SerializeField] protected Vector3[] m_wayPoints = null;
	[SerializeField] protected float m_reachedRange = 0.5f;

	int m_currentWaypoint = 0;
	private void Start() 
	{
		Initialize();

		m_states[EState.OTHER] = DoNothing; //During Misc Coroutine
		m_states[EState.IDLE] = SearchForPlayer; //While nothing is happening, searches for player
		m_states[EState.MOVING] = ChargeTowardsPlayer; //Moving towards found player
		m_states[EState.PREPARING] = DoNothing; //While attack windup is happening
		m_states[EState.RECOVERY] = DoNothing; //While attack recovery is happening

		m_states.State = EState.IDLE;

		FacePosition(m_wayPoints[0]);
	}
	void Update()
	{
		if (m_isDead) return;
		m_states.Update();
	}
	protected EState SearchForPlayer()
	{
		FacePosition(m_wayPoints[m_currentWaypoint]);
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
		FacePosition(m_playerTransform.position);
		transform.position = Vector3.MoveTowards(transform.position, m_playerTransform.position, 
			m_moveSpeed*Time.deltaTime);
		if((transform.position - m_playerTransform.position).sqrMagnitude < m_reachedRange)
		{
			//Begins the attack
			StartCoroutine(MeleeAttack());
			return EState.PREPARING;
		}
		return EState.MOVING;
	}
	protected IEnumerator MeleeAttack()
	{
		//Attack Windup
		m_anim.SetTrigger("Windup");
		float timer = m_attackWindupTime;
		while(timer > 0)
		{
			yield return null;
			timer -= Time.deltaTime;
			FacePosition(m_playerTransform.position);
		}
		Attack();
		//Attacl Recovery
		m_states.State = EState.RECOVERY;
		yield return new WaitForSeconds(m_attackRecoveryTime);
		//Next State
		if((transform.position - m_playerTransform.position).sqrMagnitude < m_detectionRange)
			m_states.State = EState.MOVING;
		else
			m_states.State = EState.IDLE;
	}
	protected bool Attack()
	{
		m_anim.SetTrigger("Attack");
		Vector3 toPlayer = (m_playerTransform.position - transform.position);
		/*float debugLength = Mathf.Sqrt(m_attackDistance);
		Debug.DrawLine(transform.position, transform.position + (transform.forward * debugLength), Color.green, 3.0f);
		Debug.DrawLine(transform.position, transform.position + (transform.forward * Mathf.Sqrt(m_attackCloseRadius)), Color.red, 3.0f);
		Debug.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(m_currentAngle + m_attackArcAngle, Vector3.up) * Vector3.forward * debugLength), Color.green, 3.0f);
		Debug.DrawLine(transform.position, transform.position + (Quaternion.AngleAxis(m_currentAngle - m_attackArcAngle, Vector3.up) * Vector3.forward * debugLength), Color.green, 3.0f);*/
		float toPlayerAngle = Mathf.Atan2(toPlayer.x, toPlayer.z) * Mathf.Rad2Deg;
		if(toPlayer.sqrMagnitude < m_attackCloseRadius || (toPlayer.sqrMagnitude < m_attackDistance && Mathf.Abs(toPlayerAngle - m_currentAngle) < m_attackArcAngle))
		{
			PlayerController.player.TakeMeleeAttack(m_attackDamage);
			//Hit animation
			return true;
		}
		else
		{
			//Whiff animation
			return false;
		}
	}
}
