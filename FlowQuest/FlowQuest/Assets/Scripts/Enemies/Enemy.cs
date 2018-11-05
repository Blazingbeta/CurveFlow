using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	protected enum EState { OTHER, IDLE, MOVING, PREPARING, ATTACKING, RECOVERY } 
	protected enum ERangedAttackType { DIRECT, APPROXIMATE, PREDICTIVE }
	[SerializeField] protected float m_moveSpeed = 3.0f;
	[SerializeField] protected float m_turnSpeed = 3f;
	[SerializeField] protected int m_health = 7;
	[SerializeField] protected float m_detectionRange = 5.0f;
	[SerializeField] protected float m_attackWindupTime = 3.0f;
	[SerializeField] protected float m_attackRecoveryTime = 3.0f;

	protected StateMachine<EState> m_states = new StateMachine<EState>();
	protected Transform m_playerTransform = null;

	protected float m_currentAngle;

	protected void Initialize()
	{
		m_playerTransform = PlayerController.player.transform;

		m_currentAngle = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
	}
	private void Start()
	{
		Initialize();

		m_states[EState.OTHER] = DoNothing;
		m_states[EState.IDLE] = DoNothing;
		m_states[EState.MOVING] = DoNothing;
		m_states[EState.PREPARING] = DoNothing;
		m_states[EState.ATTACKING] = DoNothing;
		m_states[EState.RECOVERY] = DoNothing;
	}
	void Update() 
	{
		m_states.Update();
	}
	protected EState DoNothing()
	{
		return m_states.State;
	}
	protected void MoveTowardsPosition(Vector3 target)
	{
		transform.position = Vector3.MoveTowards(transform.position, 
			target, m_moveSpeed * Time.deltaTime);
	}
	protected void FacePosition(Vector3 target)
	{
		Vector3 lookDir = target - transform.position;
		if (lookDir.sqrMagnitude > 0)
		{
			float targetAngle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
			m_currentAngle = Mathf.LerpAngle(m_currentAngle, targetAngle, m_turnSpeed * Time.deltaTime);
			transform.rotation = Quaternion.AngleAxis(m_currentAngle, Vector3.up);
		}
	}
	protected Vector3 GetApproximateAimPosition(float projectileSpeed)
	{
		//find time to reach the players current position
		float timeToReach = (m_playerTransform.position - transform.position).magnitude / projectileSpeed;
		//get players position at that time
		Vector3 pos = m_playerTransform.position + (PlayerController.player.m_movement.m_velocity * timeToReach);
		Debug.DrawLine(transform.position, pos, Color.blue);
		return pos;
	}
}
