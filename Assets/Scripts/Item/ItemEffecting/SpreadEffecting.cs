using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public class SpreadEffecting : ItemEffecting
    {
        public override void Effect()
        {
            var pos = LevelManager.Instance.CurrentPaddle.Aiming.transform.position;

            BallSpawner.Instance.SpawnSingleBall(pos, Vector3.up);
            BallSpawner.Instance.SpawnSingleBall(pos, Quaternion.Euler(0, 0, 30) * Vector3.up);
            BallSpawner.Instance.SpawnSingleBall(pos, Quaternion.Euler(0, 0, -30) * Vector3.up);
        }
    }
}