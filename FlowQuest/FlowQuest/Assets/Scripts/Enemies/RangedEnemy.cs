using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
	[SerializeField] float m_detectionRange = 5.0f;
	[SerializeField] float m_attackWindupTime = 3.0f;
	[SerializeField] float m_attackRecoveryTime = 3.0f;
	[SerializeField] ERangedAttackType m_attackType;
	[SerializeField] GameObject m_projectilePrefab = null;
	[SerializeField] float m_projectileSpeed = 5.0f;
	[SerializeField] Vector3 m_projectileSpawnPosition = Vector3.zero;
	[SerializeField] float m_attackSpread = 5.0f;

	private void Start()
	{
		Initialize();

		m_states[EState.OTHER] = DoNothing; //During Misc Coroutine
		m_states[EState.IDLE] = WaitForPlayer; //Waiting and looking around

		m_states[EState.PREPARING] = DoNothing; //Attack Windup
		m_states[EState.RECOVERY] = DoNothing; //Attack Recovery

		m_states.State = EState.IDLE;
	}
	private void Update()
	{
		m_states.Update();
	}
	private EState WaitForPlayer()
	{
		if ((transform.position - m_playerTransform.position).sqrMagnitude < m_detectionRange)
		{
			//Start coroutine
			StartCoroutine(RangedAttack());
			return EState.PREPARING;
		}
		return EState.IDLE;
	}
	private IEnumerator RangedAttack()
	{
		m_states.State = EState.PREPARING;
		//Attack Windup
		float timer = m_attackWindupTime;
		while (timer > 0)
		{
			yield return null;
			timer -= Time.deltaTime;
			if(m_attackType == ERangedAttackType.DIRECT)
			{
				FacePosition(m_playerTransform.position);
			}
			else
			{
				FacePosition(GetApproximateAimPosition(m_projectileSpeed));
			}
		}
		Fire();
		//Attacl Recovery
		m_states.State = EState.RECOVERY;
		yield return new WaitForSeconds(m_attackRecoveryTime);
		//Next State
		if ((transform.position - m_playerTransform.position).sqrMagnitude < m_detectionRange)
			StartCoroutine(RangedAttack());
		else
			m_states.State = EState.IDLE;
	}
	private void Fire()
	{
		ProjectileMovement bullet = Instantiate(m_projectilePrefab, transform.TransformPoint(m_projectileSpawnPosition), transform.rotation).GetComponent<ProjectileMovement>();
		bullet.m_speed = m_projectileSpeed;
		bullet.transform.Rotate(0, Random.Range(-m_attackSpread, m_attackSpread), 0);
	}
}
