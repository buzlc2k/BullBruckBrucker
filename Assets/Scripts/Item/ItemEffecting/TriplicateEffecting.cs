using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BullBrukBruker
{
    public class TriplicateEffecting : ItemEffecting
    {
        public override void Effect()
        {
            var currentActiveBall = LevelManager.Instance.CurrentBalls.ToArray();
            
            BallSpawner.Instance.SpawnMultipleBalls(
                currentActiveBall.SelectMany(ball => new[]
                {
                    new BallSpawner.SpawnRequest(ball.transform.position, Quaternion.Euler(0, 0, 120) * ball.transform.up),
                    new BallSpawner.SpawnRequest(ball.transform.position, Quaternion.Euler(0, 0, -120) * ball.transform.up)
                }).ToList()
            );
        }
    }
}