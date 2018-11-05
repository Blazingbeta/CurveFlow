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
	public int m_currentHealth;

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
		m_currentHealth -= damage;
		m_healthText.text = m_currentHealth.ToString();
		m_healthImage.fillAmount = (float)m_currentHealth/m_maxHealth;
		//TODO dead
	}
	private void OnTriggerEnter(Collider other) 
	{
		ProjectileMovement proj = other.gameObject.GetComponent<ProjectileMovement>();
		if(proj != null)
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
