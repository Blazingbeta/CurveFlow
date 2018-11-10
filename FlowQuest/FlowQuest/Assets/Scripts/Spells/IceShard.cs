using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
	[CreateAssetMenu(fileName = "IceShard", menuName = "Spell/IceShard", order = 2)]
	public class IceShard : Spell
	{
		[SerializeField] GameObject m_projectilePrefab = null;
		[SerializeField] float m_projectileSpeed = 8.0f;
		public override void Cast(PlayerController owner)
		{
			ProjectileMovement proj = Instantiate(m_projectilePrefab, owner.transform.position + 
				(owner.transform.rotation * owner.m_projectileSpawnOffset), owner.transform.rotation).GetComponent<ProjectileMovement>();
			proj.m_speed = m_projectileSpeed;
			proj.m_damage = m_damage;
		}
	}
}