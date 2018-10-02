using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFDebug
{
	class Program
	{
		static void Main(string[] args)
		{
			CurveFlow.CurveFlowController controller = new CurveFlow.CurveFlowController();
			controller.InitializeLog(PrintToLog, (CurveFlow.MessageType)7);

			controller.DebugLogConsole(CurveFlow.MessageType.WARNING);
		}
		static void PrintToLog(string message, CurveFlow.MessageType type)
		{
			Console.WriteLine(type + " " + message);
		}
	}
}
