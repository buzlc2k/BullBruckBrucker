using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class ScreenManager : SingletonMono<ScreenManager>
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
        public float ScreenWidth { get; private set; }
        public float ScreenHeight { get; private set; }

        public IEnumerator InitScreenManager()
        {
            while (MainCamera == null)
            {
                MainCamera = FindFirstObjectByType<Camera>();
                yield return null;
            }

            CalculateCurrentScreenSize();
            yield return null;
        }

        
        private void CalculateCurrentScreenSize()
        {
            float aspectRatio = (float) Screen.width / Screen.height;

            float minWidth = ConfigsManager.Instance.ScreenConfig.MinWidth;
            float minHeight = ConfigsManager.Instance.ScreenConfig.MinHeight;
            
            float normalizeWidth = (float) Screen.width / minWidth;
            float normalizeHeight = (float) Screen.height / minHeight;
            
            if (normalizeWidth >= normalizeHeight)
            {
                ScreenHeight = minHeight;
                ScreenWidth = minHeight * aspectRatio;
            }
            else
            {
                ScreenWidth = minWidth;
                ScreenHeight = minWidth / aspectRatio;
            }
            
            MainCamera.orthographicSize = ScreenHeight;
        }
    }
}