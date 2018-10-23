using CurveFlow;
using System;

class CurveFlowInterface
{
	CurveFlowController controller;
	void Start()
	{
		string controllerSettings = System.IO.File.ReadAllText("ControllerSettings.xml");
		controller = new CurveFlowController(controllerSettings);

	}
	void HookLog()
	{
		//Message type of 7 allows all messages
		controller.InitializeLog(PrintToLog, (MessageType)7);
	}
	void PrintToLog(string message, MessageType type)
	{
		Console.WriteLine(type + message);
	}
	void GetSettings()
	{
		System.IO.File.WriteAllText("ControllerSettings.xml", CurveFlowController.GenerateSettings());
	}
	void Evaluate()
	{
		string queryxml = "load xml here";
		OutputQuery query = new OutputQuery(queryxml);
		float desiredChallenge = 0.0f;
		string output = controller.Evaluate(query, desiredChallenge);
	}
	void GetQueryFile()
	{
		System.IO.File.WriteAllText("QuerySettings.xml", OutputQuery.GetDefaultXML());
	}
	void CreateProfile()
	{
		controller.CreateNewProfile(new CurveFlow.TrackedValue[] {
				new TrackedValue(0f, 1f, "Parry", CurveFlow.ValueType.AVERAGE),
				new TrackedValue(0f, 1f, "Dodge", CurveFlow.ValueType.AVERAGE),
				new TrackedValue(0f, 1f, "Health", CurveFlow.ValueType.SET),
				new TrackedValue(0f, float.MaxValue, "Money", CurveFlow.ValueType.ADDITIVE)
			});
	}
}
