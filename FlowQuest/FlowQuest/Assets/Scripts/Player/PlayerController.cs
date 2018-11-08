using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public static PlayerController player;
	[SerializeField] public Vector3 m_projectileSpawnOffset = Vector3.zero;
	public AbilityManager m_abilityManager;
	public PlayerMovement m_movement;

	public int m_maxHealth;
	[HideInInspector] public int m_currentHealth;
	[HideInInspector] public bool m_invincible;
	[HideInInspector] public bool m_isDead = false;

	private Image m_healthImage;
	private TMPro.TMP_Text m_healthText;

	private void Awake()
	{
		m_abilityManager = GetComponent<AbilityManager>();
		m_movement = GetComponent<PlayerMovement>();
		if(player != null)
		{
			Debug.Log("Static Playercontroller already exists. Proceeding.");
		}
		player = this;
	}
	private void Start() 
	{
		m_healthImage = GameObject.Find("HealthImage").GetComponent<Image>();
		m_healthText = m_healthImage.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
		m_healthText.text = m_currentHealth.ToString();
	}
	public void TakeDamage(int damage)
	{
		if(m_isDead) return;
		m_currentHealth -= damage;
		if(m_currentHealth <= 0)
			Die();
		m_healthText.text = m_currentHealth.ToString();
		m_healthImage.fillAmount = (float)m_currentHealth/m_maxHealth;
	}
	private void Die()
	{
		m_currentHealth = 0;
		m_isDead = true;
		StartCoroutine(DeathAnimations());
	}
	private IEnumerator DeathAnimations()
	{
		CanvasGroup group = GameObject.Find("DeathPanel").GetComponent<CanvasGroup>();
		//MAGIC NUMBER: Time to delay the deathanimations
		yield return new WaitForSeconds(0.6f);
		//MAGIC NUMBER: time to fade in the death menu
		float MENUFADEINTIME = 1.2f;
		float timer = 0.0f;
		while(timer < MENUFADEINTIME)
		{
			timer += Time.deltaTime;
			group.alpha = timer/MENUFADEINTIME;
			yield return null;
		}
		group.alpha = 1;
	}
	private void OnTriggerEnter(Collider other) 
	{
		ProjectileMovement proj = other.gameObject.GetComponent<ProjectileMovement>();
		if(!m_invincible && proj != null && proj.gameObject.layer == 12)
		{
			TakeDamage(proj.m_damage);
			proj.gameObject.SetActive(false);
		}
		else
		{
			Debug.Log("Hit by unkown object: " + other.gameObject.name);
		}
	}
}
