using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{	
	[CreateAssetMenu(fileName = "Dodge", menuName = "Spell/Dodge", order = 2)]
	public class Dodge : Spell 
	{
		[SerializeField] float m_speed;
		[SerializeField] float m_minDistance = 1.0f;
		[SerializeField] float m_maxDistance = 5.0f;
		[SerializeField] LayerMask m_wallMask;
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
			if(endDir.sqrMagnitude < m_minDistance * m_minDistance)
			{
				endDir = endDir.normalized * m_minDistance;
			}
			//Raycast to not fly through a wall
			RaycastHit hit;
			if(Physics.Raycast(startPos + owner.m_projectileSpawnOffset, endDir, out hit, m_maxDistance, m_wallMask))
			{
				//If hit a wall, this means that we need to cut the movement short.
				endDir = hit.point - startPos;
				endDir.y = 0;
			}
			float timer = 0.0f;
			float duration = endDir.magnitude / m_speed;
			while(timer < duration)
			{
				timer += Time.deltaTime;
				owner.transform.position = Vector3.Lerp(startPos, startPos + endDir, timer/duration);
				yield return null;
			}
			owner.m_movement.m_freezeMovement = false;
			owner.m_abilityManager.m_isCasting = false;
			owner.m_invincible = false;
		}
	}
}