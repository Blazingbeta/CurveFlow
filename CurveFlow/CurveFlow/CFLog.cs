using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveFlow
{
	static class CFLog
	{
		static MessageType m_allowedTypes;
		static LogCallback m_log;
		static internal void SetupLog(MessageType allowedTypeMask, LogCallback log)
		{
			m_allowedTypes = allowedTypeMask;
			m_log = log;
		}
		static internal void SendMessage(string message, MessageType type)
		{
			if((type & m_allowedTypes) != 0)
			{
				m_log(message, type);
			}
		}
	}
}
