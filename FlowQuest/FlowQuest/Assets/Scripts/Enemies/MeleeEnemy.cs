using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy 
{
	[SerializeField] protected int m_attackDamage = 3;
	[SerializeField] protected float m_attackArcAngle = 15.0f;
	[SerializeField] protected float m_attackDistance = 4f;
	[SerializeField] protected float m_attackCloseRadius = 1f;
	[SerializeField] protected float m_reachedRange = 2.89f;
	[SerializeField] protected float m_wanderRadius = 3.0f;
	[SerializeField] protected float m_wanderTime = 1.0f;

	Vector3 m_startPos;
	float m_timer = 0.0f;
	private void Start() 
	{
		Initialize();

		m_states[EState.OTHER] = DoNothing; //During Misc Coroutine
		m_states[EState.IDLE] = SearchForPlayer; //While nothing is happening, searches for player
		m_states[EState.MOVING] = ChargeTowardsPlayer; //Moving towards found player
		m_states[EState.PREPARING] = DoNothing; //While attack windup is happening
		m_states[EState.RECOVERY] = DoNothing; //While attack recovery is happening

		m_states.State = EState.IDLE;

		m_startPos = transform.position;

		SetRandomWander();
	}
	void Update()
	{
		if (m_isDead) return;
		m_states.Update();
	}
	protected EState SearchForPlayer()
	{
		m_timer += Time.deltaTime;
		if(m_timer > m_wanderTime)
		{
			m_wanderTime = 0;
			SetRandomWander();
		}
		if(m_playerTransform)
		{
			return EState.MOVING;
		}
		return EState.IDLE;
	}
	protected EState ChargeTowardsPlayer()
	{
		/*FacePosition(m_playerTransform.position);
		transform.position = Vector3.MoveTowards(transform.position, m_playerTransform.position, 
			m_moveSpeed*Time.deltaTime);*/
		m_agent.SetDestination(m_playerTransform.position);
		if ((transform.position - m_playerTransform.position).sqrMagnitude < m_reachedRange)
		{
			//Begins the attack
			m_agent.isStopped = true;
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
		m_agent.isStopped = false;
		if ((transform.position - m_playerTransform.position).sqrMagnitude < m_detectionRange)
		{
			m_states.State = EState.MOVING;
		}
		else
		{
			m_states.State = EState.IDLE;
			SetRandomWander();
		}
	}
	protected void SetRandomWander()
	{
		Vector3 newTarget = Random.onUnitSphere * m_wanderRadius;
		newTarget.y = 0;
		m_agent.SetDestination(m_startPos + newTarget);
	}
	protected bool Attack()
	{
		m_anim.SetTrigger("Attack");
		Vector3 toPlayer = (m_playerTransform.position - transform.position);
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
			PlayerController.player.EnemyWhiffAttack();
			return false;
		}
	}
}
