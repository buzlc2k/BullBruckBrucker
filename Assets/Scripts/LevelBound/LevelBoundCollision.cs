using UnityEngine;

namespace BullBrukBruker
{
    public class LevelBoundCollision : ObjectCollision
    {
        protected override void SetObjectCollisionConfigRecord()
        {
            Config = ConfigsManager.Instance.ObjectCollisionConfig.GetRecordByKeySearch(GetComponentInParent<LevelBoundController>().ID);
        }

        protected override void InitializeCollisionHandles()
        {
            collisionHandles = new()
            {
                
            };
        }
    }
}