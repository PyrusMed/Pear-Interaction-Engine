using System.Collections;
using UnityEngine;

namespace Pear.Core.Examples
{
    /// <summary>
    /// Activates/Deactivates a tooltip. Uses a timer to show and hide the tooltip based
    /// on how the user is interacting with the tooltip's button
    /// </summary>
    public abstract class TouchTooltipActivator : MonoBehaviour
    {
        [Tooltip("Waiting period before popping up the tooltip")]
        public float PopupDelay = 3f;

        protected TouchController _touchController;
        protected TouchTooltip _touchTooltip;

        // Key used to stop the timer before the tooltip is shown
        private Coroutine _timerKey;

        // Use this for initialization
        void Start()
        {
            _touchController = transform.parent.GetComponent<TouchController>();
            _touchTooltip = GetComponent<TouchTooltip>();

            _touchTooltip.OnReset.AddListener(() => StopTimerAndHideTooltip());
        }

        // Update is called once per frame
        void Update()
        {
            // If the timer key is null check to see if we should start the timer
            if (_timerKey == null)
            {
                if (ShouldStartTimer())
                    _timerKey = StartCoroutine(StartTimerToShowTooltip());
            }
            // Otherwise, check to see if you should cancel the timer
            else
            {
                if (ShouldStopTimer())
                    StopTimerAndHideTooltip();
            }
        }

        /// <summary>
        /// Should we start the tooltip timer?
        /// </summary>
        /// <returns>True if the timer should start. False otherwise</returns>
        protected abstract bool ShouldStartTimer();

        /// <summary>
        /// Should we stop the tooltip timer?
        /// </summary>
        /// <returns>True if the timer should stop. False otherwise</returns>
        protected abstract bool ShouldStopTimer();

        /// <summary>
        /// Start a timer that, when finished, will show the tooltip
        /// </summary>
        /// <returns>Enumerator for coroutine</returns>
        IEnumerator StartTimerToShowTooltip()
        {
            Debug.Log("Starting timer");
            yield return new WaitForSeconds(PopupDelay);

            Debug.Log("Starting ended. Showing tooltip");
            _touchTooltip.Show();
        }

        /// <summary>
        /// Stops the timer and hides the tooltip
        /// </summary>
        void StopTimerAndHideTooltip()
        {
            Debug.Log("Stopping timer");

            if (_timerKey != null)
                StopCoroutine(_timerKey);
            _timerKey = null;

            _touchTooltip.Hide();
        }
    }
}