using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DebugViewTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		transform.GetChild(0).gameObject.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		transform.GetChild(0).gameObject.SetActive(false);
	}
}
