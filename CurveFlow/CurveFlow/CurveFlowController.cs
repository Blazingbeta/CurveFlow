﻿using System;
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
		public CurveFlowController(string settingsXML)
		{
			//Parses the settings file
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(settingsXML);
			//Do things on the settings node here

			m_curve = new MicroCurve(doc);
		}
		/// <summary>
		/// Sets up CurveFlow's logging system to a custom callback function
		/// </summary>
		/// <param name="log">The Callback function where the string will be pushed</param>
		/// <param name="messageTypeMask">Bitmask of the MessageTypes that will be sent</param>
		public void InitializeLog(LogCallback log, MessageType messageTypeMask)
		{
			CFLog.SetupLog(messageTypeMask, log);
		}
		/// <summary>
		/// Creates an XML string of the default settings
		/// </summary>
		/// <returns>Settings in a string format</returns>
		public static string GenerateSettings()
		{
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
				writer.WriteStartElement("Settings");

				writer.WriteStartElement("MicroCurve");
					writer.WriteElementString("Algorithm", "[x] * Sin([t])");
					writer.WriteElementString("PrecompileExpression", "true");
				writer.WriteEndElement();

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
			return sb.ToString();
		}
		#region Profile
		public void CreateNewProfile(TrackedValue[] trackedValues)
		{
			m_profile = new CFProfile(trackedValues);
		}
		public void LoadProfile(Stream profileStream)
		{
			IFormatter formatter = new BinaryFormatter();
			m_profile = (CFProfile)formatter.Deserialize(profileStream);
			//new CFProfile((TrackedValue[])formatter.Deserialize(profileStream));
		}
		public void SaveProfile(Stream outputStream)
		{
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(outputStream, m_profile);
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
		public string Evaluate(OutputQuery query, float desiredDifficulty)
		{
			return query.CalculateOptimalSelection(desiredDifficulty, m_profile.GetAllValues());
		}
		public string EvaluateOnCurve(OutputQuery query, float desiredDifficulty, float time)
		{
			float difficulty = m_curve.EvaluateExpression(desiredDifficulty, time);
			return query.CalculateOptimalSelection(difficulty, m_profile.GetAllValues());
		}
		#endregion
		#region DebugMethods
		public void DebugLogConsole(MessageType type)
		{
			CFLog.SendMessage("This is a message!", type);
		}
		public void DebugChangeValues()
		{
			m_profile.AppendValue("Parry", 0.66f);
			m_profile.AppendValue("Health", 1.0f);
		}
		public float DebugTestMicroCurve(float x, float t)
		{

			Stopwatch sw = new Stopwatch();
			sw.Start();
			float output = m_curve.EvaluateExpression(x,t);
			sw.Stop();
			CFLog.SendMessage(sw.Elapsed.ToString(), MessageType.STATUS);
			return output;
		}
		#endregion
	}
}
