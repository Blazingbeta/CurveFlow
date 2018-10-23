.. _class_objects:


Misc Objects
============

Some of the smaller objects created by CurveFlow

.. _class_objects_trackedvalue:

**TrackedValue**
----------------

- **TrackedValue** **(** float_ min, float_ max, string_ name, :ref:`ValueType<class_objects_value_type>` type **)**
- float_ min , the lowest possible value this object can reach
- float_ max , the highest possible value this object can reach
- string_ name , the name of the skill to be tracked
- :ref:`ValueType<class_objects_value_type>` , the method used to append new values into this object

.. _class_objects_logcallback:

**LogCallback**
---------------

- delegate void **LogCallback** **(** string_ logMessage, :ref:`MessageType<class_objects_messagetype>` type **)**

The delegate used to bind CurveFlow's logging to your engine. Doesn't need to be explicity created, just pass a function with the same parameters 
to :ref:`Controller.InitializeLog<class_controller_initialize_log>`

.. _class_objects_value_type:

**ValueType**
-------------

- enum ValueType { **ADDITIVE**, **AVERAGE**, **SET**}
- ADDITIVE: New values are added/subtracted to the total
- AVERAGE: New values are averaged based on previous numbers entered.
- SET: New values can only be set using :ref:`Controller.SetTrackedValue<class_controller_set_tracked_value>`

.. _class_objects_messagetype:

**MessageType**
---------------

- enum MessageType {**STATUS** = 1, **DEBUG** = 2, **ERROR** = 4}

.. _string: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/

.. _float: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/float
