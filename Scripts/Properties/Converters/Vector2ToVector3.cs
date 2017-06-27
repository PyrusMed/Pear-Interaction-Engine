using UnityEngine;

namespace Pear.InteractionEngine.Properties.Converters
{
	public class Vector2ToVector3 : MonoBehaviour, IPropertyConverter<Vector2, Vector3>
	{
		public Vector3 Convert(Vector2 convertFrom)
		{
			return convertFrom;
		}
	}
}
