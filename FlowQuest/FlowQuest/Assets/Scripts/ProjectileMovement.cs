﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	[SerializeField] public float m_speed;
	public int m_damage;
	void Update ()
	{
		transform.position += transform.forward * m_speed * Time.deltaTime;
	}
}
