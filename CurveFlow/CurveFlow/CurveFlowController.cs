using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveFlow
{
	/// <summary>
	/// The master control object for interfacing with all of CurveFlow
	/// The user simply needs to create one of these at runtime and store it
	/// </summary>
    public class CurveFlowController
    {
		private CFLog m_log;
		public CurveFlowController()
		{
			m_log = new CFLog();
		}
		/// <summary>
		/// Sets up CurveFlow's logging system to a custom callback function
		/// </summary>
		/// <param name="log">The Callback function where the string will be pushed</param>
		/// <param name="messageTypeMask">Bitmask of the MessageTypes that will be sent</param>
		public void InitializeLog(LogCallback log, MessageType messageTypeMask)
		{
			m_log.SetupLog(messageTypeMask, log);
		}
		//Debug Methods
		public void DebugLogConsole(MessageType type)
		{
			m_log.SendMessage("This is a message!", type);
		}
    }
}
