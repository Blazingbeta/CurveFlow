using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveFlow
{
	class CFLog
	{
		MessageType m_allowedTypes;
		LogCallback m_log;
		internal void SetupLog(MessageType allowedTypeMask, LogCallback log)
		{
			m_allowedTypes = allowedTypeMask;
			m_log = log;
		}
		internal void SendMessage(string message, MessageType type)
		{
			if((type & m_allowedTypes) != 0)
			{
				m_log(message, type);
			}
		}
	}
}
