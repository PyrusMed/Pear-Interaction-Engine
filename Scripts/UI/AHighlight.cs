using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.UI
{
	/// <summary>
	/// Highlights\Unhighlights an object based on the Highlight property
	/// </summary>
	/// <typeparam name="TObject">Type of object to highlight</typeparam>
	/// <typeparam name="THighlightProperty">Type of highlight property</typeparam>
	public abstract class AHighlight<TObject, THighlightProperty> : MonoBehaviour
	{
		[Tooltip("Highlight color")]
		public THighlightProperty Highlight;

		[Tooltip("Object to highlight")]
		public TObject ObjectToHighlight;

		// The original highlight color
		private THighlightProperty _originalHighlight;

		/// <summary>
		/// Tells whether or not the object is highlighted
		/// and controls setting the highlighted/original value
		/// </summary>
		private bool _highlight = false;
		public bool Highlighted
		{
			get { return _highlight; }
			set
			{
				_highlight = value;
				HighlightProperty = _highlight ? Highlight : _originalHighlight;
			}
		}

		/// <summary>
		/// The property used to determine the highlight value
		/// </summary>
		protected abstract THighlightProperty HighlightProperty { get; set; }

		/// <summary>
		/// Save the original value on awake
		/// </summary>
		protected virtual void Awake()
		{
			_originalHighlight = HighlightProperty;
		}
	}
}
