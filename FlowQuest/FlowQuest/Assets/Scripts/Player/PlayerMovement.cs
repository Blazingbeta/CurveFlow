using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float m_moveSpeed = 5.0f;
	[SerializeField] float m_lookSpeed = 5.0f;

	public Vector3 m_velocity = Vector3.zero;

	Transform m_transform;
	PlayerController m_controller;
	public Animator m_anim;

	float m_currentAngle = 90.0f;
	[HideInInspector] public bool m_freezeMovement = false;

	private void Awake()
	{
		m_transform = transform;
		m_controller = GetComponent<PlayerController>();
		m_anim = transform.Find("Model").Find("Wizard").GetComponent<Animator>();
	}
	void Update ()
	{
		if(m_controller.m_isDead || CameraController.currentCam.DebugViewEnable) return;
		UpdateLookPosition();
		UpdateMovement();
	}
	void UpdateMovement()
	{
		if (m_freezeMovement) return;
		Vector3 moveDir = Vector3.zero;
		moveDir.x = Input.GetAxis("Horizontal");
		moveDir.z = Input.GetAxis("Vertical");
		if(moveDir.sqrMagnitude > 1)
		{
			moveDir.Normalize();
		}
		m_velocity = moveDir * m_moveSpeed;
		m_transform.position += moveDir * m_moveSpeed * Time.deltaTime;
		//Y drift fix?
		m_transform.position += Vector3.down * m_transform.position.y;
		m_anim.SetBool("isWalking", moveDir.sqrMagnitude != 0f);
	}
	void UpdateLookPosition()
	{
		Vector3 lookPos = CameraController.currentCam.GetLookPosition();
		if (lookPos.sqrMagnitude > 0)
		{
			Vector3 targetDir = (lookPos - m_transform.position);
			float targetAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
			m_currentAngle = Mathf.LerpAngle(m_currentAngle, targetAngle, m_lookSpeed * Time.deltaTime);
			m_transform.rotation = Quaternion.AngleAxis(m_currentAngle, Vector3.up);
		}
	}
}
