using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class ItemCollision : ObjectCollision
    {
        protected ItemEffecting effecting;
        protected ObjectDespawning despawning;

        protected override void Awake()
        {
            base.Awake();

            SetItemEffecting();
            SetObjectDespawning();
        }

        protected override void SetObjectCollisionConfigRecord()
        {
            Config = ConfigsManager.Instance.ObjectCollisionConfig.GetRecordByKeySearch(GetComponentInParent<ItemController>().ID);
        }

        protected virtual void SetItemEffecting() => effecting = GetComponentInParent<ItemController>().Effecting;
        protected virtual void SetObjectDespawning() => despawning = GetComponentInParent<ItemController>().Despawning;

        protected override void InitializeCollisionHandles()
        {
            collisionHandles = new()
            {
                [ObjectID.Paddle]
                = (objCollision => Utils.CheckRayOverlap(transform.parent.position, transform.parent.up, Config.Height * 2, objCollision.transform.parent.position, objCollision.Config.Width, objCollision.Config.Height),
                objCollision => OnCollideWithPaddle(objCollision)),

                [ObjectID.DeathBound]
                = (objCollision => Utils.CheckRayOverlap(transform.parent.position, transform.parent.up, Config.Height * 2, objCollision.transform.parent.position, objCollision.Config.Width, objCollision.Config.Height),
                objCollision => OnCollideDeathBound()),
            };
        }

        protected virtual void OnCollideDeathBound()
        {
            despawning.Despawn();
        }

        protected virtual void OnCollideWithPaddle(ObjectCollision paddleCollision)
        {
            effecting.Effect();
            despawning.Despawn();
        }
    }
}