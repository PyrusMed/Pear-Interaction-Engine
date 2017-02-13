using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pear.Core.Examples
{
    public class TouchTooltip : MonoBehaviour
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

        private TouchController _touchController;

        // Use this for initialization
        void Start()
        {
            TooltipManager.Instance.Register(this);

            _touchController = transform.parent.GetComponent<TouchController>();

            OvrAvatar avatar = FindObjectOfType<OvrAvatar>();
            avatar.AssetsDoneLoading.AddListener(Initialize);

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.flipY = true;
            _spriteRenderer.enabled = false; // Hide by default
        }

        void Initialize()
        {
            // Get the parent controller
            TouchController parentController = transform.parent.GetComponent<TouchController>();

            // Get the controller element that this will be parented to
            Transform controllerElement = parentController.transform.FindChild(ControllerElementPath);

            // Rotate the button so it lays flat on the controller
            transform.localEulerAngles = Vector3.zero + new Vector3(-90, 0, 0);
            transform.Rotate(transform.up, 5);

            // Set scale
            transform.localScale = Vector3.one * 0.02f;

            // Position the label above the button
            transform.position = controllerElement.position + parentController.transform.up * HeightAboveControllerElement;

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