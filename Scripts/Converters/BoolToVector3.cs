using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a bool to a vector 3
	/// </summary>
	public class BoolToVector3 : MonoBehaviour, IPropertyConverter<bool, Vector3>
	{
		[Tooltip("Set Field Value on vector.X?")]
		public bool SetX = true;

		[Tooltip("Set Field Value on vector.Y?")]
		public bool SetY = true;

		[Tooltip("Set Field Value on vector.Z?")]
		public bool SetZ = true;

		[Tooltip("Value to set on the new vector's fields")]
		public float FieldValue = 1f;

		/// <summary>
		/// Creates a vector based on the FieldValue
		/// and whether or not that value should be set on each field
		/// </summary>
		/// <param name="convertFrom">bool to convert from</param>
		/// <returns>Vector3 based on user settings</returns>
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
