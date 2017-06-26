using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pear.InteractionEngine.Properties.Converters
{
	public class BoolToVector3 : MonoBehaviour, IPropertyConverter<bool, Vector3>
	{
		public VectorField FieldToUpdate = VectorField.X;

		public float FieldValue = 1f;

		public Vector3 Convert(bool convertFrom)
		{
			if (!convertFrom)
				return Vector3.zero;

			return new Vector3(FieldToUpdate == VectorField.X ? FieldValue : 0,
				FieldToUpdate == VectorField.Y ? FieldValue : 0,
				FieldToUpdate == VectorField.Z ? FieldValue : 0);
		}
	}

	public enum VectorField
	{
		X,
		Y,
		Z,
	}
}
