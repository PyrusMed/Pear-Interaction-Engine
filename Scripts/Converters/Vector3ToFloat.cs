using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a vector3 to a float
	/// </summary>
	public class Vector3ToFloat : MonoBehaviour, IPropertyConverter<Vector3, float>
	{
		/// <summary>
		/// Returns the magnitude of this vector
		/// </summary>
		/// <param name="convertFrom">Vector to use during the conversion</param>
		/// <returns>Magnitude of vector 3</returns>
		public float Convert(Vector3 convertFrom)
		{
			return convertFrom.magnitude;
		}
	}
}
