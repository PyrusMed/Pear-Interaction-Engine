using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;


namespace Pear.InteractionEngine.Interactables
{
	public static class ExposeProperties
	{
		public static void Expose(SerializedFieldInfo[] fields)
		{

			GUILayoutOption[] emptyOptions = new GUILayoutOption[0];

			EditorGUILayout.BeginVertical(emptyOptions);

			foreach (SerializedFieldInfo field in fields)
			{
				EditorGUILayout.BeginHorizontal(emptyOptions);

				switch (field.SerializedType)
				{
					case SerializedPropertyType.Integer:
						field.Value = EditorGUILayout.IntField(field.Name, (int)field.Value, emptyOptions);
						break;

					case SerializedPropertyType.Float:
						field.Value = EditorGUILayout.FloatField(field.Name, (float)field.Value, emptyOptions);
						break;

					case SerializedPropertyType.Boolean:
						field.Value = EditorGUILayout.Toggle(field.Name, (bool)field.Value, emptyOptions);
						break;

					case SerializedPropertyType.String:
						field.Value = EditorGUILayout.TextField(field.Name, (String)field.Value, emptyOptions);
						break;

					case SerializedPropertyType.Vector2:
						field.Value = EditorGUILayout.Vector2Field(field.Name, (Vector2)field.Value, emptyOptions);
						break;

					case SerializedPropertyType.Vector3:
						field.Value = EditorGUILayout.Vector3Field(field.Name, (Vector3)field.Value, emptyOptions);
						break;

					case SerializedPropertyType.Enum:
						field.Value = EditorGUILayout.EnumPopup(field.Name, (Enum)field.Value, emptyOptions);
						break;

					case SerializedPropertyType.ObjectReference:
						field.Value = EditorGUILayout.ObjectField(field.Name, (UnityEngine.Object)field.Value, field.Info.FieldType, true, emptyOptions);
						break;

					default:
						break;

				}

				EditorGUILayout.EndHorizontal();

			}

			EditorGUILayout.EndVertical();

		}

		public static SerializedFieldInfo[] GetNonUnityProperties(Type type)
		{
			List<SerializedFieldInfo> fields = new List<SerializedFieldInfo>();

			FieldInfo[] infos = type.GetFields();

			foreach (FieldInfo info in infos)
			{
				// Ignore properties from MonoBehavior base classes and below
				if (info.DeclaringType.ToString().StartsWith("UnityEngine."))
					continue;

				fields.Add(new SerializedFieldInfo(info));
			}

			return fields.ToArray();
		}

	}



	public class SerializedFieldInfo
	{
		public SerializedPropertyType SerializedType
		{
			get;
			private set;
		}

		public String Name
		{
			get
			{
				return ObjectNames.NicifyVariableName(Info.Name);
			}
		}

		public FieldInfo Info
		{
			get;
			private set;
		}

		public object Value
		{
			get;
			set;
		}

		public SerializedFieldInfo(FieldInfo info)
		{
			Info = info;
			SerializedType = GetPropertyType();
			Value = info.FieldType.IsValueType ? Activator.CreateInstance(info.FieldType) : null;
		}

		private SerializedPropertyType GetPropertyType()
		{
			Type type = Info.FieldType;

			if (type == typeof(int))
				return SerializedPropertyType.Integer;

			if (type == typeof(float))
				return SerializedPropertyType.Float;

			if (type == typeof(bool))
				return SerializedPropertyType.Boolean;

			if (type == typeof(string))
				return SerializedPropertyType.String;

			if (type == typeof(Vector2))
				return SerializedPropertyType.Vector2;

			if (type == typeof(Vector3))
				return SerializedPropertyType.Vector3;

			if (type.IsEnum)
				return SerializedPropertyType.Enum;

			return SerializedPropertyType.ObjectReference;
		}

	}
}
