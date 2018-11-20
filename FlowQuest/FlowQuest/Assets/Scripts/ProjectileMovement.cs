using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	[SerializeField] public float m_speed;
	public int m_damage;
	private void Awake()
	{
		Destroy(gameObject, 8f);
	}
	void Update ()
	{
		transform.position += transform.forward * m_speed * Time.deltaTime;
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer  == 15)
		{
			gameObject.SetActive(false);
		}
	}
}
