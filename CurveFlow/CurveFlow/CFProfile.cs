using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CurveFlow
{
	/// <summary>
	/// Stores all of the values that are being tracked and manages them
	/// </summary>
	internal class CFProfile
	{
		Dictionary<string, TrackedValue> m_profile;
		Dictionary<string, List<string>> m_lockedOutputs;
		//Will be called both with new trackedValues and when loading old ones
		internal CFProfile(TrackedValue[] trackedValues)
		{
			m_profile = new Dictionary<string, TrackedValue>();
			m_lockedOutputs = new Dictionary<string, List<string>>();
			for(int j = 0; j < trackedValues.Length; j++)
			{
				m_profile.Add(trackedValues[j].m_name, trackedValues[j]);
			}
		}
		internal CFProfile(XmlNode node)
		{
			m_profile = new Dictionary<string, TrackedValue>();
			XmlNode profileNodes = node.SelectSingleNode("Profile");
			foreach (XmlNode skill in profileNodes.ChildNodes)
			{
				float min = float.Parse(skill.Attributes["Minimum"].InnerText);
				float max = float.Parse(skill.Attributes["Maximum"].InnerText);
				string name = skill.Attributes["Name"].InnerText;
				ValueType valueType = (ValueType)Enum.Parse(typeof(ValueType), skill.Attributes["Type"].InnerText, true);
				TrackedValue value = new TrackedValue(min, max, name, valueType)
				{
					m_currentValue = float.Parse(skill.Attributes["Value"].InnerText),
					m_additionCount = int.Parse(skill.Attributes["AdditionCount"].InnerText)
				};
				m_profile.Add(name, value);
			}
			m_lockedOutputs = new Dictionary<string, List<string>>();
			XmlNode lockNodes = node.SelectSingleNode("LockedValues");
			foreach(XmlNode query in lockNodes.ChildNodes)
			{
				string name = query.Attributes["Name"].InnerText;
				m_lockedOutputs.Add(name, new List<string>());
				foreach(XmlNode locke in query.ChildNodes)
				{
					m_lockedOutputs[name].Add(locke.InnerText);
				}
			}
		}
		internal void ToXML(XmlWriter writer)
		{
			writer.WriteStartElement("Profile");
			foreach(string key in m_profile.Keys)
			{
				writer.WriteStartElement("TrackedValue");
				TrackedValue value = m_profile[key];
				writer.WriteAttributeString("Name", key);
				writer.WriteAttributeString("Minimum", value.m_min.ToString("G"));
				writer.WriteAttributeString("Maximum", value.m_max.ToString("G"));
				writer.WriteAttributeString("Value", value.m_currentValue.ToString("G"));
				writer.WriteAttributeString("Type", value.m_type.ToString());
				writer.WriteAttributeString("AdditionCount", value.m_additionCount.ToString());
				writer.WriteEndElement();
			}
			writer.WriteEndElement();

			writer.WriteStartElement("LockedValues");
			foreach(string key in m_lockedOutputs.Keys)
			{
				writer.WriteStartElement("Query");
				writer.WriteStartAttribute("Name", key);
				foreach(string lockedName in m_lockedOutputs[key])
				{
					writer.WriteElementString("Lock", lockedName);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
		internal void AppendValue(string name, float value)
		{
			if (!m_profile.ContainsKey(name))
			{
				CFLog.SendMessage(name + " is not a registered value.", MessageType.ERROR);
				return;
			}
			m_profile[name].AppendValue(value);
		}
		internal void SetValue(string name, float value)
		{
			if (!m_profile.ContainsKey(name))
			{
				CFLog.SendMessage(name + " is not a registered value.", MessageType.ERROR);
				return;
			}
			m_profile[name].SetValue(value);
		}
		internal TrackedValue GetTrackedValue(string name)
		{
			if (!m_profile.ContainsKey(name))
			{
				CFLog.SendMessage(name + " is not a registered value.", MessageType.ERROR);
				return null;
			}
			return m_profile[name];
		}
		internal TrackedValue[] GetAllValues()
		{
			return m_profile.Values.ToArray();
		}
		internal void LockOutput(string queryName, string outputName)
		{
			if (!m_lockedOutputs.ContainsKey(queryName))
				m_lockedOutputs.Add(queryName, new List<string>());
			m_lockedOutputs[queryName].Add(outputName);
		}
		internal bool IsOutputLocked(string queryName, string outputName)
		{
			return m_lockedOutputs[queryName].Contains(outputName);
		}
	}
}
