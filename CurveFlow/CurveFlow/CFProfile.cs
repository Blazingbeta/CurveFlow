using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveFlow
{
	/// <summary>
	/// Stores all of the values that are being tracked and manages them
	/// </summary>
	[Serializable] internal class CFProfile
	{
		Dictionary<string, TrackedValue> m_profile;
		//Will be called both with new trackedValues and when loading old ones
		internal CFProfile(TrackedValue[] trackedValues)
		{
			m_profile = new Dictionary<string, TrackedValue>();
			for(int j = 0; j < trackedValues.Length; j++)
			{
				m_profile.Add(trackedValues[j].m_name, trackedValues[j]);
			}
		}
		internal void AppendValue(string name, float value)
		{
			m_profile[name].AppendValue(value);
		}
		internal void SetValue(string name, float value)
		{
			m_profile[name].SetValue(value);
		}
		internal TrackedValue GetTrackedValue(string name)
		{
			return m_profile[name];
		}
	}
}
