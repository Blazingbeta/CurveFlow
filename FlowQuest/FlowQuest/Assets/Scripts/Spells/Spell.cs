using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Spells
{
	public class Spell : ScriptableObject
	{
		[SerializeField] public int m_damage = 3;
		[SerializeField] public float m_cooldown = 1.0f;
		[SerializeField] public bool m_canHold = true;
		[SerializeField] public bool m_displayCooldown = false;

		[HideInInspector] public bool m_onCooldown = false;
		[HideInInspector] public TMP_Text m_cooldownText = null;
		public void InitializeSpell(Transform UISpell)
		{
			m_cooldownText = UISpell.GetChild(0).GetComponent<TMP_Text>();
			m_cooldownText.gameObject.SetActive(m_displayCooldown);
		}
		public virtual void Cast(PlayerController owner)
		{
			Debug.Log("Generic Spell Cast called");
		}
	}
}