using UnityEngine;

namespace _Scripts.Generic
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                if (_instance is not null) return _instance;
                _instance = (T)FindAnyObjectByType(typeof(T));
                switch (_instance)
                {
                    case null:
                        SetupInstance();
                        break;
                }
                return _instance;
            }
        }
        public virtual void Awake()
        {
            RemoveDuplicates();
        }
        private static void SetupInstance()
        {
            _instance = (T)FindAnyObjectByType(typeof(T));
            if (_instance is not null) return;
            var gameObj = new GameObject
            {
                name = typeof(T).Name
            };
            _instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
        private void RemoveDuplicates()
        {
            switch (_instance)
            {
                case null:
                    _instance = this as T;
                    DontDestroyOnLoad(gameObject);
                    break;
                default:
                    Destroy(gameObject);
                    break;
            }
        }
    }
}