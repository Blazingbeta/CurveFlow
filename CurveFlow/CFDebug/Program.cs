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
			controller.AppendTrackedValue("Dodge", 0.5f);
			CreateNewQuery();
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
			Dictionary<string, float> sampleMap1 = new Dictionary<string, float>
			{
				{ "Parry", 0.8f },
				{ "Dodge", 0.4f }
			};
			Dictionary<string, float> sampleMap2 = new Dictionary<string, float>
			{
				{ "Parry", 0.5f },
				{ "Dodge", 0.8f }
			};
			query.InsertOutput(sampleMap1, "ParryChallenge");
			query.InsertOutput(sampleMap2, "DodgeChallenge");
			Console.WriteLine("Selection returned: " + query.CalculateOptimalSelection(0.0f));
		}
		static void PrintToLog(string message, CurveFlow.MessageType type)
		{
			Console.WriteLine(type + ": " + message);
		}
	}
}
