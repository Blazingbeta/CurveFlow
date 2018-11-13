using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CFDebug
{
	class Program
	{
		static CurveFlow.CurveFlowController controller;
		static void Main(string[] args)
		{
			controller = new CurveFlow.CurveFlowController(CurveFlow.CurveFlowController.GenerateSettings());
			controller.InitializeLog(PrintToLog, (CurveFlow.MessageType)0);
			/*
			 * 
			 * Current Issue: No matter the difficulty, curveflow outputs the same result (and it somehow has the best delta)
			 * What I've discoverd: When difficulty deltas start getting all very small, it causes the most extreme case to be chosen every time
			 * 
			 */
			LoadProfile();
			Random rng = new Random();
			CurveFlow.OutputQuery query = LoadXmlQuery();
			for (int j = 0; j < 10; j++)
			{
				float num = (float)rng.NextDouble();
				//controller.Evaluate(query, num);
				Console.WriteLine(num.ToString() + ": " + controller.Evaluate(query, num));
			}
		}
		static void LoadAndPrintXML()
		{
			string xml = File.ReadAllText("..\\..\\..\\..\\QueryFiles\\LoadAndSave.qf");
			CurveFlow.OutputQuery xmlLoadTest = new CurveFlow.OutputQuery(xml);
			Console.WriteLine(xmlLoadTest.GetXmlString());
		}
		static void TestGroupSelection()
		{
		//	CurveFlow.OutputQuery xmlLoadTest = new CurveFlow.OutputQuery(xml);
		}
		static void TestRepeatSelection()
		{
			string xml = File.ReadAllText("GroupQuery.qf");
			CurveFlow.OutputQuery xmlLoadTest = new CurveFlow.OutputQuery(xml);
			controller.Evaluate(xmlLoadTest, 0.0f);
			controller.Evaluate(xmlLoadTest, 0.0f);
			controller.Evaluate(xmlLoadTest, 0.0f);
			controller.Evaluate(xmlLoadTest, 0.0f);
		}
		static CurveFlow.OutputQuery LoadXmlQuery()
		{
			string xml = File.ReadAllText("..\\..\\..\\..\\FlowQuest\\FlowQuest\\Assets\\Resources\\QueryFiles\\DefaultDungeonTiles.xml");
			CurveFlow.OutputQuery xmlLoadTest = new CurveFlow.OutputQuery(xml);
			return xmlLoadTest;
		}
		static void CreateProfile()
		{
			controller.CreateNewProfile(new CurveFlow.TrackedValue[] {
				new CurveFlow.TrackedValue(0f, 1f, "Parry", CurveFlow.ValueType.AVERAGE),
				new CurveFlow.TrackedValue(0f, 1f, "Dodge", CurveFlow.ValueType.AVERAGE),
				new CurveFlow.TrackedValue(0f, 1f, "Health", CurveFlow.ValueType.SET)
			});
			controller.AppendTrackedValue("Parry", 0.5f);
			controller.AppendTrackedValue("Dodge", 0.5f);
		}
		static void SaveProfile()
		{
			File.WriteAllText("..\\..\\..\\..\\QueryFiles\\controller.xml", controller.SaveProfile());
		}
		static void LoadProfile()
		{
			controller.LoadProfile(File.ReadAllText("..\\..\\..\\..\\Profiles\\DefaultProfile.pfl"));
		}
		static void WriteQueryToFile()
		{
			CurveFlow.OutputQuery query = new CurveFlow.OutputQuery(CurveFlow.OutputQuery.GetDefaultXML());
			Dictionary<string, float> sampleMap1 = new Dictionary<string, float>
			{
				{ "Parry", 0.715f },
				{ "Dodge", 0.075f }
			};
			Dictionary<string, float> sampleMap2 = new Dictionary<string, float>
			{
				{ "Parry", 0.152f },
				{ "Dodge", 0.9124f }
			};
			Dictionary<string, float> weightMap = new Dictionary<string, float>
			{
				{"Parry", 2.3f },
				{"Dodge", 1.2f }
			};
			//query.InsertOutput(sampleMap1, weightMap, "ParryChallenge");
			//query.InsertOutput(sampleMap2, weightMap, "DodgeChallenge");
			string output = query.GetXmlString();
			Console.WriteLine(output);
			System.IO.File.WriteAllText("TestQueryFile.qf", output);
		}
		static void PrintToLog(string message, CurveFlow.MessageType type)
		{
			Console.WriteLine(type + ": " + message);
		}
	}
}
