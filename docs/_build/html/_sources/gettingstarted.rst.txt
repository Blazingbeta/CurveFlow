Getting Started
===============

Welcome to CurveFlow! 

.. note::
   Be careful, errors in your input are not always caught.

Installation
^^^^^^^^^^^^
Installation is entirely engine specific. Here is the process in some major game engines.

- Unity: Simply drag the DLL into the project explorer and it should be usable by default

- Godot: Only compatible with a Mono project. Open the project and add the CurveFlow DLL as a reference in the project settings.

Initial Setup
^^^^^^^^^^^^^
To get started, first you will want to probably create a seperate object for storing Curve Flow related things, as you will need to persist the data yourself.

Make sure you don't create any more than one controller, some things might become unstable if you do. ::

	using CurveFlow;

	class CurveFlowInterface
	{
		CurveFlowController controller;
		void Start()
		{
			controller = new CurveFlowController();
		}
	}

The next step is to hook up the logging system. This will help you debug CurveFlow when things go wrong.
The logging system uses a Bitmask to determine which types of messages to allow through. You can either just pass in an enum, or do a combination of them.
Add up the following numbers you want to allow through the log and cast it as a :ref:`MessageType<class_objects_messagetype>` .

- **STATUS** : 1
- **DEBUG** : 2
- **ERROR** : 4 

Then call :ref:`InitializeLog<class_controller_initialize_log>` with your bitmask and a :ref:`callback function<class_objects_logcallback>` ::

	void HookLog()
	{
		//Message type of 7 allows all messages
		controller.InitializeLog(PrintToLog, (MessageType)7);
	}
	void PrintToLog(string message, MessageType type)
	{
		Console.WriteLine(type + message);
	}

.. _class_profile:
	
Next, you will need to initialize the values that CurveFlow will track. These are the numbers that your Queries will be matched against, and you will need to define all of the trackable values.
See :ref:`Value Type<class_objects_value_type>` for the specifics of each type. ::

	void CreateProfile()
	{
	controller.CreateNewProfile(new CurveFlow.TrackedValue[] {
		new TrackedValue(0f, 1f, "Parry", CurveFlow.ValueType.AVERAGE),
		new TrackedValue(0f, 1f, "Dodge", CurveFlow.ValueType.AVERAGE),
		new TrackedValue(0f, 1f, "Health", CurveFlow.ValueType.SET),
		new TrackedValue(0f, float.MaxValue, "Money", CurveFlow.ValueType.ADDITIVE)
	});
	}
	
If you want to persist a profile through multiple loads, you can save them by getting the save data string with :ref:`Controller.SaveProfile<class_controller_save_profile>`
and passing it into :ref:`Controller.LoadProfile<class_controller_load_profile>` later
	
Your First Query
^^^^^^^^^^^^^^^^

:ref:`Query files<class_output_query>` contain all of the information for how Curve Flow evaluates the best output. They are managed through XML, and you can get the default like so ::

	System.IO.File.WriteAllText("QuerySettings.xml", OutputQuery.GetDefaultXML());

For specific setup information, see :ref:`OutputQuery XML<class_output_query_xml_tutorial>` .

If you are just following along, use the following XML ::

	<?xml version="1.0" encoding="utf-16"?>
	<Query
		Name="TestQuery">
		<Settings>
			<RepeatSelection
				Enabled="False">
				<RepeatSelectionWeight>4.0</RepeatSelectionWeight>
				<PreviousValuesTracked>4</PreviousValuesTracked>
				<DiminishingWeight>True</DiminishingWeight>
			</RepeatSelection>
			<GroupBinding
				Enabled="False">
				<AllowDuplicates>False</AllowDuplicates>
				<GroupRepeatMultiplier>2.0</GroupRepeatMultiplier>
			</GroupBinding>
			<SelectionLock
				Enabled="False" />
		</Settings>
		<Output>
			<Name>Output1</Name>
			<Skill>
				<Name>GrabSkill</Name>
				<Value>0.8</Value>
				<Weight>1.0</Weight>
			</Skill>
			<Skill>
				<Name>DodgeSkill</Name>
				<Value>0.3</Value>
				<Weight>1.0</Weight>
			</Skill>
		</Output>
		<Output>
			<Name>Output2</Name>
			<Skill>
				<Name>DodgeSkill</Name>
				<Value>0.8</Value>
				<Weight>1.0</Weight>
			</Skill>
			<Skill>
				<Name>GrabSkill</Name>
				<Value>0.2</Value>
				<Weight>1.0</Weight>
			</Skill>
		</Output>
	</Query>
	
Finally, you can get the optimal output from your query by passing it into the :ref:`controller<class_controller>` 
along with the desired challenge, which is the change in difficulty from the players current estimated skill level. ::

	void Evaluate()
	{
		string queryxml = "load xml here";
		OutputQuery query = new OutputQuery(queryxml);
		float desiredDifficulty = 0.0f;
		string output = controller.Evaluate(query, desiredDifficulty);
	}

Next Steps
^^^^^^^^^^

If you haven't already, now is the time to hook up the specific values you want to track. 
See :ref:`Controller Settings<class_controller_settings>` and :ref:`Query Settings<class_output_query_xml_tutorial>` for more information on how to customize CurveFlow to do what you need.