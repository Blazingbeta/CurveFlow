using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spells
{
	public class Spell : ScriptableObject
	{
		[SerializeField] public float m_cooldown = 1.0f;
		[SerializeField] public bool m_canHold = true;
		public bool m_onCooldown = false;
		public virtual void Cast(PlayerController owner)
		{

		}
	}
}