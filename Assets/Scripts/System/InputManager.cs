using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;

namespace BullBrukBruker
{
    public class InputManager : SingletonMono<InputManager>
    {
        public IEnumerator InitInputManager()
        {
            EnhancedTouchSupport.Enable();
            TouchSimulation.Enable();

            yield return null;
        }

        private void Update()
        {
            if (!CalculateIsTouching()) return;
            CalculateInputPosition();
        }

        public bool CalculateIsTouching()
        {
            if (!EnhancedTouchSupport.enabled
                || UnityEngine.InputSystem.EnhancedTouch.Touch.activeFingers.Count == 0
                || EventSystem.current.IsPointerOverGameObject())
                return false;

            UnityEngine.InputSystem.EnhancedTouch.Touch touch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];

            return touch.isInProgress;
        }

        [HideInInspector] public Vector3 Position = Vector3.zero;
        private void CalculateInputPosition()
        {
            UnityEngine.InputSystem.EnhancedTouch.Touch touch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];

            Vector3 touchWorldPoint = ScreenManager.Instance.MainCamera.ScreenToWorldPoint(touch.screenPosition);

            Position.x = touchWorldPoint.x;
            Position.y = touchWorldPoint.y;
        }
    }
}
