﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spells;
public class AbilityManager : MonoBehaviour
{
	PlayerController m_controller;

	[SerializeField] private GameObject m_grabOrb = null;
	Dictionary<string, Spell> m_spells;

	[SerializeField] public int m_maxMana = 100;
	private int m_currentMana;
	public int Mana
	{
		get{return m_currentMana;}
		set
		{
			m_currentMana = Mathf.Min(m_maxMana, value);
			m_manaText.text  = m_currentMana.ToString();
			m_manaImage.fillAmount = (float)m_currentMana/m_maxMana;
		}
	}
	[SerializeField] private float m_manaGainTime = 0.4f;
	[SerializeField] private int m_manaPerTick = 1;
	private float m_manaTimer = 0.0f;
	private TMP_Text m_manaText = null;
	private Image m_manaImage =  null;

	[HideInInspector] public bool m_isCasting = false;
	private bool m_isHolding = false;
	private int  m_holdCount = 0;

	private void Awake()
	{
		m_controller = GetComponent<PlayerController>();
		m_spells = new Dictionary<string, Spell>();
		Transform canvas = GameObject.Find("Canvas").transform;
		m_manaImage = canvas.Find("ManaImage").GetComponent<Image>();
		m_manaText = m_manaImage.transform.GetChild(0).GetComponent<TMP_Text>();

		//TODO remove debug
		m_spells.Add("PrimaryFire", (Spell)Instantiate(Resources.Load("Spells/MagicMissle1")));
		m_spells.Add("SecondaryFire", (Spell)Instantiate(Resources.Load("Spells/IceShard1")));
		m_spells.Add("Dodge", (Spell)Instantiate(Resources.Load("Spells/Dodge")));
		m_spells.Add("Grab", (Spell)Instantiate(Resources.Load("Spells/Grab")));

		Transform abilityPanel = canvas.Find("AbilityPanel");
		m_spells["PrimaryFire"].InitializeSpell(abilityPanel.Find("PrimaryFire"));
		m_spells["SecondaryFire"].InitializeSpell(abilityPanel.Find("SecondaryFire"));
		m_spells["Dodge"].InitializeSpell(abilityPanel.Find("Dodge"));
		m_spells["Grab"].InitializeSpell(abilityPanel.Find("Grab"));

		Mana = m_maxMana;
	}
	private void Update()
	{
		if(m_controller.m_isDead) return;
		if (m_isCasting) return;
		else if (m_isHolding)
		{
			HoldInputs();
		}
		else
		{
			m_manaTimer += Time.deltaTime;
			while(m_manaTimer > m_manaGainTime)
			{
				Mana += m_manaPerTick;
				m_manaTimer -= m_manaGainTime;
			}
			foreach (string inputName in m_spells.Keys)
			{
				Spell spell = m_spells[inputName];
				if (spell.m_onCooldown) continue;
				if ((spell.m_canHold && Input.GetButton(inputName)) ||
					(!spell.m_canHold && Input.GetButtonDown(inputName)))
				{
					if(Mana >= spell.m_manaCost)
					{
						CastSpell(spell);
						Mana -= spell.m_manaCost;
					}
				}
			}
		}
	}
	private void HoldInputs()
	{
		if (Input.GetButtonDown("PrimaryFire"))
		{
			StartCoroutine(ThrowHeldAttacks());
		}
		else if (Input.GetButtonDown("SecondaryFire"))
		{
			StartCoroutine(ConsumeHeldAttacks());
		}
	}
	private void CastSpell(Spell spell)
	{
		spell.Cast(m_controller);
		StartCoroutine(SpellCooldown(spell));
	}
	public void SetGrab(int count)
	{
		m_isHolding = true;
		m_holdCount = count;
		//TODO spawn visual in front of player
	}
	private IEnumerator SpellCooldown(Spell spell)
	{
		spell.m_onCooldown = true;
		spell.m_cooldownImage.color = new Color(.5f, .5f, .5f, .5f);
		float timeRemaining = spell.m_cooldown;
		while(timeRemaining > 0)
		{
			timeRemaining -= Time.deltaTime;
			spell.m_cooldownText.text = timeRemaining.ToString("0.0");
			yield return null;
		}
		spell.m_cooldownText.text = "";
		spell.m_cooldownImage.color = new Color(1f, 1f, 1f, 1f);
		spell.m_onCooldown = false;
	}
	private IEnumerator ConsumeHeldAttacks()
	{
		m_isCasting = true;
		Grab grab = (Grab)m_spells["Grab"];
		Mana = Mana + grab.m_baseManaGained + (grab.m_manaPerHold * m_holdCount);
		yield return new WaitForSeconds(0.2f);
		m_isCasting = false;
		m_isHolding = false;
	}
	private IEnumerator ThrowHeldAttacks()
	{
		m_isCasting = true;
		ProjectileMovement orb = Instantiate(m_grabOrb, transform.position + (transform.rotation 
			* m_controller.m_projectileSpawnOffset), transform.rotation).GetComponent<ProjectileMovement>();
		Grab grab = (Grab)m_spells["Grab"];
		orb.m_speed = grab.m_baseSpeed + (m_holdCount * grab.m_speedPerHold);
		orb.m_damage = grab.m_damage + (m_holdCount * grab.m_damagePerHold);
		orb.transform.localScale = Vector3.one * (grab.m_baseScale + (grab.m_scalePerHold * m_holdCount));
		yield return new WaitForSeconds(1.0f);
		m_isCasting = false;
		m_isHolding = false;
	}
}
