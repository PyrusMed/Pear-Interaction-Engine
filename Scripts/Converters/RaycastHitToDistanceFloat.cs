using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts from raycast hit to bool.
	/// True when hit is equal to gameObject. False otherwise.
	/// </summary>
	public class RaycastHitToDistanceFloat : MonoBehaviour, IPropertyConverter<RaycastHit?, float>
	{
		public float Convert(RaycastHit? convertFrom)
		{
			return convertFrom.HasValue ?
				convertFrom.Value.distance :
				1000000;
		}
	}
}
