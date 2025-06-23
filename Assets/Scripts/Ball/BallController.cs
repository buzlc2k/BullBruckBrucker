using System;
using UnityEngine;

namespace BullBrukBruker
{
    public class BallController : MonoBehaviour, IPooled
    {
        [field: SerializeField] public ObjectID ID { get; private set; }
        [field: SerializeField] public ObjectMoving Moving { get; private set; }
        [field: SerializeField] public ObjectCollision Collision { get; private set; }
        [field: SerializeField] public ObjectDespawning Despawning { get; private set; }

        public Action<GameObject> ReleaseCallback { get; set; }

        protected virtual void OnDisable()
        {
            ReleaseCallback?.Invoke(gameObject);
        }

        private void Update()
        {
            Moving.Move();

            Collision.CalculateCellGroup();
            Collision.CalculateCollision();
        }
    }
}