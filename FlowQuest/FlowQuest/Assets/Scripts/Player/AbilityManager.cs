using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spells;
public class AbilityManager : MonoBehaviour
{

	Dictionary<string, Spell> m_spells;

	PlayerController m_controller;
	private void Awake()
	{
		m_controller = GetComponent<PlayerController>();
		m_spells = new Dictionary<string, Spell>();

		//TODO remove debug
		m_spells.Add("PrimaryFire", (Spell)Instantiate(Resources.Load("Spells/MagicMissle1")));
		m_spells.Add("SecondaryFire", (Spell)Instantiate(Resources.Load("Spells/IceShard1")));
		m_spells.Add("Dodge", (Spell)Instantiate(Resources.Load("Spells/IceShard1")));

		GameObject abilityPanel = GameObject.FindGameObjectWithTag("AbilityPanel");
		m_spells["PrimaryFire"].InitializeSpell(abilityPanel.transform.Find("PrimaryFire"));
		m_spells["SecondaryFire"].InitializeSpell(abilityPanel.transform.Find("SecondaryFire"));
	}
	private void Update()
	{
		foreach(string inputName in m_spells.Keys)
		{
			Spell spell = m_spells[inputName];
			if (spell.m_onCooldown) continue;
			if((spell.m_canHold && Input.GetButton(inputName)) || 
				(!spell.m_canHold && Input.GetButtonDown(inputName)))
			{
				CastSpell(spell);
			}
		}
	}
	private void CastSpell(Spell spell)
	{
		spell.Cast(m_controller);
		StartCoroutine(SpellCooldown(spell));
	}
	private IEnumerator SpellCooldown(Spell spell)
	{
		spell.m_onCooldown = true;
		float timeRemaining = spell.m_cooldown;
		while(timeRemaining > 0)
		{
			timeRemaining -= Time.deltaTime;
			spell.m_cooldownText.text = timeRemaining.ToString("0.0");
			yield return null;
		}
		spell.m_cooldownText.text = "";
		spell.m_onCooldown = false;
	}
}
