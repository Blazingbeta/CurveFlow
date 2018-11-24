using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	[SerializeField] public float m_speed;
	[SerializeField] GameObject m_impactFX = null;
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
			CollideWithObject();
		}
	}
	public void CollideWithObject()
	{
		ParticleSystem[] particles = transform.GetComponentsInChildren<ParticleSystem>();
		GameObject impactFX = null;
		if (m_impactFX) impactFX = Instantiate(m_impactFX, transform.position, transform.rotation);
		for (int j = 0; j < particles.Length; j++)
		{
			if (impactFX)
			{
				particles[j].transform.parent = impactFX.transform;
			}
			else
			{
				particles[j].transform.parent = null;
			}
			particles[j].Stop();
		}
		Destroy(gameObject);
	}
}
