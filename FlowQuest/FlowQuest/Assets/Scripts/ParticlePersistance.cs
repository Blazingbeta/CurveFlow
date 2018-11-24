﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePersistance : MonoBehaviour
{
	ParticleSystem m_ps;
	private void Awake()
	{
		m_ps = GetComponent<ParticleSystem>();
	}
	private void Update()
	{
		if (m_ps)
		{
			if (!m_ps.IsAlive())
			{
				Destroy(gameObject);
			}
		}
	}
}
