using UnityEngine;

namespace BullBrukBruker
{
    public class BallMovingForward : ObjectMoving
    {
        protected override void SetObjectMovingConfigRecord()
            => config = ConfigsManager.Instance.ObjectMovingConfig.GetRecordByKeySearch(GetComponentInParent<BallController>().ID);

        protected override void CalculateTargetPosition()
        {
            targetPosition = transform.position + transform.up;
        }
    }
}