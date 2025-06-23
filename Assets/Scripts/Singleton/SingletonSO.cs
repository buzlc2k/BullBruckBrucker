using Unity.VisualScripting;
using UnityEngine;

namespace BullBrukBruker
{
    public class SingletonSO<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var TAssets = Resources.LoadAll<T>($"Configs/");
                    if (TAssets == null || TAssets.Length < 1)
                        throw new System.Exception($"No instance of {typeof(T).Name} in resources");
                    else if (TAssets.Length > 1)
                        Debug.Log($"Multiple instance of {typeof(T).Name} in resources");

                    instance = TAssets[0];
                }

                return instance;
            }
        }
    }   
}