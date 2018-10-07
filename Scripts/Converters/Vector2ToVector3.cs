using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a Vector2 to Vector3
	/// </summary>
	public class Vector2ToVector3 : MonoBehaviour, IPropertyConverter<Vector2, Vector3>
	{
		[Tooltip("Where should the X value be set")]
		public VectorFields SetXOn = VectorFields.X;

		[Tooltip("Multiplied on the X value when it's set")]
		public float XMultiplier = 1;

		[Tooltip("Where should the Y value be set")]
		public VectorFields SetYOn = VectorFields.Y;

		[Tooltip("Multiplied on the Y value when it's set")]
		public float YMultiplier = 1;

		public bool DebugLogs = false;

		public Vector3 Convert(Vector2 convertFrom)
		{
			float x = 0;
			float y = 0;
			float z = 0;

			Dictionary<VectorFields, Action<float>> setActions = new Dictionary<VectorFields, Action<float>>()
			{
				{ VectorFields.X, val => x = val },
				{ VectorFields.Y, val => y = val },
				{ VectorFields.Z, val => z = val },
			};

			setActions[SetXOn](convertFrom.x * XMultiplier);
			setActions[SetYOn](convertFrom.y * YMultiplier);

			if (DebugLogs)
				Debug.Log(String.Format("[Vector2ToVector3] Converted {0} to {1}", convertFrom, new Vector3(x, y, z)));

			return new Vector3(x, y, z);
		}
	}

	public enum VectorFields
	{
		X,
		Y,
		Z,
	}
}
