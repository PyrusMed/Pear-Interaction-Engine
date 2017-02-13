using Pear.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pear.Core.Controllers
{
    /// <summary>
    /// Based class for script that need to use a controller
    /// </summary>
    /// <typeparam name="T">Type of controller</typeparam>
    public class ControllerBehavior<T> : MonoBehaviour where T : Controller
    {
        private T _controller;
        protected T Controller
        {
            get
            {
                return _controller ?? (_controller = GetComponent<T>());
            }
        }
    }
}