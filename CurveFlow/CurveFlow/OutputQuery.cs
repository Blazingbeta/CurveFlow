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
		List<Output> m_outputList;

		//Repeat Selection
		int m_nextID = 0;
		bool m_discourageRepeatSelection = false;
		float m_repeatSelectionWeight = 3.5f;
		int m_previousTrackedValues = 1;
		bool m_diminishingWeight = false;
		List<int> m_previousValues = new List<int>();

		//Group binding
		bool m_isGroupBinding = false;
		StackingType m_stackingType = StackingType.ADDITIVE;
		bool m_allowGroupDuplicates = false;

		/// <summary>
		/// Default constructor for use without an existing XML string.
		/// <para>This should only be used for creating your XML files as not everything can be setup here</para>
		/// </summary>
		/// <param name="isGroupBinding"></param>
		public OutputQuery(bool isGroupBinding)
		{
			m_outputList = new List<Output>();
			m_isGroupBinding = isGroupBinding;
		}
		public OutputQuery(string xmlString)
		{
			m_outputList = new List<Output>();
			//Parses the file back into an object
			//TODO try catch
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlString);
			//Do things on the settings node here
			var repeatNodes = doc.SelectSingleNode("/Query/Settings/RepeatSelection");
			m_discourageRepeatSelection = repeatNodes["DiscourageRepeatSelection"].InnerText == "True";
			if (m_discourageRepeatSelection)
			{
				m_repeatSelectionWeight = float.Parse(repeatNodes["RepeatSelectionWeight"].InnerText);
				m_previousTrackedValues = int.Parse(repeatNodes["PreviousValuesTracked"].InnerText);
				m_diminishingWeight = bool.Parse(repeatNodes["DiminishingWeight"].InnerText);
			}

			var groupNodes = doc.SelectSingleNode("/Query/Settings/GroupBinding");
			m_isGroupBinding = groupNodes["IsGroupSelection"].InnerText == "True";
			if (m_isGroupBinding)
			{
				string stackingType = groupNodes["StackingType"].InnerText;
				m_stackingType = (stackingType == "ADDITIVE") ? StackingType.ADDITIVE : StackingType.MULTIPLICATIVE;
				m_allowGroupDuplicates = bool.Parse(groupNodes["AllowDuplicates"].InnerText);
			}

			//Load the outputs
			var outputNodes = doc.SelectNodes("/Query/Output");
			foreach (XmlNode node in outputNodes)
			{
				Output outputObject = new Output(m_nextID, node["Name"].InnerText);
				m_nextID++;
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
			Output newOutput = new Output(m_nextID, returnString);
			m_nextID++;
			foreach (string key in estimatedValues.Keys)
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
			Output newOutput = new Output(m_nextID, returnString);
			m_nextID++;
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
			if (m_isGroupBinding)
			{
				CFLog.SendMessage("Trying to calculate single selection on group binding. Continuing...", MessageType.ERROR);
			}
			StringBuilder sb = new StringBuilder();
			sb.Append("Beginning Query:\n");
			float currentBestDelta = float.MaxValue;
			int currentBestIndex = -1;
			for(int j = 0; j < m_outputList.Count; j++)
			{
				float difficulty = m_outputList[j].CalculateDifficulty(currentValues, sb);
				if (m_discourageRepeatSelection)
				{
					int position = m_previousValues.IndexOf(m_outputList[j].id);
					//If the ID is in the previous list
					if(position != -1)
					{
						if (m_diminishingWeight)
						{
							difficulty *= m_repeatSelectionWeight * 
								//Ie if this object is 3rd in a 3 object list, 3/3 weight applied
								(position+1) / m_previousTrackedValues;
						}
						else
						{
							difficulty *= m_repeatSelectionWeight;
						}
						sb.Append("\tIn previous Selection at Index ");
						sb.Append(position);
						sb.Append(". Modded to ");
						sb.Append(difficulty);
						sb.Append("\n");
					}
				}
				float delta = Math.Abs(intendedDifficulty - difficulty);
				if(delta < currentBestDelta)
				{
					currentBestDelta = delta;
					currentBestIndex = j;
				}
			}
			if (m_discourageRepeatSelection)
			{
				//Removes the value if it has already been placed into the queue
				m_previousValues.Remove(m_outputList[currentBestIndex].id);
				m_previousValues.Add(m_outputList[currentBestIndex].id);
				//If the queue has exceded its count
				if(m_previousValues.Count > m_previousTrackedValues)
				{
					m_previousValues.RemoveAt(0);
				}
			}
			sb.Append("Query Returning ");
			sb.Append(m_outputList[currentBestIndex].returnString);
			sb.Append(" with difficulty delta ");
			sb.Append(currentBestDelta);
			CFLog.SendMessage(sb.ToString(), MessageType.STATUS);
			return m_outputList[currentBestIndex].returnString;
		}
		internal string[] GetGroupBinding(float intendedDifficulty, TrackedValue[] currentValues, int totalSelections)
		{
			if (!m_isGroupBinding)
			{
				CFLog.SendMessage("Trying to get group selection on individual selection. Returning null.", MessageType.ERROR);
				return null;
			}
			StringBuilder sb = new StringBuilder();
			sb.Append("Beginning Group Query:\n Individual Values:\n");
			//Precalculate all of the output's results
			m_outputList.ForEach(o => o.CalculateDifficulty(currentValues, sb));
			//Try combinations until we find one of size n with the closest value to intendedDifficulty
			m_recursiveBestValue = float.MaxValue;
			m_recursiveBestArray = new int[totalSelections];
			GetBestGroupBinding(0, 0, totalSelections, new int[totalSelections], intendedDifficulty);
			string[] output = new string[totalSelections];
			sb.Append("Group Query Returning:\n");
			for(int j = 0; j < totalSelections; j++)
			{
				output[j] = m_outputList[m_recursiveBestArray[j]].returnString;
				sb.Append('\t');
				sb.Append(m_recursiveBestArray[j]);
				sb.Append(':');
				sb.Append(output[j]);
				sb.Append('\n');
			}
			sb.Append("With total difficulty delta of ");
			sb.Append(m_recursiveBestValue);
			CFLog.SendMessage(sb.ToString(), MessageType.STATUS);
			return output;
		}
		//Bad? Recursive Way
		int[] m_recursiveBestArray;
		float m_recursiveBestValue;
		private void GetBestGroupBinding(int listIndex, int arrIndex, int remainingSelections, int[] current, float diff)
		{
			remainingSelections--;
			if (remainingSelections == 0)
			{
				//Has reached the bottom (this index still needs to be added however)
				current[arrIndex] = listIndex;
				//Sum the values
				float total = 0.0f;
				for(int j = 0; j < current.Length; j++)
				{
					//TODO multiplicitve/additive modifier here
					total += m_outputList[current[j]].lastDifficulty;
				}
				float delta = Math.Abs(diff - total);
				if(delta < m_recursiveBestValue)
				{
					m_recursiveBestValue = delta;
					current.CopyTo(m_recursiveBestArray, 0);
				}
			}
			else
			{
				if (m_allowGroupDuplicates)
				{
					for (int j = listIndex; j < m_outputList.Count; j++)
					{
						//Recurses for every possible case this number can be
						//Since in a list of 5, 444 is valid output, top number *can* be 3 or 4
						current[arrIndex] = j;
						GetBestGroupBinding(j, arrIndex + 1, remainingSelections, current, diff);
					}
				}
				else
				{
					for (int j = listIndex; j < m_outputList.Count - remainingSelections; j++)
					{
						//Recurses for every possible case this number can be
						//IE with an output count of 5 and a selection of 3, top number can never be 3 or 4
						current[arrIndex] = j;
						GetBestGroupBinding(j + 1, arrIndex + 1, remainingSelections, current, diff);
					}
				}
			}
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
				//Settings
				writer.WriteStartElement("Settings");
				writer.WriteStartElement("RepeatSelection");
					writer.WriteElementString("DiscourageRepeatSelection", m_discourageRepeatSelection.ToString());
					writer.WriteElementString("RepeatSelectionWeight", m_repeatSelectionWeight.ToString("G"));
					writer.WriteElementString("PreviousValuesTracked", m_previousTrackedValues.ToString());
					writer.WriteElementString("DiminishingWeight", m_diminishingWeight.ToString());
				writer.WriteEndElement();
				writer.WriteStartElement("GroupBinding");
					writer.WriteElementString("IsGroupSelection", m_isGroupBinding.ToString());
					writer.WriteElementString("StackingType", m_stackingType.ToString());
					writer.WriteElementString("AllowDuplicates", m_allowGroupDuplicates.ToString());
				writer.WriteEndElement();
				//Outputs
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
		private struct Weight
		{
			public float value;
			public float multiplier;
		}
		private class Output
		{
			readonly public int id;
			readonly public Dictionary<string, Weight> queryValues;
			readonly public string returnString;
			public float lastDifficulty;
			public Output(int id, string outputString)
			{
				this.id = id;
				queryValues = new Dictionary<string, Weight>();
				returnString = outputString;
			}
			public float CalculateDifficulty(TrackedValue[] currentValues, StringBuilder sb)
			{
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
					float weightedDifficulty = (difficulty * Math.Abs(difficulty)) * weight.multiplier;
					totalDifficulty += weightedDifficulty;

					sb.Append('\t');
					sb.Append(val.m_name);
					sb.Append(':');
					sb.Append((difficulty).ToString("G"));
					sb.Append('/');
					sb.Append((weightedDifficulty).ToString("G"));
					sb.Append('\n');
				}
				float averageDifficulty = totalDifficulty / sharedValues;
				lastDifficulty = averageDifficulty;

				sb.Append("\tTotal: ");
				sb.Append(totalDifficulty.ToString("G"));
				sb.Append(" Average: ");
				sb.Append(averageDifficulty.ToString("G"));
				sb.Append('\n');
				return averageDifficulty;
			}
		}
	}
}
