using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CurveFlow
{
	internal class MicroCurve
	{
		private MicroCurveExpression m_expression;
		public MicroCurve(MicroCurveExpression exp)
		{
			m_expression = exp;
		}
		public float EvaluateExpression(float x, float t)
		{
			return m_expression(x, t);
		}
	}
}
