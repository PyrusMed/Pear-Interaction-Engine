using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Pear.InteractionEngine.Interactables.Behaviors
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

		private List<GameObjectProperty<bool>> _properties = new List<GameObjectProperty<bool>>();

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			_properties.Add(property);

			property.OnChange += HandleFade;

			FadeHelper fader = property.Owner.AddComponent<FadeHelper>();
			fader.fadeDelay = FadeDelay;
			fader.fadeTime = FadeTime;
			fader.fadeAplha = FadeAlpha;
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			property.OnChange -= HandleFade;
			_properties.Remove(property);
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
        /// If at least one object is hovered over, fade the rest out
        /// If no object is hovered over, fade them all in
        /// </summary>
        private void FadeAll()
        {
            // If at least one object is hovered over,
            //	Fade in all hovered objects
            //	Fade out all non-hovered objects
            if (_properties.Any((prop) => prop.Value))
            {
                foreach (GameObjectProperty<bool> property in _properties)
                {
                    FadeHelper fader = property.Owner.GetComponent<FadeHelper>();
                    if (property.Value)
                        fader.FadeIn();
                    else
                        fader.FadeOut();
                }
            }
            // Otherwise, fade in all objects
            else
            {
                foreach (GameObjectProperty<bool> property in _properties)
                {
                    FadeHelper fader = property.Owner.GetComponent<FadeHelper>();
                    fader.FadeIn();
                }
            }
        }
	}
}