using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

namespace CurveFlow
{
	/// <summary>
	/// The master control object for interfacing with all of CurveFlow
	/// The user simply needs to create one of these at runtime and store it
	/// </summary>
    public class CurveFlowController
    {
		internal CFProfile m_profile;
		internal MicroCurve m_curve;
		/// <summary>
		/// Sets up CurveFlow's logging system to a custom callback function
		/// </summary>
		/// <param name="log">The Callback function where the string will be pushed</param>
		/// <param name="messageTypeMask">Bitmask of the MessageTypes that will be sent</param>
		public void InitializeLog(LogCallback log, MessageType messageTypeMask)
		{
			CFLog.SetupLog(messageTypeMask, log);
		}
		public void InitializeCurve(MicroCurveExpression expression)
		{
			m_curve = new MicroCurve(expression);
		}
		/// <summary>
		/// Creates an XML string of the default settings
		/// </summary>
		/// <returns>Settings in a string format</returns>
		#region Profile
		public void CreateNewProfile(TrackedValue[] trackedValues)
		{
			m_profile = new CFProfile(trackedValues);
		}
		public void LoadProfile(string profileXML)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(profileXML);
			m_profile = new CFProfile(doc.SelectSingleNode("/Controller"));
		}
		public string SaveProfile()
		{
			//Build to xml
			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings()
			{
				Indent = true,
				IndentChars = "\t",
				NewLineOnAttributes = true
			};
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("Controller");
				m_profile.ToXML(writer);
				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
			return sb.ToString();
		}
		public float GetCurrentValue(string valueName)
		{
			return m_profile.GetTrackedValue(valueName).m_currentValue;
		}
		public void AppendTrackedValue(string valueName, float nextValue)
		{
			m_profile.AppendValue(valueName, nextValue);
		}
		public void SetTrackedValue(string valueName, float newValue)
		{
			m_profile.SetValue(valueName, newValue);
		}
		#endregion
		#region OutputQuery
		public string Evaluate(OutputQuery query, float desiredChallenge)
		{
			return query.CalculateOptimalSelection(desiredChallenge, m_profile);
		}
		public string EvaluateOnCurve(OutputQuery query, float desiredChallenge, float time)
		{
			float difficulty = m_curve.EvaluateExpression(desiredChallenge, time);
			return query.CalculateOptimalSelection(difficulty, m_profile);
		}
		public string[] EvaluateGroupSelection(OutputQuery query, float desiredChallenge, int count)
		{
			return query.GetGroupBinding(desiredChallenge, m_profile.GetAllValues(), count);
		}
		public string[] EvaluateGroupSelectionOnCurve(OutputQuery query, float desiredChallenge, int count, float time)
		{
			float difficulty = m_curve.EvaluateExpression(desiredChallenge, time);
			return query.GetGroupBinding(difficulty, m_profile.GetAllValues(), count);
		}
		#endregion
	}
}
