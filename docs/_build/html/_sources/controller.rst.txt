.. _class_controller:

Curve Flow Controller
=====================

The main controller that drives CurveFlow. All of the processing is handled through this object.

+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| ctor       | :ref:`CurveflowController<class_controller_constructor>` **(** string_ settingsXML **)**                                                                                               |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| void       | :ref:`InitializeLog<class_controller_initialize_log>` **(** :ref:`LogCallback<class_objects_logcallback>` log, :ref:`MessageType<class_objects_messagetype>` typeMask **)**            |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| void       | :ref:`CreateNewProfile<class_controller_create_profile>` **(** :ref:`TrackedValue<class_objects_trackedvalue>` [] trackedValues **)**                                                  |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| void       | :ref:`LoadProfile<class_controller_load_profile>` **(** string_ :ref:`profileXML<class_controller_profile>` **)**                                                                      |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| string_    | :ref:`SaveProfile<class_controller_save_profile>` **(** **)**                                                                                                                          |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| float_     | :ref:`GetCurrentValue<class_controller_get_current_value>` **(** string_ valueName **)**                                                                                               |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| void       | :ref:`AppendTrackedValue<class_controller_append_tracked_value>` **(** string_ valueName, float_ nextValue **)**                                                                       |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| void       | :ref:`SetTrackedValue<class_controller_set_tracked_value>` **(** string_ valueName, float_ newValue **)**                                                                              |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| string     | :ref:`Evaluate<class_controller_evaluate>` **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge **)**                                                           |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| string     | :ref:`EvaluateOnCurve<class_controller_evaluate_on_curve>` **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, float_ time **)**                              |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| string_ [] | :ref:`EvaluateGroupSelection<class_controller_eval_group>` **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, int_ count **)**                               |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| string_ [] | :ref:`EvaluateGroupSelectionOnCurve<class_controller_eval_group_on_curve>` **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, int_ count, float_ time **)**  |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+

.. _int: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/int

.. _string: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/

.. _float: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/float



.. _class_controller_constructor:

- **CurveFlowController** **(** string_ xmlString **)**

Creates a Controller using the supplied settings XML string. See TODO XML SETTINGS

.. _class_controller_initialize_log:

- void **InitializeLog** **(** :ref:`LogCallback<class_objects_logcallback>` log, :ref:`MessageType<class_objects_messagetype>` typeMask **)**

Hooks CurveFlow's internal logging system up to a supplied method, usually you will want this to be a custom function that forwards it to your engine's logging.

The :ref:`Type Mask<class_objects_messagetype>` is a bitmask of what types of messages you'd like to allow through. If unsure, input "(MessageType)7" which allows all.

.. _class_controller_create_profile:

- void **CreateProfile** **(** :ref:`TrackedValue<class_objects_trackedvalue>` [] trackedValues **)**

Creates a new profile with the supplied :ref:`TrackedValues<class_objects_trackedvalue>` , which define the names, min/maxes, and append type of each tracked value.

.. _class_controller_load_profile:

- void **LoadProfile** **(** _string :ref:'profileXML<class_controller_profile>` **)**

Loads a profile along with any current values contained and any locked outputs from the specified :ref:`Profile XML<class_controller_profile>`.

.. _class_controller_save_profile:

- string_ **SaveProfile** **(** **)**

Returns the profile and any current values or locked outputs contained formatted as a :ref:`Profile XML<class_controller_profile>`.

.. _class_controller_get_current_value:

- float_ **GetCurrentValue** **(** string_ valueName **)**

Returns the current value of the skill named valueName.

.. _class_controller_append_tracked_value:

- void **AppendTrackedValue** **(** string_ valueName, float_ nextValue **)**

Appends a number into the skill named valueName. How the number is applied is based on the selected :ref:`ValueType<class_objects_value_type>`

.. _class_controller_set_tracked_value:

- void **SetTrackedValue** **(** string_ valueName, float_ newValue **)**

Foribly set the skill to a new value.

.. _class_controller_evaluate:

- string_ **Evaluate** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge **)**

Evaluates the inserted :ref:`Output Query<class_output_query>` on the current :ref:`Profile<class_controller_create_profile>` and returns the string name of the estimated best output.

.. _class_controller_evaluate_on_curve:

- string_ **EvaluateOnCurve** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, float_ time **)**

Evaluates the inserted :ref:`Output Query<class_output_query>` on the current :ref:`Profile<class_controller_create_profile>` and returns the string name of the estimated best output.

The Desired Challenge will be modified by the :ref:`Micro Curve<class_micro_curve>` before being used to calculate an output.

.. _class_controller_eval_group:

- string_[] **EvaluateGroupSelection** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, int_ count **)**

Evaluates the inserted :ref:`Output Query<class_output_query>` on the current :ref:`Profile<class_controller_create_profile>` and returns an array of size *count* which contains the estimated best group output.

.. _class_controller_eval_group_on_curve:

- string_[] **EvaluateGroupSelection** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, int_ count, float_ time **)**

Evaluates the inserted :ref:`Output Query<class_output_query>` on the current :ref:`Profile<class_controller_create_profile>` and returns an array of size *count* which contains the estimated best group output.

The Desired Challenge will be modified by the :ref:`Micro Curve<class_micro_curve>` before being used to calculate an output.

.. _class_controller_settings:

Master Settings
---------------

The main settings file here contains all of the information that isn't tied to a specific query.
It currently just contains the :ref:`Micro Curve Settings<class_micro_curve_settings>` but will eventually have the profile as well. ::

	<?xml version="1.0" encoding="utf-16"?>
	<Settings>
		<MicroCurve>
			<Algorithm>[x] * Sin([t])</Algorithm>
			<PrecompileExpression>true</PrecompileExpression>
		</MicroCurve>
	</Settings>

.. _class_controller_profile:
	
Profile Settings
----------------

The Profile XML defines the values being tracked, their current values, and any outputs locked through the :ref:`selection lock<class_output_query_selectionlock>`. ::

	<?xml version="1.0" encoding="utf-16"?>
	<Controller>
		<Profile>
			<TrackedValue
				Name="Parry"
				Minimum="0"
				Maximum="1"
				Value="0.5"
				Type="AVERAGE"
				AdditionCount="1" />
			<TrackedValue
				Name="Dodge"
				Minimum="0"
				Maximum="1"
				Value="0.5"
				Type="AVERAGE"
				AdditionCount="1" />
		</Profile>
		<LockedValues>
			<Query
				Name="DefaultQuery">
				<Lock>OptimalChallenge</Lock>
			</Query>
		</LockedValues>
	</Controller>

Profile
^^^^^^^

The profile section contains the list of :ref:`Tracked Values<class_objects_trackedvalue>`.
Each section is explained on the :ref:`Tracked Values<class_objects_trackedvalue>` page with the exception of the AdditionCount variable, 
which is simply the amount of numbers that have been added into an AVERAGE type value in order to track the weight. ::

	<Profile>
		<TrackedValue
			Name="Parry"
			Minimum="0"
			Maximum="1"
			Value="0.5"
			Type="AVERAGE"
			AdditionCount="1" />
		<TrackedValue
			Name="Dodge"
			Minimum="0"
			Maximum="1"
			Value="0.5"
			Type="AVERAGE"
			AdditionCount="1" />
	</Profile>

.. _class_profile_locks:
	
LockedValues
^^^^^^^^^^^^

This section tracks the :ref:`locked selections<class_output_query_selectionlock>`.
Each :ref:`query<class_output_query>` object holds each of the locks for the query with the associated name.
Each lock contains the name of the output inside that query which has been locked for this profile. ::

	<LockedValues>
		<Query
			Name="DefaultQuery">
			<Lock>OptimalChallenge</Lock>
		</Query>
	</LockedValues>