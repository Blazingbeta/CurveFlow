using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController currentCam;

	[SerializeField] LayerMask m_floorMask;

	//Smooth Follow Stuff
	[SerializeField] Transform m_followTarget;
	[SerializeField] float m_smoothTime = 0.3f;
	private Vector3 m_velocity = Vector3.zero;
	private Vector3 m_offset = Vector3.zero;
	private Vector3 m_followPosition = Vector3.zero;

	//Mouse dir offset stuff
	[SerializeField] float m_maxOffset = 3.0f;
	private Vector2 m_centerScreen = Vector2.zero;

	//Debug View Stuff
	private bool m_debugViewEnable = false;
	public bool DebugViewEnable
	{
		get { return m_debugViewEnable; }
		set
		{
			m_debugViewEnable = value;
			if (m_debugViewEnable)
			{
				//Mouse position plane, disabled to prevent ui blocking
				transform.GetChild(0).gameObject.SetActive(false);
				Time.timeScale = 0.0f;
				transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				m_cam.orthographicSize = m_debugOrthoSize;
			}
			else
			{
				transform.GetChild(0).gameObject.SetActive(true);
				Time.timeScale = 1.0f;
				transform.rotation = Quaternion.Euler(60f, 0f, 0f);
				m_cam.orthographicSize = 12;
			}
		}
	}
	private Vector3 m_debugCurrentPosition = Vector3.up * 10f;
	[SerializeField] float m_debugViewSpeed = 8f;
	[SerializeField] int m_debugOrthoSize = 20;

	private Camera m_cam;
	private void Awake()
	{
		currentCam = this;
		m_cam = GetComponent<Camera>();
		m_offset = transform.position - m_followTarget.position;

		m_centerScreen.x = Screen.width / 2.0f;
		m_centerScreen.y = Screen.height / 2.0f;
	}
	private void Update()
	{
		if (m_debugViewEnable)
		{
			if (Input.GetKeyDown(KeyCode.PageUp))
			{
				m_debugOrthoSize+= 3;
				m_cam.orthographicSize = m_debugOrthoSize;
			}
			else if (Input.GetKeyDown(KeyCode.PageDown))
			{
				m_debugOrthoSize-= 3;
				m_cam.orthographicSize = m_debugOrthoSize;
			}
			Vector3 velocity = Vector3.zero;
			if (Input.GetKey(KeyCode.W)) velocity.z += 1;
			if (Input.GetKey(KeyCode.S)) velocity.z -= 1;
			if (Input.GetKey(KeyCode.D)) velocity.x += 1;
			if (Input.GetKey(KeyCode.A)) velocity.x -= 1;
			velocity *= Time.unscaledDeltaTime * m_debugViewSpeed;
			if (Input.GetKey(KeyCode.LeftShift)) velocity *= 3;
			m_debugCurrentPosition += velocity;
			transform.position = m_debugCurrentPosition;
		}
		else
		{
			Vector3 targetPos = m_followTarget.position + m_offset;
			m_followPosition = Vector3.SmoothDamp(m_followPosition, targetPos, ref m_velocity, m_smoothTime);

			Vector2 offsetDir = (((Vector2)Input.mousePosition - m_centerScreen) / m_centerScreen);
			offsetDir *= m_maxOffset;
			Vector3 mouseOffsetVec = new Vector3(offsetDir.x, 0, offsetDir.y);

			transform.position = m_followPosition + mouseOffsetVec;
		}
	}
	public Vector3 GetLookPosition()
	{
		//Also updates the camera look offset
		Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 50.0f, m_floorMask))
		{
			return hit.point;
		}
		return Vector3.zero;
	}
}
