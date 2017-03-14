using Pear.InteractionEngine.Properties;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Manages what happens when object are hovered over
	/// </summary>
	[Serializable]
	public class Fade : MonoBehaviour, IGameObjectPropertyEventHandler<bool>
    {
        [Tooltip("Seconds between when the controller hovers over the object and when fading starts")]
        public float FadeDelay = 0.0f;

        [Tooltip("Seconds is takes for the fade to complete")]
        public float FadeTime = 0.5f;

        [Tooltip("Opacity that objects fade to")]
        public float FadeAlpha = 0.3f;

		// Maps a game object to its fade state
		private Dictionary<GameObject, FadeState> _gameObjectToFadeStateMap = new Dictionary<GameObject, FadeState>();

		/// <summary>
		/// Begins tracking this object's fade state
		/// </summary>
		/// <param name="property">property used to track state</param>
		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			StoreProperty(property);

			property.ChangeEvent += HandleFade;

			// Make sure we initialize the fade helper, which does the fading
			FadeHelper fader = property.Owner.transform.GetOrAddComponent<FadeHelper>();
			fader.fadeDelay = FadeDelay;
			fader.fadeTime = FadeTime;
			fader.fadeAplha = FadeAlpha;
		}

		/// <summary>
		/// Stops tracking this object's fade state
		/// </summary>
		/// <param name="property"></param>
		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			property.ChangeEvent -= HandleFade;

			ForgetProperty(property);
		}

		/// <summary>
		/// Saves this property in a FadeState object, which links all properties associated with
		/// the same GameObject
		/// </summary>
		/// <param name="property">property to store</param>
		private void StoreProperty(GameObjectProperty<bool> property)
		{
			FadeState gameObjectFadeState = null;
			if (!_gameObjectToFadeStateMap.TryGetValue(property.Owner, out gameObjectFadeState))
				_gameObjectToFadeStateMap[property.Owner] = gameObjectFadeState = new FadeState();
			gameObjectFadeState.Add(property);
		}

		/// <summary>
		/// Removes the property from it's FadeState object. In effect, we will
		/// no longer use this property when determining whether the object should fade
		/// </summary>
		/// <param name="property">Property to remove</param>
		private void ForgetProperty(GameObjectProperty<bool> property)
		{
			FadeState gameObjectFadeState = _gameObjectToFadeStateMap[property.Owner];
			gameObjectFadeState.Remove(property);
			if (!gameObjectFadeState.HasProperties)
				_gameObjectToFadeStateMap.Remove(property.Owner);
		}

		/// <summary>
		/// Event handler for hover events
		/// </summary>
		/// <param name="e"></param>
		private void HandleFade(bool oldHoverValue, bool newHoverValue)
        {
            FadeAll();
        }

        /// <summary>
        /// Fade all objects
        /// If at least one object should stand out, fade it in and fade the rest out
        /// If no object should stand out, fade them all in
        /// </summary>
        private void FadeAll()
        {
            // If at least one object should stand out,
            //	Fade in all objects that should stand out
            //	Fade out all objects that should not stand out
            if (_gameObjectToFadeStateMap.Any(kvp => kvp.Value.ShouldStandOut))
            {
				foreach(KeyValuePair<GameObject, FadeState> gameObjectFadeState in _gameObjectToFadeStateMap)
                {
                    FadeHelper fader = gameObjectFadeState.Key.GetComponent<FadeHelper>();
                    if (gameObjectFadeState.Value.ShouldStandOut)
                        fader.FadeIn();
                    else
                        fader.FadeOut();
                }
            }
            // Otherwise, fade in all objects
            else
            {
				foreach (KeyValuePair<GameObject, FadeState> gameObjectFadeState in _gameObjectToFadeStateMap)
				{
                    FadeHelper fader = gameObjectFadeState.Key.GetComponent<FadeHelper>();
                    fader.FadeIn();
                }
            }
        }

		/// <summary>
		/// Tracks the fade state of a GameObject by aggregating the values of each of its properties
		/// </summary>
		private class FadeState
		{
			// List of properties associated with a single GameObject
			private List<GameObjectProperty<bool>> _properties = new List<GameObjectProperty<bool>>();

			/// <summary>
			/// Does this FadeState have any properties?
			/// </summary>
			public bool HasProperties
			{
				get { return _properties.Count > 0; }
			}

			/// <summary>
			/// Should the GameObject associated with this FadeState stand out?
			/// </summary>
			public bool ShouldStandOut
			{
				get { return _properties.Any(p => p.Value); }
			}

			/// <summary>
			/// Associates a new property with this FadeState
			/// </summary>
			/// <param name="property">property to associate</param>
			public void Add(GameObjectProperty<bool> property)
			{
				_properties.Add(property);
			}

			/// <summary>
			/// Disassociates a property with this FadeState
			/// </summary>
			/// <param name="property">property to remove</param>
			public void Remove(GameObjectProperty<bool> property)
			{
				_properties.Remove(property);
			}
		}
	}
}