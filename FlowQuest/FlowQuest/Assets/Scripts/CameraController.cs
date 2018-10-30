using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController currentCam;

	[SerializeField] LayerMask m_floorMask;

	private Camera m_cam;
	private void Awake()
	{
		currentCam = this;
		m_cam = GetComponent<Camera>();
	}
	public Vector3 GetLookPosition()
	{
		Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 50.0f, m_floorMask))
		{
			return hit.point;
		}
		return Vector3.zero;
	}
}
