using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplayManager : MonoBehaviour
{
	public Image m_image;

	Color m_baselineColor;
	Color m_currentDingColor;

	[SerializeField] float m_dingTime = 0.4f;
	private float m_currentFillAmount = 0.0f;
	private float m_targetFillAmount = 0.0f;
	private float m_dingTimer = 0.0f;
	bool m_isChanging = false;

	private void Awake()
	{
		m_image = GetComponent<Image>();
		m_baselineColor = m_image.color;
	}
	private void Update()
	{
		if (m_isChanging)
		{
			m_image.fillAmount = Mathf.Lerp(m_targetFillAmount, m_currentFillAmount, m_dingTimer / m_dingTime);
			m_image.color = Color.Lerp(m_baselineColor, m_currentDingColor, m_dingTimer / m_dingTime);
			m_dingTimer -= Time.deltaTime;
			if(m_dingTimer < 0f)
			{
				m_isChanging = false;
				m_image.fillAmount = m_targetFillAmount;
				m_image.color = m_baselineColor;
				m_currentFillAmount = m_targetFillAmount;
			}
		}
	}
	public void SetNewFillAmount(float amount)
	{
		m_targetFillAmount = amount;
		m_currentDingColor = (m_targetFillAmount < m_currentFillAmount) ? Color.gray : Color.white;
		m_dingTimer = m_dingTime;
		m_isChanging = true;
	}
}
