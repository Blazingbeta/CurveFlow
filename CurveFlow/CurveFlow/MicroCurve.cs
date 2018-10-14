using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NCalc;

namespace CurveFlow
{
	internal class MicroCurve
	{
		Expression equation;
		public MicroCurve(XmlDocument settings)
		{
			var data = settings.SelectSingleNode("/Settings/MicroCurve");
			equation = new Expression(data["Algorithm"].InnerText);
			if(data["PrecompileExpression"].InnerText == "true")
			{
				EvaluateExpression(1.0f, 1.0f);
			}
		}
		public float EvaluateExpression(float x, float t)
		{
			equation.Parameters["x"] = x;
			equation.Parameters["t"] = t;
			return Convert.ToSingle(((double)equation.Evaluate()));
		}
	}
}
