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

public class CurveFlowManager : MonoBehaviour
{
	string folderPath = "..\\..\\Profiles";
	CurveFlowController m_controller = null;
	private void Awake()
	{
		m_controller = new CurveFlowController(CurveFlowController.GenerateSettings());
		m_controller.InitializeLog(PrintToLog, (MessageType)7);
		m_controller.LoadProfile(File.ReadAllText(folderPath + "\\DefaultProfile.pfl"));
		OutputQuery query = new OutputQuery(Resources.Load<TextAsset>("QueryFiles/DefaultQuery").text);
		Debug.Log(m_controller.Evaluate(query, 0.5f));
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			Debug.Log(m_controller.GetCurrentValue("CurrentHealth"));
		}
	}
	public void AppendValue(string name, float amount)
	{
		m_controller.AppendTrackedValue(name, amount);
	}
	public void SetValue(string name, float amount)
	{
		m_controller.SetTrackedValue(name, amount);
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
