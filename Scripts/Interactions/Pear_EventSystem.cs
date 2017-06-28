using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// PIE's event system
	/// Almost a direction port from VTK
	/// https://github.com/thestonefox/VRTK/blob/25991cd6b9f935de07f2dcbd9ed1be82055e14f5/Assets/VRTK/Scripts/Internal/VRTK_EventSystem.cs
	/// </summary>
	public class Pear_EventSystem : EventSystem
	{
		// The previous event system used to keep track of state when enabling and disabling
		protected EventSystem _previousEventSystem;

		// The input module
		protected Pear_InputModule _inputModule;

		// Fields used to copy values
		private static readonly FieldInfo[] EVENT_SYSTEM_FIELD_INFOS = typeof(EventSystem).GetFields(BindingFlags.Public | BindingFlags.Instance);
		private static readonly PropertyInfo[] EVENT_SYSTEM_PROPERTY_INFOS = typeof(EventSystem).GetProperties(BindingFlags.Public | BindingFlags.Instance).Except(new[] { typeof(EventSystem).GetProperty("enabled") }).ToArray();
		private static readonly FieldInfo BASE_INPUT_MODULE_EVENT_SYSTEM_FIELD_INFO = typeof(BaseInputModule).GetField("m_EventSystem", BindingFlags.NonPublic | BindingFlags.Instance);

		protected override void Awake()
		{
			base.Awake();

			InitializeEvents();
		}

		/// <summary>
		/// Make sure each object that implements IEvent has the event object set
		/// </summary>
		private void InitializeEvents()
		{
			// Loop over every object
			// If it's an event, set it's event property
			UnityEngine.Object[] objectsInScene = Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour));
			Type iEventType = typeof(IEvent<>);
			Type propertyType = typeof(Property<>);
			int eventCounter = 0;
			foreach (MonoBehaviour mono in objectsInScene)
			{
				Type eventType = mono.GetType().GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == iEventType);
				if (eventType != null)
				{
					// Create an instance of Property<T>
					Type eventValueType = eventType.GetGenericArguments()[0];
					Type eventPropertyType = propertyType.MakeGenericType(eventValueType);
					object eventPropertyValue = Activator.CreateInstance(eventPropertyType);

					// Set it on the script
					PropertyInfo eventProperty = eventType.GetProperty("Event");
					eventProperty.SetValue(mono, eventPropertyValue, null);
					eventCounter++;
				}
			}
		}

		protected override void OnEnable()
		{
			_previousEventSystem = current;
			if (_previousEventSystem != null)
			{
				_previousEventSystem.enabled = false;
				CopyValuesFrom(_previousEventSystem, this);
			}

			_inputModule = transform.GetOrAddComponent<Pear_InputModule>();
			base.OnEnable();
			StartCoroutine(SetEventSystemOfBaseInputModulesAfterFrameDelay(this));
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			Destroy(_inputModule);

			if (_previousEventSystem != null)
			{
				_previousEventSystem.enabled = true;
				CopyValuesFrom(this, _previousEventSystem);
				SetEventSystemOfBaseInputModules(_previousEventSystem);
			}
		}

		protected override void Update()
		{
			base.Update();

			if (current == this)
			{
				_inputModule.Process();
			}
		}

		protected override void OnApplicationFocus(bool hasFocus)
		{
			//Don't call the base implementation because it will set a pause flag for this EventSystem
		}

		/// <summary>
		/// Copies values from one event system to another
		/// </summary>
		/// <param name="fromEventSystem">copy from</param>
		/// <param name="toEventSystem">copy to</param>
		private static void CopyValuesFrom(EventSystem fromEventSystem, EventSystem toEventSystem)
		{
			foreach (FieldInfo fieldInfo in EVENT_SYSTEM_FIELD_INFOS)
			{
				fieldInfo.SetValue(toEventSystem, fieldInfo.GetValue(fromEventSystem));
			}

			foreach (PropertyInfo propertyInfo in EVENT_SYSTEM_PROPERTY_INFOS)
			{
				if (propertyInfo.CanWrite)
				{
					propertyInfo.SetValue(toEventSystem, propertyInfo.GetValue(fromEventSystem, null), null);
				}
			}
		}

		/// <summary>
		/// Used to force update a private reference.
		/// </summary>
		/// <param name="eventSystem">event system to update</param>
		/// <returns></returns>
		private static IEnumerator SetEventSystemOfBaseInputModulesAfterFrameDelay(EventSystem eventSystem)
		{
			yield return null;
			SetEventSystemOfBaseInputModules(eventSystem);
		}

		/// <summary>
		/// BaseInputModule has a private field referencing the current EventSystem. That field is set in
		/// BaseInputModule.OnEnable only.It's used in BaseInputModule.OnEnable and BaseInputModule.OnDisable
		/// to call EventSystem.UpdateModules.
		/// This means we could just disable and enable every enabled BaseInputModule to fix that reference.
		/// But the StandaloneInputModule (which is added by default when adding an EventSystem in the Editor)
		/// requires EventSystem.Which means we can't correctly destroy the old EventSystem first and then add
		/// our own one.
		/// We therefore update that private reference directly here.
		/// </summary>
		/// <param name="eventSystem"></param>
		private static void SetEventSystemOfBaseInputModules(EventSystem eventSystem)
		{
			foreach (BaseInputModule module in FindObjectsOfType<BaseInputModule>())
			{
				BASE_INPUT_MODULE_EVENT_SYSTEM_FIELD_INFO.SetValue(module, eventSystem);
			}

			eventSystem.UpdateModules();
		}
	}
}