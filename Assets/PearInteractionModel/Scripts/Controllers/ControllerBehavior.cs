using PearMed.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PearMed.Controllers
{
    /// <summary>
    /// Based class for script that need to use a controller
    /// </summary>
    /// <typeparam name="T">Type of controller</typeparam>
    public class ControllerBehavior<T> : MonoBehaviour where T : Controller
    {

        protected T Controller { get; private set; }

        void Start()
        {
            Controller = GetComponent<T>();
        }
    }
}