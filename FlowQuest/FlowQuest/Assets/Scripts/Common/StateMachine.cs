using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
	public delegate T Action();
	private Dictionary<T, Action> m_actions = new Dictionary<T, Action>();
	public T State {get; set;}
	public Action this[T State] 
	{ 	get { return m_actions[State]; }
		set { m_actions[State] = value; }
	}
	public void Update()
	{
		State = m_actions[State]();
	}
}
