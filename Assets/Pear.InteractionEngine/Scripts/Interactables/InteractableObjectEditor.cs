using Pear.InteractionEngine.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Pear.InteractionEngine.Interactables
{
	[CustomEditor(typeof(InteractableObject))]
	public class InteractableObjectEditor : Editor
	{
		private bool _addingInteraction = false;
		private string[] _changers;

		void OnEnable()
		{
			_changers = GetChangers();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (GUILayout.Button("Add Interaction"))
				_addingInteraction = true;

			if (_addingInteraction)
			{
				GUILayout.BeginVertical();
				EditorGUILayout.Popup(0, _changers);
				GUILayout.EndVertical();
			}


			//EditorGUILayout.PropertyField(lookAtPoint);
			serializedObject.ApplyModifiedProperties();
		}

		private string[] GetChangers()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<string> changers = new List<string>();
			foreach (Assembly assembly in assemblies)
			{
				IEnumerable<Type> types = assembly.GetTypes().Where(t =>
				{
					foreach(Type i in t.GetInterfaces())
					{
						if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPropertyChanger<>))
							return true;
					}

					return false;
				});

				changers.AddRange(types.Select(t => PropertyChangerHelpers.GetPropertyName(t)));
			}

			return changers.ToArray();
		}
	}
}