using UnityEngine;

namespace BullBrukBruker
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _reference;

        public static bool IsValid => _reference != null;

        public bool isDontDestroy;

        public static T Instance
        {
            get
            {
                if (!IsValid)
                {
                    _reference = FindAnyObjectByType<T>();
                    if (!IsValid) Debug.LogWarning($"No Instance of {typeof(T).Name} in this scene");
                }
                return _reference;
            }
        }

        protected virtual void Awake()
        {
            if (IsValid && _reference != this)
            {
                Destroy(gameObject);
                Debug.Log("Destroy");
            }
            else
            {
                _reference = (T)(MonoBehaviour)this;

                if (isDontDestroy)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }

    }
}