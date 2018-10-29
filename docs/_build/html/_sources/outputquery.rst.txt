.. _class_output_query:

Output Query
===============

Description

+---------+------------------------------------------------------------------------------------+
| ctor    | :ref:`OutputQuery<class_output_query_constructor>` **(** string_ settingsXML **)** |
+---------+------------------------------------------------------------------------------------+
| string_ | :ref:`GetXmlString<class_output_query_getxml>` **(** **)**                         |
+---------+------------------------------------------------------------------------------------+

.. _string: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/

.. _class_output_query_constructor:

- **OutputQuery** **(** string_ settingsXML **)**

Creates a new query, based on the passed in :ref:`XML String<class_output_query_xml_tutorial>`.

.. _class_output_query_getxml:

- **GetXmlString** **(** **)**

Returns the current :ref:`XML String<class_output_query_xml_tutorial>` with any modifications made, such as PREVIOUSSELECTIONLOCK

.. _class_output_query_xml_tutorial:

Output Query Settings
---------------------

The Query is entirely defined by it's settings file. Here is what the full file looks like, and each part is explained in detail further below. ::

	<?xml version="1.0" encoding="utf-16"?>
	<Query
		Name="DefaultQuery">
		<Settings>
			<RepeatSelection
				Enabled="True">
				<RepeatSelectionWeight>3.0</RepeatSelectionWeight>
				<PreviousValuesTracked>2</PreviousValuesTracked>
				<DiminishingWeight>True</DiminishingWeight>
			</RepeatSelection>
			<GroupBinding
				Enabled="False">
				<AllowDuplicates>False</AllowDuplicates>
				<GroupRepeatMultiplier>2.0</GroupRepeatMultiplier>
			</GroupBinding>
			<SelectionLock
				Enabled="False">
				<Lock>OutputName2</Lock>
			</SelectionLock>
		</Settings>
		<Output>
			<Name>OutputName1</Name>
			<Skill>
				<Name>Parry</Name>
				<Value>0.5145</Value>
				<Weight>1.0</Weight>
			</Skill>
			<Skill>
				<Name>Dodge</Name>
				<Value>0.1521</Value>
				<Weight>1.0</Weight>
			</Skill>
		</Output>
		<Output>
			<Name>OutputName2</Name>
			<Skill>
				<Name>Parry</Name>
				<Value>0.5145</Value>
				<Weight>1.0</Weight>
			</Skill>
			<Skill>
				<Name>Dodge</Name>
				<Value>0.1521</Value>
				<Weight>1.0</Weight>
			</Skill>
		</Output>
	</Query>

Outputs
^^^^^^^

Each output's name corrisponds to a string that the query can potentially return. Each output has a list of skill values that are defined as such

Skill

- Name: The case-sensitive name of the skill in the :ref:`Profile<class_profile>`
- Value: The estimated amount of that skill this output is associated with
- Weight: How much weight this skill has to the overall output evaluation. A skill with a weight of 2.5 would be added to the average 2 and a half times (and increase the total count by 2.5 instead of just 1)
	
Repeat Selection
^^^^^^^^^^^^^^^^

If enabled, causes the Query to keep track of previously output values and it will try to prevent them from happening again.

- Previous Selection Weight

If a selection has already been selected, causes it's challenge delta (how far from your desired challenge number it is) to be multiplied by this much. 

For example, if you have two outputs that evaluate to 0.1 and 0.2, and a repeat selection weight of 3, the 0.2 would be chosen instead of the 0.1 if it was in the list of previous selected values.

- Prevous Values Tracked

How many selections will need to be made before one will be no longer tracked.

- Diminishing Weight

if true, causes the older selections in the list to be modified by less than the newer ones.

Given previous selections 1 2 and 3 and a previous selection weight of 9.0, this setting would cause 1's weight to be 3.0, 2's weight to be 6.0, and 3's weight to be 9.0

Group Binding
^^^^^^^^^^^^^

If enabled, this query will be treated as a Group Query. This means that it is setup for each output to be a single part of a whole, such as individual enemies in an encounter.

- Allow Duplicates

If true, allows the query to select more than one of the same output in a single evaluation.

- Group Repeat Multiplier

If allow duplicates is enabled, the challenge delta of a group (how far from your desired challenge rating it is) will be multiplied by this number for every extra duplicate.

For exaxmple, a group containing 1, 1, and 2 would be multiplied by the repeat multiplier once.

.. _class_output_query_selectionlock:

Selection Lock
^^^^^^^^^^^^^^

If true, this query will remember the previous returns and never allow them to be repeated. 
This can be useful for things such as choosing the next level to go to without wanting the same one twice.
These will be saved into the :ref:`profile<class_profile_locks>` ::

	<SelectionLock
		Enabled="False">
	</SelectionLock>