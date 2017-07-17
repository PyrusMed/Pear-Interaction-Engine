using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a vector2 to a float
	/// </summary>
	public class Vector2ToFloat : MonoBehaviour, IPropertyConverter<Vector2, float>
	{
		[Tooltip("The value to use during the conversion")]
		public ValueToUse Method = ValueToUse.X;

		/// <summary>
		/// Returns the X, Y or magnitude value of this vector
		/// </summary>
		/// <param name="convertFrom">Vector to use during the conversion</param>
		/// <returns>Value based on the conversion method</returns>
		public float Convert(Vector2 convertFrom)
		{
			return Method == ValueToUse.X ? convertFrom.x :
				Method == ValueToUse.Y ? convertFrom.y :
				convertFrom.magnitude;
		}

		public enum ValueToUse
		{
			X,
			Y,
			Magnitude
		}
	}
}
