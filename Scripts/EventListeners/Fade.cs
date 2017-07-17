using UnityEngine;
using System;
using Pear.InteractionEngine.Utils;
using Pear.InteractionEngine.Events;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Fades an object in when the event is true.
	/// Fades the object out when the event is false and at least one other object is faded in.
	/// </summary>
	[Serializable]
	public class Fade : MonoBehaviour, IEventListener<bool>
    {
        [Tooltip("Seconds between when the controller hovers over the object and when fading starts")]
        public float FadeDelay = 0.0f;

        [Tooltip("Seconds is takes for the fade to complete")]
        public float FadeTime = 0.5f;

        [Tooltip("Opacity that objects fade to")]
        public float FadeAlpha = 0.3f;

		[Tooltip("Manages fadable objects")]
		public FadeManager Manager;

		// Does the fading
		private FadeHelper _fader;

		private void Awake()
		{
			// Make sure we initialize the fade helper, which does the fading
			_fader = transform.GetOrAddComponent<FadeHelper>();
			_fader.fadeDelay = FadeDelay;
			_fader.fadeTime = FadeTime;
			_fader.fadeAplha = FadeAlpha;

			// Fade this object based on the manager's state
			Manager.FadableObjectCountChangedEvent += FadeCountChanged;
		}

		private void OnDestroy()
		{
			Manager.FadableObjectCountChangedEvent -= FadeCountChanged;
		}

		private void FadeCountChanged(int count)
		{
			// If there are no items in the manager or if this object is in the manager
			// make sure it's faded in
			if (count <= 0 || Manager.IsRegistered(gameObject))
				_fader.FadeIn();
			// Otherwise, fade this object out
			else
				_fader.FadeOut();
		}

		/// <summary>
		/// Fade in when true, out when false
		/// </summary>
		/// <param name="args">Event args</param>
		public void ValueChanged(EventArgs<bool> args)
		{
			if (args.NewValue)
				Manager.RegisterFadableObject(gameObject);
			else
				Manager.UnregisterFadableObject(gameObject);
		}
	}
}
