using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Allows the user to specify which properties are effected by the conversion
	/// </summary>
	[CustomEditor(typeof(FloatToVector3))]
	public class FloatToVector3Editor : Editor
	{
		public override void OnInspectorGUI()
		{
			var floatToVector3 = target as FloatToVector3;

			// X
			floatToVector3.XSetToFloat = GUILayout.Toggle(floatToVector3.XSetToFloat, "X set to float");
			if (!floatToVector3.XSetToFloat)
				floatToVector3.XDefaultValue = EditorGUILayout.FloatField("Default value:", floatToVector3.XDefaultValue);

			// Y
			floatToVector3.YSetToFoat = GUILayout.Toggle(floatToVector3.YSetToFoat, "Y set to float");
			if (!floatToVector3.YSetToFoat)
				floatToVector3.YDefaultValue = EditorGUILayout.FloatField("Default value:", floatToVector3.YDefaultValue);

			// Z
			floatToVector3.ZSetToFloat = GUILayout.Toggle(floatToVector3.ZSetToFloat, "Z set to float");
			if (!floatToVector3.ZSetToFloat)
				floatToVector3.ZDefaultValue = EditorGUILayout.FloatField("Default value:", floatToVector3.ZDefaultValue);
		}
	}
}
