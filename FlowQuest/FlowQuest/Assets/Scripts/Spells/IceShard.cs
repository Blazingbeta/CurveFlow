using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
	[CreateAssetMenu(fileName = "IceShard", menuName = "Spell/IceShard", order = 2)]
	public class IceShard : Spell
	{
		[SerializeField] GameObject m_projectilePrefab = null;
		public override void Cast(PlayerController owner)
		{
			Instantiate(m_projectilePrefab, owner.transform.position, owner.transform.rotation);
		}
	}
}