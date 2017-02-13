using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages all tooltips
/// </summary>
public class TooltipManager : MonoBehaviour
{
	// All tooltips
	private List<TouchTooltip> _tooltips = new List<TouchTooltip>();

	// Maps tooltips to model interactions
	private Dictionary<InputTypes.ModelInteractions, List<TouchTooltip>> _modelInteractionTooltipMap = new Dictionary<InputTypes.ModelInteractions, List<TouchTooltip>>();

	// Maps tooltips to global interactions
	private Dictionary<InputTypes.Global, List<TouchTooltip>> _globalInputTooltipMap = new Dictionary<InputTypes.Global, List<TouchTooltip>>();

	/// <summary>
	/// Singleton instance
	/// </summary>
	private static TooltipManager s_instance;
	public static TooltipManager Instance
	{
		get { return s_instance ?? (s_instance = new TooltipManager()); }
	}

	/// <summary>
	/// Gets all tooltips
	/// </summary>
	public List<TouchTooltip> Tooltips
	{
		get { return _tooltips.ToList(); }
	}

	private TooltipManager() { }

	/// <summary>
	/// Called by tooltips when they start. Adds tooltips to the list of all tooltips
	/// </summary>
	/// <param name="tooltip">Tooltip to register</param>
	public void Register(TouchTooltip tooltip)
	{
		_tooltips.Add(tooltip);

		// Is this associated with any model interactions?
		ModelInteractionTouchTolltipType modelInteractionTooltip = tooltip.GetComponent<ModelInteractionTouchTolltipType>();
		if (modelInteractionTooltip != null)
			AddToDictionary(_modelInteractionTooltipMap, modelInteractionTooltip.Type, tooltip);

		// Is this associated with any global interactions?
		GlobalInputTouchTooltipType globalInputTooltip = tooltip.GetComponent<GlobalInputTouchTooltipType>();
		if (globalInputTooltip != null)
			AddToDictionary(_globalInputTooltipMap, globalInputTooltip.Type, tooltip);
	}

	/// <summary>
	/// Get all tooltips associated with the model interaction
	/// </summary>
	/// <param name="type">type of interaction</param>
	/// <returns>List of tooltips associated with the interaction</returns>
	public List<TouchTooltip> GetTooltips(InputTypes.ModelInteractions type)
	{
		return _modelInteractionTooltipMap[type];
	}

	/// <summary>
	/// Get all tooltips associated with the input
	/// </summary>
	/// <param name="type">type of input</param>
	/// <returns>List of tooltips associated with the input</returns>
	public List<TouchTooltip> GetTooltips(InputTypes.Global type)
	{
		return _globalInputTooltipMap[type];
	}

	/// <summary>
	/// Associate the tooltip with its input
	/// </summary>
	/// <typeparam name="T">BoscInputType</typeparam>
	/// <param name="dict">Dictionary to update</param>
	/// <param name="key">Input type</param>
	/// <param name="tooltip">Tooltip</param>
	private void AddToDictionary<T>(Dictionary<T, List<TouchTooltip>> dict, T key, TouchTooltip tooltip)
	{
		if (!dict.ContainsKey(key))
			dict[key] = new List<TouchTooltip>();

		dict[key].Add(tooltip);
	}
}
