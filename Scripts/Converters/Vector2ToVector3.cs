using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a Vector2 to Vector3
	/// </summary>
	public class Vector2ToVector3 : MonoBehaviour, IPropertyConverter<Vector2, Vector3>
	{
		public Vector3 Convert(Vector2 convertFrom)
		{
			return convertFrom;
		}
	}
}
