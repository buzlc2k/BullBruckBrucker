using UnityEngine;

namespace BullBrukBruker
{
    public class PaddelMovingByInput : ObjectMoving
    {
        private float horizontalRange;

        protected override void Awake()
        {
            base.Awake();
            SetHorizontalRange();
        }

        protected override void SetObjectMovingConfigRecord() => config = ConfigsManager.Instance.ObjectMovingConfig.GetRecordByKeySearch(GetComponentInParent<PaddelController>().ID);

        private void SetHorizontalRange() => horizontalRange = ScreenManager.Instance.ScreenWidth - ConfigsManager.Instance.ObjectCollisionConfig.GetRecordByKeySearch(GetComponentInParent<PaddelController>().ID).Width;

        protected override void CalculateTargetPosition()
        {
            targetPosition = InputManager.Instance.Position;

            targetPosition.x = Mathf.Clamp(targetPosition.x, -horizontalRange, horizontalRange);
            targetPosition.y = -ScreenManager.Instance.ScreenHeight;
        }
    }
}