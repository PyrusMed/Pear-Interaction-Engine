using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pear.InteractionEngine.Properties.Converters
{
	public class BoolToVector3 : MonoBehaviour, IPropertyConverter<bool, Vector3>
	{
		public bool SetX = true;
		public bool SetY = true;
		public bool SetZ = true;

		public float FieldValue = 1f;

		public Vector3 Convert(bool convertFrom)
		{
			if (!convertFrom)
				return Vector3.zero;

			return new Vector3(SetX ? FieldValue : 0,
				SetY ? FieldValue : 0,
				SetZ ? FieldValue : 0);
		}
	}
}
