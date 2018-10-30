using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
	[CreateAssetMenu(fileName = "MagicMissle", menuName = "Spell/MagicMissle", order = 1)]
	public class MagicMissle : Spell
	{
		[SerializeField] GameObject m_projectilePrefab = null;
		public override void Cast(PlayerController owner)
		{
			Instantiate(m_projectilePrefab, owner.transform.position, owner.transform.rotation);
		}
	}
}