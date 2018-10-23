.. _class_micro_curve:

Micro Curve
=======================

The Micro Curve controls the challenge modifiers used in :ref:`EvaluateOnCurve<class_controller_evaluate_on_curve>` 
and :ref:`EvaluateGroupSelectionOnCurve<class_controller_eval_group_on_curve>` .
It takes in 2 variables, X for the baseline challenge and T for the time along the curve.
The equation used is defined in the :ref:`Controller Settings<class_controller_settings>`

.. _class_micro_curve_settings:

Settings
^^^^^^^^

There are only two settings in the MicroCurve ::

	<MicroCurve>
		<Algorithm>[x] * Sin([t])</Algorithm>
		<PrecompileExpression>true</PrecompileExpression>
	</MicroCurve>
	
- Algorithm

The algorithm that the Micro Curve will use when evaluating challenge ratings.
You can define your own algorithms using NCalc_. 

All you really need to worry about is the variables [x] and [t], which must be put in square brackets.

- Precompile Expression

Causes the Curve to automatically compile the expression before evaluation. If false, this will happen the first time you use the curve.

You probably always want this as true, unless you have a specific reason not to, such as if you plan on never using this feature.

The first expression evaluated in NCalc_ always takes a lot longer so doing this allows you to get that out of the way while still on a loading screen

.. _NCalc: https://archive.codeplex.com/?p=ncalc