using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Converts a game object to a bool
	/// </summary>
	public class GameObjectToBool : MonoBehaviour, IPropertyConverter<GameObject, bool>
	{
		public bool Convert(GameObject convertFrom)
		{
			return convertFrom == gameObject;
		}
	}
}
