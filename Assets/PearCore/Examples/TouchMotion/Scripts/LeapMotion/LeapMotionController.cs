using Leap.Unity;
using Pear.Core.Controllers;
using Pear.Core.Interactables;

/// <summary>
/// Defines how the leap motion controller interacts with objects and the environment
/// </summary>
public class LeapMotionController : Controller
{
	public RigidHand PhysicsHand;

	/// <summary>
	 /// The hand associated with this leap motion controller
	 /// NOTE:
	 ///		Each hand will have its own leap motion controller object
	 /// </summary>
	public HandModel Hand
	{
		get;
		private set;
	}

	void Awake()
	{
		InUse = true;
    }

	// Hook up events
	void Start()
	{
		Hand = GetComponent<HandModel>();

		// When either controller is enabled activate it in the hierarchy
		OnStartUsing.AddListener((c) => SetActiveFromEnabledDisabled(true));

		// When either controller is disabled deactivate it in the hierarchy
		OnStopUsing.AddListener((c) => SetActiveFromEnabledDisabled(false));

        AttachLeapRTS();
        AttachHoverEvents();
	}

	void OnEnable()
	{
		if (!InUse)
			SetActiveFromEnabledDisabled(false);
	}

	void SetActiveFromEnabledDisabled(bool active)
	{
		gameObject.SetActive(active);
		PhysicsHand.gameObject.SetActive(active);
	}

	private void AttachLeapRTS()
	{
		foreach(InteractableObject interactable in InteractableObjectManager.Instance.AllObjects)
		{
			// Add the LeapRTS component which controls movement
			LeapRTS rts = interactable.gameObject.AddComponent<LeapRTS>();
			rts.OneHandedRotationMethod = LeapRTS.RotationMethod.Full;
			rts.TwoHandedRotationMethod = LeapRTS.RotationMethod.Single;

			rts.OnGrabChanged.AddListener((r) =>
			{
				if (r.Grabbing)
				{
					interactable.Moving.Add(this);
					ActiveObject = interactable;
				}
				else
				{
					interactable.Moving.Remove(this);
					ActiveObject = null;
				}
			});

			rts.OnZoomChanged.AddListener((r) =>
			{
				if (r.Zooming)
					interactable.Resizing.Add(this);
				else
					interactable.Resizing.Remove(this);
			});
		}
	}

	/// <summary>
	/// When the leap motion hand starts or stops hovering over a object, update it.
	/// </summary>
	private void AttachHoverEvents()
	{
		PinchDetector pinchDetector = Hand.gameObject.GetComponentInChildren<PinchDetector>();
		ProximityDetector proximityDetector = Hand.gameObject.GetComponentInChildren<ProximityDetector>();

		// When the hand starts hovering over the object
		// let the object know this controller
		// is hovering over it
		InteractableObject lastHoveredObject = null;
		proximityDetector.OnProximity.AddListener((obj) =>
		{
			InteractableObject interactable = lastHoveredObject = obj.GetComponent<InteractableObject>();
			if (interactable != null)
				interactable.Hovering.Add(this);
		});

		// When the hand stops hovering over the object
		// let the object know this controller
		// is no longer hovering over it
		proximityDetector.OnDeactivate.AddListener(() =>
		{
			if (lastHoveredObject != null)
				lastHoveredObject.Hovering.Remove(this);
		});
	}
}
