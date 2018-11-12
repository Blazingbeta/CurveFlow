using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{	
	[CreateAssetMenu(fileName = "Dodge", menuName = "Spell/Dodge", order = 2)]
	public class Dodge : Spell 
	{
		[SerializeField] float m_duration = 0.6f;
		[SerializeField] float m_maxDistance = 5.0f;
		public override void Cast(PlayerController owner)
		{
			owner.StartCoroutine(DashRoutine(owner));
		}
		private IEnumerator DashRoutine(PlayerController owner)
		{
			owner.m_abilityManager.m_isCasting = true;
			owner.m_movement.m_freezeMovement = true;
			owner.m_invincible = true;
			Vector3 startPos = owner.transform.position;
			//Get proper end pos
			Vector3 endPos = CameraController.currentCam.GetLookPosition();
			Vector3 endDir = endPos - startPos;
			endDir.y = 0;
			if(endDir.sqrMagnitude > m_maxDistance * m_maxDistance)
			{
				endDir = endDir.normalized * m_maxDistance;
			}
			float timer = 0.0f;
			while(timer < m_duration)
			{
				timer += Time.deltaTime;
				owner.transform.position = Vector3.Lerp(startPos, startPos + endDir, timer/m_duration);
				yield return null;
			}
			owner.m_movement.m_freezeMovement = false;
			owner.m_abilityManager.m_isCasting = false;
			owner.m_invincible = false;
		}
	}
}