using UnityEngine;
using CurveFlow;
using System.IO;

/*
 * 
 * When should the profile be updated?
 * CurrentHealth: On TakeDamage
 * GrabSkill up: When catching projectiles, extra for multi-catch
 *	Down: Grabbing too early, getting hit while grab is avalible
 * DodgeSkill up: Evading melee attacks, dodging through blasts
 *	Down: Getting hit by melee or blast attacks while dodge is avalible.
 * 
 */

public static class CurveFlowManager
{
	static string folderPath = "..\\..\\Profiles";
	static CurveFlowController m_controller = null;
	static OutputQuery m_query = null;
	public static void Initialize(string QueryName)
	{
		m_controller = new CurveFlowController(CurveFlowController.GenerateSettings());
		m_controller.InitializeLog(PrintToLog, (MessageType)7);
		m_controller.LoadProfile(File.ReadAllText(folderPath + "\\DefaultProfile.pfl"));
		m_query = new OutputQuery(Resources.Load<TextAsset>("QueryFiles/" + QueryName).text);
	}
	public static void AppendValue(string name, float amount)
	{
		m_controller.AppendTrackedValue(name, amount);
	}
	public static void SetValue(string name, float amount)
	{
		m_controller.SetTrackedValue(name, amount);
	}
	public static string Query(float value)
	{
		return m_controller.Evaluate(m_query, value);
	}
	private static void PrintToLog(string message, MessageType type)
	{
		Debug.Log(message);
	}
	private static void CreateNewProfile()
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
