using UnityEngine;

namespace Pear.InteractionEngine.Utils
{
	/// <summary>
	/// Extension methods for GameObjects
	/// </summary>
	public static class GameObjectExtensions
	{
		public static T AddComponentFrom<T>(this GameObject go, T from) where T : Component
		{
			if (from == null)
				return null;

			return go.AddComponent(from.GetType()).GetCopyOf(from) as T;
		}
	}
}
