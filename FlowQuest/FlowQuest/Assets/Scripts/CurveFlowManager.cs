using UnityEngine;
using CurveFlow;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

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
	static string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/my games/FlowQuest";
	static CurveFlowController m_controller = null;
	static OutputQuery m_query = null;
	static Dictionary<string, ValueDisplayManager> m_guiBars;
	public static void Initialize(string QueryName)
	{
		m_controller = new CurveFlowController();
		m_controller.InitializeLog(PrintToLog, (MessageType)7);
		m_controller.InitializeCurve(Expression);
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		if (!File.Exists(folderPath + "/" + WorldController.ProfileName + ".pfl"))
		{
			//Create and load a new profile if one does not already exist
			CreateAndLoadNewProfile();
		}
		else
		{
			//If a previous profile exists, load it
			m_controller.LoadProfile(File.ReadAllText(folderPath + "/" + WorldController.ProfileName + ".pfl"));
		}
		m_query = new OutputQuery(Resources.Load<TextAsset>("QueryFiles/" + QueryName).text);
	}
	public static void LoadQuery(string QueryName)
	{
		Debug.Log(QueryName);
		m_query = new OutputQuery(Resources.Load<TextAsset>("QueryFiles/" + QueryName).text);
	}
	static float Expression(float x, float t)
	{
		return x * (1 + Mathf.Sin(t));
	}
	public static void AppendValue(string name, float amount)
	{
		m_controller.AppendTrackedValue(name, amount);
		if(m_guiBars != null)
		{
			m_guiBars[name].SetNewFillAmount(m_controller.GetCurrentValue(name));
		}
	}
	public static void SetValue(string name, float amount)
	{
		m_controller.SetTrackedValue(name, amount);
	}
	public static string Query(float value)
	{
		return m_controller.Evaluate(m_query, value);
	}
	public static string QueryOnCurve(float value, float t)
	{
		return m_controller.EvaluateOnCurve(m_query, value, t);
	}
	public static string[] GroupQuery(float value, int count)
	{
		return m_controller.EvaluateGroupSelection(m_query, value, count);
	}
	public static string LastMessage { get; set; }
	private static void PrintToLog(string message, MessageType type)
	{
		Debug.Log(message);
		if(type == MessageType.STATUS)
		{
			LastMessage = message;
		}
	}
	private static void CreateAndLoadNewProfile()
	{
		m_controller.CreateNewProfile(new TrackedValue[]
		{
			new TrackedValue(0f, 1f, "GrabSkill", ValueType.AVERAGEWEIGHTED, 30),
			new TrackedValue(0f, 1f, "DodgeSkill", ValueType.AVERAGEWEIGHTED, 15),
			new TrackedValue(0f, 1f, "CurrentHealth", ValueType.SET)
		});
		File.WriteAllText(folderPath + "\\" + WorldController.ProfileName + ".pfl", m_controller.SaveProfile());
	}
	public static void SaveProfile()
	{
		string xml = m_controller.SaveProfile();
		File.WriteAllText(folderPath + "\\" + WorldController.ProfileName + ".pfl", xml);
	}
	public static void SetGUIValues(Transform parent)
	{
		m_guiBars = new Dictionary<string, ValueDisplayManager>();
		for(int j = 0; j < parent.childCount; j++)
		{
			string name = parent.GetChild(j).name;
			m_guiBars.Add(name, parent.GetChild(j).GetComponent<ValueDisplayManager>());
			m_guiBars[name].SetNewFillAmount(m_controller.GetCurrentValue(name));
		}
	}
}
