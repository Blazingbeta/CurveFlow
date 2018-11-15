using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveFlow
{
	public enum MessageType { STATUS = 1, DEBUG = 2, ERROR = 4}
	public enum ValueType { ADDITIVE, AVERAGE, SET}
	public delegate void LogCallback(string logMessage, MessageType type);
	public delegate float MicroCurveExpression(float x, float t);
	[Serializable] public class TrackedValue
	{
		internal readonly float m_min;
		internal readonly float m_max;
		internal readonly string m_name;
		internal readonly ValueType m_type;
		internal int m_additionCount = 0;
		internal float m_currentValue;
		public TrackedValue(float min, float max, string name, ValueType type)
		{
			m_min = min;
			m_max = max;
			m_name = name;
			m_type = type;
		}
		internal void AppendValue(float nextValue)
		{
			float oldValue = m_currentValue;
			switch(m_type)
			{
				case ValueType.ADDITIVE:
					SetCurrentValueInBounds(m_currentValue + nextValue);
					m_additionCount++;
					break;
				case ValueType.AVERAGE:
					m_currentValue = ((m_currentValue * m_additionCount) + nextValue) / (m_additionCount + 1);
					m_additionCount++;
					break;
				case ValueType.SET:
					//Log error
					CFLog.SendMessage("Tried to append to set only value: " + m_name, MessageType.ERROR);
					break;
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(m_name);
			sb.Append(" Old Value: ");
			sb.Append(oldValue.ToString("G"));
			sb.Append(" New Value: ");
			sb.Append(m_currentValue);
			sb.Append(" Type: ");
			sb.Append(m_type);

			CFLog.SendMessage(sb.ToString(), MessageType.STATUS);
		}
		internal void SetValue(float newValue)
		{
			SetCurrentValueInBounds(newValue);
			m_additionCount = 1;
		}
		private void SetCurrentValueInBounds(float newValue)
		{
			m_currentValue = Math.Min(m_max, Math.Max(m_min, newValue));
		}
	}
}
