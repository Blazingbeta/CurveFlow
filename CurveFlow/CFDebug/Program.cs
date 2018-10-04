using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CFDebug
{
	class Program
	{
		static CurveFlow.CurveFlowController controller;
		static void Main(string[] args)
		{
			controller = new CurveFlow.CurveFlowController();
			controller.InitializeLog(PrintToLog, (CurveFlow.MessageType)7);

			CreateProfile();
			controller.AppendTrackedValue("Parry", 0.33f);
			controller.AppendTrackedValue("Parry", 0.68f);
			controller.AppendTrackedValue("Parry", 0.574f);
			SaveProfile();
		}
		static void CreateProfile()
		{
			controller.CreateNewProfile(new CurveFlow.TrackedValue[] {
				new CurveFlow.TrackedValue(0f, 1f, "Parry", CurveFlow.ValueType.AVERAGE),
				new CurveFlow.TrackedValue(0f, 1f, "Dodge", CurveFlow.ValueType.AVERAGE),
				new CurveFlow.TrackedValue(0f, 1f, "Health", CurveFlow.ValueType.SET)
			});
		}
		static void SaveProfile()
		{
			FileStream file = new FileStream("TestProfile.cfp", FileMode.Create, FileAccess.Write, FileShare.None);
			controller.SaveProfile(file);
			file.Close();
		}
		static void LoadProfile()
		{
			FileStream stream = new FileStream("TestProfile.cfp", FileMode.Open, FileAccess.Read, FileShare.None);
			controller.LoadProfile(stream);
			stream.Close();
			Console.WriteLine("Loading Profile. Parry: " + controller.GetCurrentValue("Parry"));
		}
		static void CreateNewQuery()
		{
			//TODO test these methods with the inefficient manual creation methods
			CurveFlow.OutputQuery query = new CurveFlow.OutputQuery(controller);
		}
		static void PrintToLog(string message, CurveFlow.MessageType type)
		{
			Console.WriteLine(type + ": " + message);
		}
	}
}
