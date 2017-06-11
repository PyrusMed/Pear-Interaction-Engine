using Pear.InteractionEngine.Utils;
using System;
using UnityEditor;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers {
	[CustomEditor(typeof(ControllerBehaviorBase), true)]
	[CanEditMultipleObjects]
	public class ControllerBehaviorEditor : Editor {

		// The controller property
		SerializedProperty _controller;

		void OnEnable()
		{
			_controller = serializedObject.FindProperty("_controller");
		}

		/// <summary>
		/// Attempt to set the controller if it is not set
		/// </summary>
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			// If a reference to the controller is not set
			// try to find it on the game object
			if (_controller.objectReferenceValue == null)
			{
				// Get the type of controller by examing the templated argument
				// pased to the controller behavior
				Type typeOfController = ReflectionHelpers.GetGenericArgumentTypes(target.GetType(), typeof(IControllerBehavior<>))[0];

				// Check to see if this component has a controller on the same gameobject or one of its parents.
				Transform objTpCheck = ((Component)target).transform;
				while(_controller.objectReferenceValue == null && objTpCheck != null)
				{
					_controller.objectReferenceValue = objTpCheck.GetComponent(typeOfController);
					if (_controller.objectReferenceValue == null)
						objTpCheck = objTpCheck.parent;
				}
			}

			serializedObject.ApplyModifiedProperties();

			DrawDefaultInspector();
		}
	}
}
