using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts from raycast hit to bool.
	/// True when hit is equal to gameObject. False otherwise.
	/// </summary>
	public class RaycastHitToBool : MonoBehaviour, IPropertyConverter<RaycastHit?, bool>
	{
		public bool Convert(RaycastHit? convertFrom)
		{
			return convertFrom.HasValue ?
				convertFrom.Value.transform.gameObject == gameObject :
				false;
		}
	}
}
