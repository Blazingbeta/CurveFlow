using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CurveFlow
{
	/// <summary>
	/// The master control object for interfacing with all of CurveFlow
	/// The user simply needs to create one of these at runtime and store it
	/// </summary>
    public class CurveFlowController
    {
		internal CFProfile m_profile;
		public CurveFlowController()
		{
			//m_log = new CFLog();
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
		#endregion
	}
}
