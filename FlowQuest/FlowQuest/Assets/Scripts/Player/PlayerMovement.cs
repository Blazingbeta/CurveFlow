using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float m_moveSpeed = 5.0f;
	[SerializeField] float m_lookSpeed = 5.0f;

	public Vector3 m_velocity = Vector3.zero;

	Transform m_transform;

	float m_currentAngle = 90.0f;
	bool m_freezeMovement = false;

	private void Awake()
	{
		m_transform = transform;
	}
	void Update ()
	{
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
	public void FreezeMovement()
	{
		m_freezeMovement = true;
	}
}
