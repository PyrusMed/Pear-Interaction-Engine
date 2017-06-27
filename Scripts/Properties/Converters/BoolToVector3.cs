using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pear.InteractionEngine.Properties.Converters
{
	public class BoolToVector3 : MonoBehaviour, IPropertyConverter<bool, Vector3>
	{
		public VectorField[] FieldsToUpdate;

		private bool _updateX;
		private bool _updateY;
		private bool _updateZ;

		public float FieldValue = 1f;

		private void Awake()
		{
			Dictionary<VectorField, Action> fieldSetter = new Dictionary<VectorField, Action>()
			{
				{ VectorField.X, () => _updateX = true },
				{ VectorField.Y, () => _updateY = true },
				{ VectorField.Z, () => _updateZ = true },
			};

			// Set the appropriate var
			foreach(VectorField fieldToUpdate in FieldsToUpdate)
				fieldSetter[fieldToUpdate]();
		}

		public Vector3 Convert(bool convertFrom)
		{
			if (!convertFrom)
				return Vector3.zero;

			return new Vector3(_updateX ? FieldValue : 0,
				_updateY ? FieldValue : 0,
				_updateZ ? FieldValue : 0);
		}
	}

	public enum VectorField
	{
		X,
		Y,
		Z,
	}
}
