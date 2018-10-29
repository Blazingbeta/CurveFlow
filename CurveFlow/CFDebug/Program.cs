﻿using System;
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
			//File.WriteAllText("..\\..\\..\\..\\QueryFiles\\DefaultQuery.xml", CurveFlow.OutputQuery.GetDefaultXML());
			controller = new CurveFlow.CurveFlowController(CurveFlow.CurveFlowController.GenerateSettings());
			controller.InitializeLog(PrintToLog, (CurveFlow.MessageType)7);

			LoadProfile();
			LoadXmlQuery();
		}
		static void LoadAndPrintXML()
		{
			string xml = File.ReadAllText("..\\..\\..\\..\\QueryFiles\\LoadAndSave.qf");
			CurveFlow.OutputQuery xmlLoadTest = new CurveFlow.OutputQuery(xml);
			Console.WriteLine(xmlLoadTest.GetXmlString());
		}
		static void TestGroupSelection()
		{
			string xml = File.ReadAllText("..\\..\\..\\..\\QueryFiles\\Default.qf");
			CurveFlow.OutputQuery xmlLoadTest = new CurveFlow.OutputQuery(xml);
			controller.EvaluateGroupSelection(xmlLoadTest, 0.0f, 2);
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
		static void LoadXmlQuery()
		{
			string xml = File.ReadAllText("..\\..\\..\\..\\QueryFiles\\DefaultQuery.xml");
			CurveFlow.OutputQuery xmlLoadTest = new CurveFlow.OutputQuery(xml);
			controller.Evaluate(xmlLoadTest, 0.0f);
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
			controller.LoadProfile(File.ReadAllText("..\\..\\..\\..\\QueryFiles\\controller.xml"));
			Console.WriteLine("Loading Profile. Parry: " + controller.GetCurrentValue("Parry"));
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
