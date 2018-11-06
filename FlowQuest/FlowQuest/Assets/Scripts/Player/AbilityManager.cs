using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spells;
public class AbilityManager : MonoBehaviour
{
	PlayerController m_controller;

	[SerializeField] private GameObject m_grabOrb = null;
	Dictionary<string, Spell> m_spells;

	[SerializeField] public int m_maxMana = 100;
	private int m_currentMana;
	[HideInInspector] public bool m_isCasting = false;
	private bool m_isHolding = false;
	private int  m_holdCount = 0;

	private void Awake()
	{
		m_controller = GetComponent<PlayerController>();
		m_spells = new Dictionary<string, Spell>();

		//TODO remove debug
		m_spells.Add("PrimaryFire", (Spell)Instantiate(Resources.Load("Spells/MagicMissle1")));
		m_spells.Add("SecondaryFire", (Spell)Instantiate(Resources.Load("Spells/IceShard1")));
		m_spells.Add("Dodge", (Spell)Instantiate(Resources.Load("Spells/Dodge")));
		m_spells.Add("Grab", (Spell)Instantiate(Resources.Load("Spells/Grab")));

		GameObject abilityPanel = GameObject.FindGameObjectWithTag("AbilityPanel");
		m_spells["PrimaryFire"].InitializeSpell(abilityPanel.transform.Find("PrimaryFire"));
		m_spells["SecondaryFire"].InitializeSpell(abilityPanel.transform.Find("SecondaryFire"));
		m_spells["Dodge"].InitializeSpell(abilityPanel.transform.Find("Dodge"));
		m_spells["Grab"].InitializeSpell(abilityPanel.transform.Find("Grab"));

		m_currentMana = m_maxMana;
	}
	private void Update()
	{
		if (m_isCasting) return;
		else if (m_isHolding)
		{
			HoldInputs();
		}
		else
		{
			foreach (string inputName in m_spells.Keys)
			{
				Spell spell = m_spells[inputName];
				if (spell.m_onCooldown) continue;
				if ((spell.m_canHold && Input.GetButton(inputName)) ||
					(!spell.m_canHold && Input.GetButtonDown(inputName)))
				{
					CastSpell(spell);
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
	private IEnumerator ConsumeHeldAttacks()
	{
		m_isCasting = true;
		Grab grab = (Grab)m_spells["Grab"];
		m_currentMana = Mathf.Min(m_currentMana + grab.m_baseManaGained + (grab.m_manaPerHold * m_holdCount), m_maxMana);
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
