using System.Linq;
using UnityEngine;

namespace Plugins.Dreamteck.Utilities
{
    public class Singleton<T> : PrivateSingleton<T> where T : Component
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Object.FindObjectsOfType<T>().FirstOrDefault();
                }

                return _instance;
            }
        }
    }
}