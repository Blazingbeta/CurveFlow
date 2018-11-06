using System.Collections;
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
		[SerializeField] LayerMask m_grabMask;
		public override void Cast(PlayerController owner)
		{
			owner.StartCoroutine(GrabProjectiles(owner));
		}
		private IEnumerator GrabProjectiles(PlayerController owner)
		{
			Collider[] enemyProjectiles = Physics.OverlapSphere(owner.transform.position, m_radius, m_grabMask, QueryTriggerInteraction.Collide);
			//Do some sort of visual here
			if(enemyProjectiles.Length == 0)
			{
				//Spell has not gotten a projectile, don't lock casting and just let the whiff animation play
				yield return null;
			}
			else
			{
				yield return null;
				//Spell has gotten at least one projectile, grab them and enter into the special mode for it
				owner.m_abilityManager.m_isCasting = true;
				//Disable their projectileMovement scripts and then pull all them in all fancy like
				for (int j = 0; j < enemyProjectiles.Length; j++)
				{
					enemyProjectiles[j].gameObject.SetActive(false);
				}
				owner.m_abilityManager.SetGrab(enemyProjectiles.Length);
				owner.m_abilityManager.m_isCasting = false;

			}
		}
	}
}