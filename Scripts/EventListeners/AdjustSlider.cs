using System;
using System.Collections;
using System.Collections.Generic;
using Pear.InteractionEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Pear.InteractionEngine.EventListeners
{
	public class AdjustSlider : MonoBehaviour, IEventListener<float>
	{
		[Tooltip("The slider to adjust")]
		public Slider Slider;

		[Tooltip("The speed to adjust at")]
		public float Speed = 1;

		// The velocity to change at
		private float _velocity;

		public void ValueChanged(EventArgs<float> args)
		{
			_velocity = args.NewValue * Speed;
			Debug.Log("Setting velocity: " + _velocity);
		}

		private void Update()
		{
			Slider.value += _velocity * Time.deltaTime;
		}
	}
}
