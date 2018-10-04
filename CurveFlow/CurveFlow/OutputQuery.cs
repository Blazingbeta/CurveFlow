using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CurveFlow
{
	/// <summary>
	/// Contains all of the information for the CurveFlowController to select from during a query
	/// <para>Can be used to both create new output files for storing, or to load previously created ones</para>
	/// </summary>
	public class OutputQuery
	{
		private class Output
		{
			readonly public Dictionary<string, float> queryValues;
			readonly public string returnString;
			public Output(string outputString)
			{
				queryValues = new Dictionary<string, float>();
				returnString = outputString;
			}
			public float CalculateDifficulty(TrackedValue[] currentValues)
			{
				//TODO Difficulty Calculation
				return 0.0f;
			}
		}
		List<Output> m_outputList;
		private CurveFlowController m_controller;
		public OutputQuery(CurveFlowController controller)
		{
			m_outputList = new List<Output>();
			m_controller = controller;
		}
		public void InsertOutput(Dictionary<string, float> estimatedValues, string returnString)
		{
			Output newOutput = new Output(returnString);
			foreach(string key in estimatedValues.Keys)
			{
				newOutput.queryValues.Add(key, estimatedValues[key]);
			}
			m_outputList.Add(newOutput);
		}
		public OutputQuery(string queryFile)
		{
			//Parses the file back into an object
		}
		public string CalculateOptimalSelection(float intendedDifficulty)
		{
			//Does not use the Micro/Macro curves yet, just based on the difficulty number
			float currentBestDelta = float.MaxValue;
			int currentBestIndex = -1;
			TrackedValue[] currentValues = m_controller.m_profile.GetAllValues();
			for(int j = 0; j < m_outputList.Count; j++)
			{
				float difficulty = m_outputList[j].CalculateDifficulty(currentValues);
				float delta = Math.Abs(intendedDifficulty - difficulty);
				if(delta < currentBestDelta)
				{
					currentBestDelta = delta;
					currentBestIndex = j;
				}
			}

			return m_outputList[j].returnString;
		}
		public string GetSavableString()
		{
			return "";
		}
		public override string ToString()
		{
			return GetSavableString();
		}
	}
}
