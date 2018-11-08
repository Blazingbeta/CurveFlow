using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace Spells
{
	public class Spell : ScriptableObject
	{
		[SerializeField] public Texture m_uiImage = null;
		[SerializeField] public int m_damage = 3;
		[SerializeField] public float m_cooldown = 1.0f;
		[SerializeField] public int m_manaCost = 5;
		[SerializeField] public bool m_canHold = true;
		[SerializeField] public bool m_displayCooldown = false;

		[HideInInspector] public bool m_onCooldown = false;
		[HideInInspector] public TMP_Text m_cooldownText = null;
		[HideInInspector] public RawImage m_cooldownImage = null;
		public void InitializeSpell(Transform UISpell)
		{
			m_cooldownImage = UISpell.GetComponent<RawImage>();
			m_cooldownImage.texture = m_uiImage;
			m_cooldownImage.color = Color.white;

			m_cooldownText = UISpell.GetChild(0).GetComponent<TMP_Text>();
			m_cooldownText.gameObject.SetActive(m_displayCooldown);
		}
		public virtual void Cast(PlayerController owner)
		{
			Debug.Log("Generic Spell Cast called");
		}
	}
}