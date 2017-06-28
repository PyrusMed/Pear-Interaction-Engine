using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a float to a vector3
	/// </summary>
	public class FloatToVector3 : MonoBehaviour, IPropertyConverter<float, Vector3>
	{
		[Header("X")]
		[SerializeField]
		public bool XSetToFloat = true;

		[SerializeField]
		public float XDefaultValue = 0;

		[Header("Y")]
		[SerializeField]
		public bool YSetToFoat = true;

		[SerializeField]
		public float YDefaultValue = 0;

		[Header("Z")]
		[SerializeField]
		public bool ZSetToFloat = true;

		[SerializeField]
		public float ZDefaultValue = 0;

		/// <summary>
		/// Sets the float on certain fields of the vector based on user preferences
		/// </summary>
		/// <param name="convertFrom">Float to use during the conversion</param>
		/// <returns>A float with specific fields set based on user preferences</returns>
		public Vector3 Convert(float convertFrom)
		{
			return new Vector3(
				!XSetToFloat ? XDefaultValue : convertFrom,
				!YSetToFoat ? YDefaultValue : convertFrom,
				!ZSetToFloat ? ZDefaultValue : convertFrom);
		}
	}
}
