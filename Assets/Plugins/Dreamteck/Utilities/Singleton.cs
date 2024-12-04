namespace Dreamteck
{
    using System.Linq;
    using UnityEngine;

    public class Singleton<T> : PrivateSingleton<T> where T : Component
    {
        public static T instance => _instance ?? (_instance = FindFirstObjectByType<T>());
    }
}