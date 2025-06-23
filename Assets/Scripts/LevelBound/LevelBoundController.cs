using System;
using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class LevelBoundController : MonoBehaviour
    {
        [field: SerializeField] public ObjectID ID { get; private set; }
        [field: SerializeField] public ObjectCollision Collision { get; private set; }

        public void InitLevelBound()
        {
            Collision.CalculateCellGroup();
        }
    }
}