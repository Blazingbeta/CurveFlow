.. _class_controller:

Curve Flow Controller
=====================

Description

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
| string     | :ref:`Evaluate<class_controller_evaluate>` **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredDifficulty **)**                                                          |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| string     | :ref:`EvaluateOnCurve<class_controller_evaluate_on_curve>` **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredDifficulty, float_ time **)**                             |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| string_ [] | :ref:`EvaluateGroupSelection<class_controller_eval_group>` **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredDifficulty, int_ count **)**                              |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+
| string_ [] | :ref:`EvaluateGroupSelectionOnCurve<class_controller_eval_group_on_curve>` **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredDifficulty, int_ count, float_ time **)** |
+------------+----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------+

.. _Stream: https://docs.microsoft.com/en-us/dotnet/api/system.io.stream?view=netframework-4.7.2

.. _int: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/int

.. _string: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/

.. _float: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/float



.. _class_controller_constructor:

- **CurveFlowController** **(** string_ xmlString **)**

.. _class_controller_initialize_log:

- void **InitializeLog** **(** :ref:`LogCallback<class_objects_logcallback>` log, :ref:`MessageType<class_objects_messagetype>` typeMask **)**

.. _class_controller_create_profile:

- void **CreateProfile** **(** :ref:`TrackedValue<class_objects_trackedvalue>` [] trackedValues **)**

.. _class_controller_load_profile:

- void **LoadProfile** **(** Stream_ profileStream **)**

.. _class_controller_save_profile:

- void **SaveProfile** **(** Stream_ outputStream **)**

.. _class_controller_get_current_value:

- float **GetCurrentValue** **(** string_ valueName **)**

.. _class_controller_append_tracked_value:

- void **AppendTrackedValue** **(** string_ valueName, float_ nextValue **)**

.. _class_controller_set_tracked_value:

- void **SetTrackedValue** **(** string_ valueName, float_ newValue **)**

.. _class_controller_evaluate:

- string **Evaluate** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredDifficulty **)**

.. _class_controller_evaluate_on_curve:

- string **EvaluateOnCurve** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredDifficulty, float_ time **)**

.. _class_controller_eval_group:

- string[] **EvaluateGroupSelection** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredDifficulty, int_ count **)**

.. _class_controller_eval_group_on_curve:

- string[] **EvaluateGroupSelection** **(** :ref:`OutputQuery<class_output_query>` query, float_ desiredDifficulty, int_ count, float_ time **)**