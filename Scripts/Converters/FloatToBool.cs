using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a float to a bool
	/// </summary>
	public class FloatToBool : MonoBehaviour, IPropertyConverter<float, bool>
	{
		[Tooltip("Threshold that decides true or false")]
		public float Threshold = 0.5f;

		[Tooltip("If true, when float >= threshold the conversion will be true. If false, when the float is <= threshold the conversion will be false.")]
		public bool TrueWhenGreaterThanOrEqualToThreshold = true;

		public bool Convert(float convertFrom)
		{
			return TrueWhenGreaterThanOrEqualToThreshold ? 
				convertFrom >= Threshold :
				convertFrom <= Threshold;
		}
	}
}
