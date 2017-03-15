using Pear.InteractionEngine.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace Pear.InteractionEngine.Examples
{
    public class TouchTooltip : ControllerBehavior<OculusTouchController>
    {

        [Tooltip("Path to controller element")]
        public string ControllerElementPath;

        [Tooltip("Height of the tooltip above the controller element")]
        public float HeightAboveControllerElement = 0.005f;

        // Fired when the tooltip resets
        public UnityEvent OnReset = new UnityEvent();

        // Key used to stop the timer before the tooltip is shown
        private Coroutine _timerKey;

        // Image renderer
        private SpriteRenderer _spriteRenderer;

        // Are we forcing the tooltip to be shown or hidden?
        private bool _force = false;

        // Use this for initialization
        void Start()
        {
            TooltipManager.Instance.Register(this);

            OvrAvatar avatar = FindObjectOfType<OvrAvatar>();
            avatar.AssetsDoneLoading.AddListener(Initialize);

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.flipY = true;
            _spriteRenderer.enabled = false; // Hide by default
        }

        void Initialize()
        {
            // Get the controller element that this will be parented to
            Transform controllerElement = Controller.transform.FindChild(ControllerElementPath);

            // Rotate the button so it lays flat on the controller
            transform.localEulerAngles = Vector3.zero + new Vector3(-90, 0, 0);
            transform.Rotate(transform.up, 5);

            // Set scale
            transform.localScale = Vector3.one * 0.02f;

            // Position the label above the button
            transform.position = controllerElement.position + Controller.transform.up * HeightAboveControllerElement;

            // Make the label a child of the controller element
            transform.SetParent(controllerElement, true);
        }

        public void Show()
        {
            if (_force)
                return;

            _spriteRenderer.enabled = true;
        }

        public void Hide()
        {
            if (_force)
                return;

            _spriteRenderer.enabled = false;
        }

        public void ForceShow()
        {
            Reset();
            Show();
            _force = true;
        }

        public void ForceHide()
        {
            Reset();
            Hide();
            _force = true;
        }

        public void Reset()
        {
            _force = false;
            Hide();
            OnReset.Invoke();
        }
    }
}