using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
	[SerializeField] protected float m_despawnTime = 1.0f;
	protected bool m_isDead = false;

	protected StateMachine<EState> m_states = new StateMachine<EState>();
	protected Transform m_playerTransform = null;
	protected Animator m_anim = null;
	protected NavMeshAgent m_agent;

	protected float m_currentAngle;

	protected void Initialize()
	{
		m_anim = transform.GetChild(0).GetComponent<Animator>();
		m_agent = GetComponent<NavMeshAgent>();
		m_agent.speed = m_moveSpeed;
		m_agent.angularSpeed = m_turnSpeed;

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
	public void PlayerEnterRoom(Transform player)
	{
		m_playerTransform = player;
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
	private void OnTriggerEnter(Collider other) 
	{
		ProjectileMovement proj = other.gameObject.GetComponent<ProjectileMovement>();
		if(proj != null && proj.gameObject.layer == 11)
		{
			TakeDamage(proj.m_damage);
			proj.CollideWithObject();
		}
		else
		{
			Debug.Log("Hit by unkown object: " + other.gameObject.name);
		}	
	}
	protected virtual void TakeDamage(int damage)
	{
		if (!m_isDead)
		{
			m_health -= damage;
			if (m_health <= 0)
			{
				m_anim.SetTrigger("Die");
				m_isDead = true;
				enabled = false;
				StartCoroutine(Despawn());
				m_agent.isStopped = true;
				WorldController.i.EnemyKilled();
				//gameObject.SetActive(false);
			}
			else
			{
				m_anim.SetTrigger("Hit");
			}
		}
	}
	protected IEnumerator Despawn()
	{
		yield return new WaitForSeconds(m_despawnTime);
		gameObject.SetActive(false);
	}
}
