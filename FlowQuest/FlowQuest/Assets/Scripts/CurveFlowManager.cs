using UnityEngine;
using CurveFlow;
using System.IO;

public class CurveFlowManager : MonoBehaviour
{
	string folderPath = "C:\\Users\\Beta\\Desktop\\NUWork\\Files\\Capstone\\Profiles";
	CurveFlowController m_controller = null;
	private void Awake()
	{
		m_controller = new CurveFlowController(CurveFlowController.GenerateSettings());
		m_controller.InitializeLog(PrintToLog, (MessageType)7);
		m_controller.LoadProfile(File.ReadAllText(folderPath + "\\DefaultProfile.pfl"));
		OutputQuery query = new OutputQuery(Resources.Load<TextAsset>("QueryFiles/DefaultQuery").text);
		Debug.Log(m_controller.Evaluate(query, 0.5f));
	}
	private void PrintToLog(string message, MessageType type)
	{
		Debug.Log(message);
	}
	private void CreateNewProfile()
	{
		m_controller.CreateNewProfile(new TrackedValue[]
		{
			new TrackedValue(0f, 1f, "GrabSkill", ValueType.AVERAGE),
			new TrackedValue(0f, 1f, "DodgeSkill", ValueType.AVERAGE),
			new TrackedValue(0f, 1f, "CurrentHealth", ValueType.SET)
		});
		File.WriteAllText("DefaultProfile.pfl", m_controller.SaveProfile());
	}
}
