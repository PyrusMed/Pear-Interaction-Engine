using System;
using System.Collections.Generic;

namespace Pear.InteractionEngine.Properties
{
	public interface IPropertyAction
	{
		string ActionName { get; }

		Type PropertyType { get; }
	}

	public static class PropertyActionManager
	{
		private static Dictionary<Type, List<IPropertyAction>> _actions = new Dictionary<Type, List<IPropertyAction>>();

		public static void RegisterAction(IPropertyAction action, Type type)
		{
			List<IPropertyAction> actions;
			if (!_actions.TryGetValue(type, out actions))
				_actions[type] = actions = new List<IPropertyAction>();

			actions.Add(action);
		}

		public static List<IPropertyAction> GetPropertyActions(Type[] types)
		{
			List<IPropertyAction> actions = new List<IPropertyAction>();
			foreach (Type type in types)
			{
				List<IPropertyAction> actionsForType;
				if (_actions.TryGetValue(type, out actionsForType))
				{
					actions.AddRange(actionsForType);
				}
			}

			return actions;
		}
	}
}