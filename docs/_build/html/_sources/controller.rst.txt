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
| void       | :ref:`LoadProfile<class_controller_load_profile>` **(** Stream_ profileStream **)**                                                                                                    |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| void       | :ref:`SaveProfile<class_controller_save_profile>` **(** Stream_ outputStream **)**                                                                                                     |
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

.. _Stream: https://docs.microsoft.com/en-us/dotnet/api/system.io.stream?view=netframework-4.7.2

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

- void **LoadProfile** **(** Stream_ profileStream **)**

Loads a profile along with any current values contained from the specified Stream_ .

.. _class_controller_save_profile:

- void **SaveProfile** **(** Stream_ outputStream **)**

Saves a profile and any current values contained to the specified Stream_ .

.. _class_controller_get_current_value:

- float **GetCurrentValue** **(** string_ valueName **)**

Returns the current value of the skill named valueName.

.. _class_controller_append_tracked_value:

- void **AppendTrackedValue** **(** string_ valueName, float_ nextValue **)**

Appends a number into the skill named valueName. How the number is applied is based on the selected :ref:`ValueType<class_objects_value_type>`

.. _class_controller_set_tracked_value:

- void **SetTrackedValue** **(** string_ valueName, float_ newValue **)**

Foribly set the skill to a new value.

.. _class_controller_evaluate:

- string **Evaluate** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge **)**

Evaluates the inserted :ref:`Output Query<class_output_query>` on the current :ref:`Profile<class_controller_create_profile>` and returns the string name of the estimated best output.

.. _class_controller_evaluate_on_curve:

- string **EvaluateOnCurve** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, float_ time **)**

Evaluates the inserted :ref:`Output Query<class_output_query>` on the current :ref:`Profile<class_controller_create_profile>` and returns the string name of the estimated best output.

The Desired Challenge will be modified by the :ref:`Micro Curve<class_micro_curve>` before being used to calculate an output.

.. _class_controller_eval_group:

- string[] **EvaluateGroupSelection** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, int_ count **)**

Evaluates the inserted :ref:`Output Query<class_output_query>` on the current :ref:`Profile<class_controller_create_profile>` and returns an array of size *count* which contains the estimated best group output.

.. _class_controller_eval_group_on_curve:

- string[] **EvaluateGroupSelection** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredChallenge, int_ count, float_ time **)**

Evaluates the inserted :ref:`Output Query<class_output_query>` on the current :ref:`Profile<class_controller_create_profile>` and returns an array of size *count* which contains the estimated best group output.

The Desired Challenge will be modified by the :ref:`Micro Curve<class_micro_curve>` before being used to calculate an output.

.. _class_controller_settings:

Master Settings
^^^^^^^^^^^^^^^

Explanation of the settings file, GenerateSettings(), and links to the other settings areas (just microcurve?)