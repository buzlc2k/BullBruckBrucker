using UnityEngine;

namespace BullBrukBruker
{
    public class PaddleCollision : ObjectCollision
    {
        protected override void SetObjectCollisionConfigRecord()
            => Config = ConfigsManager.Instance.ObjectCollisionConfig.GetRecordByKeySearch(GetComponentInParent<PaddelController>().ID);

        protected override void InitializeCollisionHandles()
        {
            collisionHandles = new()
            {

            };
        }
    }
}