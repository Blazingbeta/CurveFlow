using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
	[CreateAssetMenu(fileName = "MagicMissle", menuName = "Spell/MagicMissle", order = 1)]
	public class MagicMissle : Spell
	{
		[SerializeField] GameObject m_projectilePrefab = null;
		[SerializeField] float m_bulletSpeed = 6f;
		[SerializeField] float m_bulletSpread = 45f;
		public override void Cast(PlayerController owner)
		{
			GameObject bullet = Instantiate(m_projectilePrefab, owner.transform.position + (owner.transform.rotation * owner.m_projectileSpawnOffset), owner.transform.rotation);
			bullet.transform.Rotate(0, Random.Range(-m_bulletSpread, m_bulletSpread), 0);
			bullet.transform.GetChild(0).rotation = Random.rotation;
			ProjectileMovement proj = bullet.GetComponent<ProjectileMovement>();
			proj.m_damage = m_damage;
			proj.m_speed = m_bulletSpeed;
		}
	}
}