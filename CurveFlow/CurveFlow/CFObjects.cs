using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveFlow
{
	public enum MessageType { STATUS = 1, WARNING = 2, ERROR = 4}
	public delegate void LogCallback(string logMessage, MessageType type);
}
