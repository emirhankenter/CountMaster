using UnityEngine;

namespace Mek.Utilities
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<T>();
            
                if (!Instance)
                {
                    Debug.LogError($"An instance of {typeof(T)} is needed in the scene, but there is none!");
                }
                else
                {
                    OnAwake();
                }
            }
            else if (Instance != this)
            {
                Destroy(gameObject); // destroy duplicate on load
            }
        }

        protected abstract void OnAwake();

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>();

                    if (!_instance)
                    {
                        Debug.LogError($"An instance of {typeof(T)} is needed in the scene, but there is none!");
                    }
                    else
                    {
                        SingletonBehaviour<T> instance = _instance as SingletonBehaviour<T>;
                        instance.OnAwake();
                    }
                }
                return _instance;
            }
            private set => _instance = value;
        }
    }
}
