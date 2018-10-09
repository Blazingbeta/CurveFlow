using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CurveFlow
{
	/// <summary>
	/// Contains all of the information for the CurveFlowController to select from during a query
	/// <para>Can be used to both create new output files for storing, or to load previously created ones</para>
	/// </summary>
	public class OutputQuery
	{
		private struct Weight
		{
			public float value;
			public float multiplier;
		}
		private class Output
		{
			readonly public Dictionary<string, Weight> queryValues;
			readonly public string returnString;
			public Output(string outputString)
			{
				queryValues = new Dictionary<string, Weight>();
				returnString = outputString;
			}
			public float CalculateDifficulty(TrackedValue[] currentValues)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("Begining Difficulty Calculation on ");
				sb.Append(returnString);
				sb.Append(":\n");
				float sharedValues = 0f;
				float totalDifficulty = 0.0f; //How many shared keys are used in the average
				for (int j = 0; j < currentValues.Length; j++)
				{
					TrackedValue val = currentValues[j];
					if (!queryValues.ContainsKey(val.m_name)) continue;
					Weight weight = queryValues[val.m_name];
					sharedValues += weight.multiplier;
					//Get the associated queryValues key based on the currentValue[j]
					float delta = val.m_currentValue - weight.value;
					//Get this delta as a percentage based on min and max
					float difficulty = delta / (val.m_max - val.m_min);
					float weightedDifficulty = (difficulty * Math.Abs(difficulty))*weight.multiplier;
					totalDifficulty += weightedDifficulty;

					sb.Append(val.m_name);
					sb.Append(':');
					sb.Append((difficulty).ToString("G"));
					sb.Append('/');
					sb.Append((weightedDifficulty).ToString("G"));
					sb.Append(' ');
				}
				float averageDifficulty = totalDifficulty / sharedValues;

				sb.Append("Total: ");
				sb.Append(totalDifficulty.ToString("G"));
				sb.Append(" Average: ");
				sb.Append(averageDifficulty.ToString("G"));

				CFLog.SendMessage(sb.ToString(), MessageType.DEBUG);
				return averageDifficulty;
			}
		}


		List<Output> m_outputList;
		public OutputQuery()
		{
			m_outputList = new List<Output>();
		}
		public OutputQuery(string xmlString)
		{
			m_outputList = new List<Output>();
			//Parses the file back into an object
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlString);
			//Do things on the settings node here
			//Load the outputs
			var outputNodes = doc.SelectNodes("/Query/Output");
			foreach (XmlNode node in outputNodes)
			{
				Output outputObject = new Output(node["Name"].InnerText);
				var skillNodes = node.SelectNodes("Skill");
				foreach (XmlNode skillNode in skillNodes)
				{
					Weight weight = new Weight
					{
						value = float.Parse(skillNode["Value"].InnerText),
						multiplier = float.Parse(skillNode["Weight"].InnerText)
					};
					outputObject.queryValues.Add(skillNode["Name"].InnerText, weight);
				}
				m_outputList.Add(outputObject);
			}
			CFLog.SendMessage("XML Successfully Loaded.", MessageType.STATUS);
		}
		public void InsertOutput(Dictionary<string, float> estimatedValues, string returnString)
		{
			Output newOutput = new Output(returnString);
			foreach(string key in estimatedValues.Keys)
			{
				Weight weight = new Weight()
				{
					value = estimatedValues[key],
					multiplier = 1.0f
				};
				newOutput.queryValues.Add(key, weight);
			}
			m_outputList.Add(newOutput);
		}
		public void InsertOutput(Dictionary<string, float> estimatedValues, Dictionary<string, float> weights, string returnString)
		{
			Output newOutput = new Output(returnString);
			foreach (string key in estimatedValues.Keys)
			{
				Weight weight = new Weight()
				{
					value = estimatedValues[key],
					multiplier = weights[key]
				};
				newOutput.queryValues.Add(key, weight);
			}
			m_outputList.Add(newOutput);
		}
		internal string CalculateOptimalSelection(float intendedDifficulty, TrackedValue[] currentValues)
		{
			//Does not use the Micro/Macro curves yet, just based on the difficulty number
			float currentBestDelta = float.MaxValue;
			int currentBestIndex = -1;
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
			CFLog.SendMessage("Query returning " + m_outputList[currentBestIndex].returnString 
				+ " with difficulty delta " + currentBestDelta, MessageType.STATUS);
			return m_outputList[currentBestIndex].returnString;
		}
		public string GetXmlString()
		{
			StringBuilder sb = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings()
			{
				Indent = true,
				IndentChars = "\t",
				NewLineOnAttributes = true
			};
			using (XmlWriter writer = XmlWriter.Create(sb, settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("Query");
				foreach(Output output in m_outputList)
				{
					writer.WriteStartElement("Output");
					writer.WriteElementString("Name", output.returnString);
					foreach(string key in output.queryValues.Keys)
					{
						writer.WriteStartElement("Skill");
						writer.WriteElementString("Name", key);
						writer.WriteElementString("Value", output.queryValues[key].value.ToString("G"));
						writer.WriteElementString("Weight", output.queryValues[key].multiplier.ToString("G"));
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
			return sb.ToString();
		}
		public override string ToString()
		{
			return GetXmlString();
		}
	}
}
