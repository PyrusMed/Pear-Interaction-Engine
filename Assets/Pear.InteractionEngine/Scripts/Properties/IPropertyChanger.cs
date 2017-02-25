using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Properties
{
	public interface IPropertyChanger<T>
	{
		void RegisterProperty(GameObjectProperty<T> property);
	}

	public static class PropertyChangerHelpers
	{
		public static string GetPropertyName<T>(this IPropertyChanger<T> changer)
		{
			return GetPropertyName(changer.GetType());
		}

		public static string GetPropertyName(Type type)
		{
			PropertyChangerAttribute attribute = Attribute.GetCustomAttribute(type, typeof(PropertyChangerAttribute)) as PropertyChangerAttribute;
			if(attribute == null)
				throw new MissingAttributeException(string.Format("{0} is missing the PropertyChangerAttribute", type));

			return attribute.PropertyName;
		}

		public static Type GetPropertyType<T>(this IPropertyChanger<T> changer)
		{
			return typeof(T);
		}
	}

	public static class PropertyChangerManager
	{

		private static Dictionary<string, List<Type>> _changers = new Dictionary<string, List<Type>>();

		public static void RegisterChanger<T>(IPropertyChanger<T> changer)
		{
			List<Type> types;
			string propertyName = changer.GetPropertyName();
			if (!_changers.TryGetValue(propertyName, out types))
				_changers[propertyName] = types = new List<Type>();

			types.Add(changer.GetPropertyType());
		}
	}
}