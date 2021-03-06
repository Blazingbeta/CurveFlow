﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
	[CreateAssetMenu(fileName = "Grab", menuName = "Spell/Grab", order = 4)]
	public class Grab : Spell 
	{
		[SerializeField] public int m_damagePerHold = 3;
		[SerializeField] public int m_speedPerHold = 3;
		[SerializeField] public int  m_baseSpeed = 3;
		[SerializeField] public int m_baseManaGained = 10;
		[SerializeField] public int m_manaPerHold = 20;
		[SerializeField] public float m_baseScale = 0.3f;
		[SerializeField] public float m_scalePerHold = 0.2f;
		[SerializeField] float m_radius = 3.0f;
		[SerializeField] float m_missRadius = 5.0f;
		[SerializeField] LayerMask m_grabMask;
		ParticleSystem m_grabParticles = null;
		public override void Cast(PlayerController owner)
		{
			if(m_grabParticles == null)
			{
				m_grabParticles = owner.transform.Find("GrabParticle").GetComponent<ParticleSystem>();
				ParticleSystem.ShapeModule shape = m_grabParticles.shape;
				shape.radius = m_radius;
			}
			m_grabParticles.Play();
			owner.StartCoroutine(GrabProjectiles(owner));
		}
		private IEnumerator GrabProjectiles(PlayerController owner)
		{
			Collider[] enemyProjectiles = Physics.OverlapSphere(owner.transform.position, m_radius, m_grabMask, QueryTriggerInteraction.Collide);
			//Do some sort of visual here
			if(enemyProjectiles.Length == 0)
			{
				//If anyprojectiles are near but not caught, minus the grab skill
				enemyProjectiles = Physics.OverlapSphere(owner.transform.position, m_missRadius, m_grabMask, QueryTriggerInteraction.Collide);
				if(enemyProjectiles.Length != 0)
				{
					CurveFlowManager.AppendValue("GrabSkill", 0.0f);
				}
				//Spell has not gotten a projectile, don't lock casting and just let the whiff animation play
				yield return null;
			}
			else
			{
				//Spell has gotten at least one projectile, grab them and enter into the special mode for it
				owner.m_abilityManager.m_isCasting = true;
				float DRAWTIME = 0.6f;
				//Disable their projectileMovement scripts and then pull all them in all fancy like
				for (int j = 0; j < enemyProjectiles.Length; j++)
				{
					ProjectileMovement proj = enemyProjectiles[j].GetComponent<ProjectileMovement>();
					owner.StartCoroutine(DrawInProjectile(owner, proj, DRAWTIME));
					CurveFlowManager.AppendValue("GrabSkill", 1.0f);
				}
				yield return new WaitForSeconds(DRAWTIME);
				owner.m_abilityManager.SetGrab(enemyProjectiles.Length);
				owner.m_abilityManager.m_isCasting = false;
			}
		}
		private IEnumerator DrawInProjectile(PlayerController owner, ProjectileMovement proj, float time)
		{
			proj.enabled = false;
			proj.GetComponent<Collider>().enabled = false;
			float timer = 0f;
			Vector3 startPos = proj.transform.position;
			while(timer < time)
			{
				float percent = timer / time;
				proj.transform.localScale = Vector3.one * (1f-percent);
				proj.transform.position = Vector3.Slerp(startPos, owner.transform.TransformPoint(owner.m_projectileSpawnOffset), percent);
				timer += Time.deltaTime;
				yield return null;
			}
			Destroy(proj.gameObject);
		}
	}
}